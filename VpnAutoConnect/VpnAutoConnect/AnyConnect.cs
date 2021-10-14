using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace VpnAutoConnect
{
    public class AnyConnect
    {
        /// <summary>
        /// Cisco AnyConnect CLI実行ファイルのパス
        /// </summary>
        private string _vpncliPath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vpncliPath"></param>
        public AnyConnect(string vpncliPath)
        {
            _vpncliPath = vpncliPath;
        }

        // Debugメッセージ
        public event EventHandler DebugMessage;
        protected virtual void OnDebugMessage(string text)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(text));
        }
        protected virtual void OnDebugMessage(object sender, EventArgs e)
        {
            DebugMessage?.Invoke(sender, e);
        }
        protected virtual void OnDebugMessage(string message, bool lineFeed, bool timeStamp)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(message, lineFeed, timeStamp));
        }

        /// <summary>
        /// 登録されているhostを取得
        /// </summary>
        /// <param name="vpncliPath"></param>
        /// <returns></returns>
        private async Task<string> Host()
        {
            try
            {
                // 子プロセス起動
                ProcessStartInfo psi = new ProcessStartInfo(_vpncliPath);

                psi.UseShellExecute = false; // シェルを使用せず子プロセスを起動する(リダイレクトするために必要)
                psi.RedirectStandardOutput = true; // 子プロセスの標準出力をリダイレクトする
                psi.CreateNoWindow = true;
                psi.Arguments = "host";

                using (Process child = Process.Start(psi))
                {
                    // 出力読み込み
                    string content = await child.StandardOutput.ReadToEndAsync();
                    var sr = new StringReader(content);
                    var line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith(">"))
                        {
                            return line.Replace(">", "").Trim();
                        }
                    }
                }
                throw new Exception("登録されたホストの取得に失敗しました。（コマンド出力が読み取れません）");
            }
            catch (Exception ex)
            {
                throw new Exception($"登録されたホストの取得に失敗しました。（{ex.Message}）");
            }
        }

        /// <summary>
        /// statusを取得
        /// </summary>
        /// <param name="vpncliPath"></param>
        /// <returns></returns>
        public async Task<VpnStatus> Status()
        {
            // 子プロセス起動
            ProcessStartInfo psi = new ProcessStartInfo(_vpncliPath);

            psi.UseShellExecute = false; // シェルを使用せず子プロセスを起動する(リダイレクトするために必要)
            psi.RedirectStandardOutput = true; // 子プロセスの標準出力をリダイレクトする
            psi.CreateNoWindow = true;
            psi.Arguments = "status";

            using (Process child = Process.Start(psi))
            {
                // 出力読み込み
                string content = await child.StandardOutput.ReadToEndAsync();

                // Status取得
                foreach (var st in Enum.GetNames(typeof(VpnStatus)))
                {
                    if (content.Contains($"state: {st}"))
                        return (VpnStatus)Enum.Parse(typeof(VpnStatus), st);
                }
            }
            return VpnStatus.Unknown;
        }

        /// <summary>
        /// VPN接続状態
        /// </summary>
        public enum VpnStatus
        {
            Disconnected,
            Connected,
            Disconnecting,
            Reconnecting,
            Connecting,
            Unknown,
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task Connect(string password)
        {
            // すでにConnected状態であれば何もしない
            var status = await Status();
            if (status == VpnStatus.Connected) return;

            // Disconnected以外の状態は想定していないので、自動ログイン停止
            if (status != VpnStatus.Disconnected) throw new Exception($"接続状態が{status}です。");

            // 関連するプロセスをKill
            await Task.Run(() => KillAll());

            // 登録されているホスト名取得
            var host = await Host();
            OnDebugMessage($"ホスト名: {host}");

            // 接続用にCLIを-sオプションで起動
            ProcessStartInfo psi = new ProcessStartInfo(_vpncliPath);
            psi.UseShellExecute = false; // シェルを使用せず子プロセスを起動する(リダイレクトするために必要)
            psi.RedirectStandardOutput = true; // 子プロセスの標準出力をリダイレクトする
            psi.RedirectStandardInput = true; // 子プロセスの標準入力をリダイレクトする
            psi.CreateNoWindow = true;
            psi.Arguments = "-s";

            using (Process child = Process.Start(psi))
            {
                try
                {
                    // リアルタイムでStdout読み込み
                    var rtRead = new RtRead(child.StandardOutput);
                    rtRead.DebugMessage += DebugMessage;

                    // VPN> 待ち
                    await rtRead.WaitFor(new string[] { "VPN> ", "error: " }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));
                    var str = rtRead.Read();
                    if (!str.EndsWith("VPN> "))
                        throw new Exception($"「VPN>」プロンプトが確認できませんでした。");
                    if (str.Contains("error: "))
                        throw new Exception($"「VPN>」プロンプト待機中にエラーが発生しました。{str}");

                    // connectコマンド入力
                    await child.StandardInput.WriteLineAsync($"connect {host}");
                    OnDebugMessage($"", true, false);
                    OnDebugMessage($"connect {host}");

                    // Username待ち
                    await rtRead.WaitFor(new string[] {@"Group: \[.*\] ",  @"Username: \[.*\] ", @"error: ", @"Connect Anyway\? \[y/n\]: " }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));
                    str = rtRead.Read();

                    if (str.Contains("Group: "))
                    {
                        // vpnモード選択が出たら、改行を入力して再びUsername待ち
                        await child.StandardInput.WriteLineAsync($"");
                        await rtRead.WaitFor(new string[] { @"Username: \[.*\] ", "error: " }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));
                        str = rtRead.Read();
                    }

                    if (str.Contains("Connect Anyway? [y/n]: "))
                    {
                        // 証明書の警告が出たら「y」を入力して再びUsername待ち
                        await child.StandardInput.WriteLineAsync($"y");
                        await rtRead.WaitFor(new string[] { @"Username: \[.*\] ", "error: ", @"Always trust this server and import the certificate\? \[y/n\]: " }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));
                        str = rtRead.Read();

                        // さらに証明書のエラーなら、「y」を入力して再びUsername待ち
                        if (str.Contains(@"Always trust this server and import the certificate? [y/n]: "))
                        {
                            await child.StandardInput.WriteLineAsync($"y");
                            await rtRead.WaitFor(new string[] { @"Username: \[.*\] ", "error: "}, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));
                            str = rtRead.Read();
                        }
                    }
                    if (str.Contains("error: "))
                        throw new Exception($"「Username: [] 」プロンプト待機中にエラーが発生しました。{str}");
                    if (!str.EndsWith("] ") || !new Regex(@"Username: \[.*\] ").IsMatch(str))
                        throw new Exception($"「Username: [] 」プロンプトが確認できませんでした。{str}");

                    // Username入力
                    await child.StandardInput.WriteLineAsync($"");

                    // Password待ち
                    await rtRead.WaitFor(new string[] { @"Password: ", "error: " }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));
                    str = rtRead.Read();
                    if (str.Contains("error: "))
                        throw new Exception($"「Password: 」プロンプト待機中にエラーが発生しました。{str}");
                    if (!str.EndsWith(@"Password: "))
                        throw new Exception($"「Password: 」プロンプトが確認できませんでした。{str}");

                    // Password入力
                    await child.StandardInput.WriteLineAsync($"{password}");

                    // Connected 待ち
                    await rtRead.WaitFor(new string[] { @"state: Connected", "error: ", @"Login failed" }, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));
                    str = rtRead.Read();
                    if (str.Contains("Login failed"))
                        throw new Exception($"ログインに失敗しました。パスワードとIDを確認してください。");
                    if (str.Contains("error: "))
                        throw new Exception($"接続待ち中にエラーが発生しました。{str}");
                    if (!str.Contains("state: Connected"))
                        throw new Exception($"接続が完了できませんでした。");
                    if (!str.EndsWith("VPN> "))
                        throw new Exception($"「VPN> 」プロンプトが確認できませんでした。");

                    // コマンド終了
                    await child.StandardInput.WriteLineAsync($"quit");
                }
                finally
                {
                    if (!child.HasExited)
                    {
                        // 子プロセスが終了していなければ、500ms待ち、それでも終了しなければkill
                        await Task.Delay(500);
                        if (!child.HasExited)
                            try
                            {
                                child.Kill();
                            }
                            catch { }   // Killでは例外を発生させない
                    }

                    // 念のためすべての関連プロセスkill
                    KillAll();
                }
            }
        }

        /// <summary>
        /// 切断
        /// </summary>
        /// <param name="vpncliPath"></param>
        /// <returns></returns>
        public async Task<bool> Disconnect()
        {
            // 子プロセス起動
            ProcessStartInfo psi = new ProcessStartInfo(_vpncliPath);

            psi.UseShellExecute = false; // シェルを使用せず子プロセスを起動する(リダイレクトするために必要)
            psi.RedirectStandardOutput = true; // 子プロセスの標準出力をリダイレクトする
            psi.CreateNoWindow = true;
            psi.Arguments = "disconnect";

            using (Process child = Process.Start(psi))
            {
                // 出力読み込み
                string content = await child.StandardOutput.ReadToEndAsync();
                OnDebugMessage(content);

                if (content.Contains("Disconnected"))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// VPN関連プロセスをすべてkill
        /// </summary>
        /// <returns></returns>
        public static void KillAll()
        {
            var ps = Process.GetProcessesByName("vpncli").ToList();
            ps.AddRange(Process.GetProcessesByName("vpnui"));
            foreach (var p in ps)
            {
                try
                {
                    p.Kill();
                }
                catch { }   // Killでは例外を発生させない
            }
        }
    }
}
