using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnAutoConnect
{
    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// メイン画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {

            // ログ窓
            _logForm = new LogForm();
            _logForm.WindowState = FormWindowState.Minimized;
            _logForm.Show();
            _logForm.Visible = false;

            // 設定フォーム
            _settings = new SettingsForm();
            _settings.DebugMessage += DebugMessage;

            // カスタムコマンド、カスタムラベルロード
            label1.Text = ConfigurationManager.AppSettings["LabelId1"];
            label2.Text = ConfigurationManager.AppSettings["LabelId2"];
            customCommandButton.Text = ConfigurationManager.AppSettings["LabelCustomCommandButton"];
        }

        #region パスフレーズ登録

        /// <summary>
        /// パスフレーズ登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void registerPassPhraseButton_Click(object sender, EventArgs e)
        {
            if (!passLogicControl.IsPassPhraseRegistering)
            {
                // パスフレーズ登録開始
                OnDebugMessage("パスフレーズ登録開始");
                registerPassPhraseButton.Text = "登録終了";
                passLogicControl.BeginRegister();
                await messageBox.StartBlink("パスフレーズ登録中...");
                passLogicControl.EndRegister();
            }
            else
            {
                // パスフレーズ登録終了
                OnDebugMessage("パスフレーズ登録終了");
                registerPassPhraseButton.Text = "パスフレーズ登録*";
                messageBox.StopBlink();
            }
        }

        #endregion

        #region 設定ボタン

        /// <summary>
        /// 設定ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            _settings.ShowDialog();
        }
        private SettingsForm _settings;

        #endregion

        #region デバッグログ

        // デバッグメッセージのイベントハンドラ
        private void DebugMessage(object sender, EventArgs e)
        {
            _logForm?.OnDebugMessage(sender, e);
        }
        private void OnDebugMessage(string message)
        {
            DebugMessage(this, new DebugLogMessageEventArgs(message));
        }

        /// <summary>
        /// ログ窓表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLog_Click(object sender, EventArgs e)
        {
            if (!_logForm.Visible)
            {
                _logForm.Visible = true;
                _logForm.WindowState = FormWindowState.Normal;
            }
            else if (_logForm.WindowState == FormWindowState.Minimized)
                _logForm.WindowState = FormWindowState.Normal;
            else if (_logForm.WindowState == FormWindowState.Normal)
                _logForm.WindowState = FormWindowState.Minimized;
        }

        private LogForm _logForm { get; set; }

        #endregion

        #region VPN接続

        /// <summary>
        /// VPN接続
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_connected)
                {
                    // 接続

                    // AnyConnectCliPath
                    if (!File.Exists(_settings.AnyConnectCliPath))
                    {
                        MessageBox.Show("AnyConnect CLIのインストールパスが正しく設定されていません。", "警告", MessageBoxButtons.OK);
                        return;
                    }

                    // ID
                    if (idBox1.Text == "")
                    {
                        MessageBox.Show($"{label1.Text}が正しく設定されていません。", "警告", MessageBoxButtons.OK);
                        return;
                    }

                    // PassLogicUrl
                    Uri uriResult;
                    if (!Uri.TryCreate(_settings.PassLogicUrl(idBox1.Text, idBox2.Text), UriKind.Absolute, out uriResult)
                        || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                    {
                        MessageBox.Show($"PassLogicキーのURLが正しく設定されていません。", "警告", MessageBoxButtons.OK);
                        return;
                    }

                    // PassPhrase
                    if (!passLogicControl.PassPhraseRegistered)
                    {
                        MessageBox.Show($"パスフレーズが登録されていません。", "警告", MessageBoxButtons.OK);
                        return;
                    }

                    // 接続処理スタート
                    OnDebugMessage($"接続開始");
                    buttonConnect.Enabled = false;

                    // 成功するまで繰り返し
                    while (true)
                    {
                        try
                        {
                            if (await Connect()) break;
                        }
                        catch (AutoPilotContinueException ex)
                        {
                            OnDebugMessage($"ログイン継続：{ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            OnDebugMessage($"ログイン中断：{ex.Message}");
                            MessageBox.Show($"接続エラー：{ex.Message}", "エラー", MessageBoxButtons.OK);
                            return;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }

                    // 接続成功
                    _connected = true;
                    _watchDog = new WatchDog(
                        async () => await _anyConnect.Status() != AnyConnect.VpnStatus.Disconnected,
                        async () => await Connect());
                    _watchDog.DebugMessage += DebugMessage;
                    await _watchDog.Start();
                }
                else
                {
                    // 切断
                    if (_anyConnect == null) return;
                    buttonConnect.Enabled = false;
                    var task = messageBox.StartBlink("切断中...");

                    try
                    {
                        if (!await _anyConnect.Disconnect())
                        {
                            // 切断失敗
                            throw new Exception("_anyConnect.Disconnect()に失敗しました");
                        }
                        else
                        {
                            // 切断成功
                            OnDebugMessage($"切断完了");
                            buttonConnect.Text = "VPN接続";
                            _connected = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 切断失敗
                        OnDebugMessage($"切断失敗");
                        MessageBox.Show($"切断エラー：{ex.Message}", "エラー", MessageBoxButtons.OK);
                    }

                    messageBox.StopBlink();
                    await task;
                    messageBox.Text = "";

                    if (_watchDog != null)
                    {
                        _watchDog.Stop();
                        _watchDog = null;
                    }
                    buttonConnect.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"VPN接続中に予期せぬエラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK);
            }
        }
        private WatchDog _watchDog { get; set; }

        /// <summary>
        /// 接続処理
        /// </summary>
        /// <param name="interactive"></param>
        /// <returns></returns>
        private async Task<bool> Connect()
        {
            try
            {
                // AnyConnect起動
                _anyConnect = new AnyConnect(_settings.AnyConnectCliPath);
                _anyConnect.DebugMessage += DebugMessage;

                // 接続状態確認
                var status = await _anyConnect.Status();
                if (status == AnyConnect.VpnStatus.Disconnected)
                {
                    // PassLogicパターンテーブル取得
                    var task = messageBox.StartBlink("ワンタイムパスワード取得中...");
                    var table = await new PassLogicPattern(_settings.PassLogicUrl(idBox1.Text, idBox2.Text)).GetTable();
                    OnDebugMessage($"ワンタイムパスワード取得:{string.Join(",", table)}");
                    messageBox.StopBlink();
                    await task;

                    // パスワード取得
                    var pass = passLogicControl.GetPassword(table);

                    // AnyConnect起動
                    task = messageBox.StartBlink("Connecting...");
                    _anyConnect = new AnyConnect(_settings.AnyConnectCliPath);
                    _anyConnect.DebugMessage += DebugMessage;

                    // 接続
                    await _anyConnect.Connect(pass);

                    // 接続成功
                    messageBox.StopBlink();
                    await task;
                    messageBox.Text = "Connected";
                    buttonConnect.Text = "VPN切断";
                    OnDebugMessage($"接続完了");
                    return true;
                }
                else if (status == AnyConnect.VpnStatus.Connected)
                {
                    // すでに接続済み
                    OnDebugMessage($"すでに接続済みです");
                    messageBox.Text = "Connected";
                    buttonConnect.Text = "VPN切断";
                    return true;
                }
                else
                {
                    // その他のステータス
                    throw new Exception($"接続状態が{status}です");
                }
            }
            catch (AutoPilotContinueException ex)
            {
                // 自動ログインを継続可能な例外の場合、falseを返す
                OnDebugMessage(ex.ToString());
                messageBox.StopBlink();
                return false;
            }
            catch (Exception)
            {
                messageBox.StopBlink();
                messageBox.Text = "";
                throw;
            }
            finally
            {
                buttonConnect.Enabled = true;
            }
        }

        private async Task<bool> Disconnect()
        {
            // 切断
            if (_anyConnect == null) return true;
            buttonConnect.Enabled = false;
            var task = messageBox.StartBlink("切断中...");

            try
            {
                if (!await _anyConnect.Disconnect()) return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                messageBox.StopBlink();
                await task;
                messageBox.Text = "";
                buttonConnect.Enabled = true;
            }

            OnDebugMessage($"切断完了");
            buttonConnect.Text = "VPN接続";
            return true;
        }

        private bool _connected { get; set; }
        private AnyConnect _anyConnect { get; set; }


        #endregion

        #region カスタムコマンド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customCommandButton_Click(object sender, EventArgs e)
        {
            // AnyConnectCliPath
            if (!File.Exists(_settings.CustomCommand))
            {
                MessageBox.Show("カスタムコマンドのパスが正しく設定されていません。", "警告", MessageBoxButtons.OK);
                return;
            }

            // IDが設定されているかチェック
            if (idBox1.Text == "")
            {
                MessageBox.Show($"{label1.Text}が正しく設定されていません。", "警告", MessageBoxButtons.OK);
                return;
            }
            if (idBox2.Text == "")
            {
                MessageBox.Show($"{label2.Text}が正しく設定されていません。", "警告", MessageBoxButtons.OK);
                return;
            }

            // コマンド起動(立ち上げっぱなしで特に回収はしない)
            var psi = new ProcessStartInfo(_settings.CustomCommand);
            psi.UseShellExecute = true;
            psi.CreateNoWindow = true;
            psi.Arguments = _settings.CustomCommandParameters(idBox1.Text, idBox2.Text);

            var child = Process.Start(psi);
        }

        #endregion
    }
}
