using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace VpnAutoConnect
{
    /// <summary>
    /// StreamReaderを実時間で読み、バッファリングするクラス
    /// </summary>
    internal class RtRead
    {
        /// <summary>
        /// RtReadが読み込むStream
        /// </summary>
        private StreamReader _stdout { get; }

        /// <summary>
        /// Streamを読み込むプロセス
        /// </summary>
        private Task _task { get; set; }

        /// <summary>
        /// 読み込み中のstring
        /// </summary>
        private string _content { get; set; }

        /// <summary>
        /// _contentの排他ロック
        /// </summary>
        private object _lock { get; set; }

        // Debugメッセージ
        public event EventHandler DebugMessage;
        protected virtual void OnDebugMessage(string message)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(message));
        }
        protected virtual void OnDebugMessage(string message, bool lineFeed, bool timeStamp)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(message, lineFeed, timeStamp));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stdout"></param>
        public RtRead(StreamReader stdout)
        {
            this._stdout = stdout;
            _lock = new object();
            _content = "";
            _lastUpdate = DateTime.Now;

            _task = Task.Run(() => job());
        }

        /// <summary>
        /// _contentが最後に更新された時刻
        /// </summary>
        private DateTime _lastUpdate { get; set; }

        /// <summary>
        /// Streamから一文字づつ読み込んで、_contentに書き込み続けるプロセス
        /// </summary>
        /// <returns></returns>
        private async Task job()
        {
            var buf = new char[1];

            while (await _stdout.ReadAsync(buf, 0, buf.Length) > 0)
            {
                OnDebugMessage($"{new string(buf)}", false, false);
                lock(_lock)
                {
                    _content += new string(buf);
                }
                _lastUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// これまでに読み込まれた_contentを取得して_contentクリア
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            lock(_lock)
            {
                var ret = _content;
                _content = "";
                return ret;
            }
        }

        /// <summary>
        /// これまでに読み込まれた_contentを取得。_contentはそのまま。
        /// </summary>
        /// <returns></returns>
        public string Peek()
        {
            lock (_lock)
            {
                return new string(_content.ToCharArray());
            }
        }

        /// <summary>
        /// _contentがpatterns条件（正規表現）のいずれかを満たすまで待機
        /// _contentがminWait時間以上更新されなくなったときに条件照合開始
        /// maxWait時間経過するとOperationCanceledExceptionを投げて終了
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="minWait"></param>
        /// <returns></returns>
        public async Task WaitFor(string[] patterns, TimeSpan minWait, TimeSpan maxWait)
        {
            // 正規表現パーサ作成
            var regs = new List<Regex>();
            foreach(var pattern in patterns)
            {
                regs.Add(new Regex(pattern));
            }

            // maxWait時間経過後キャンセルされるToken
            var cts = new CancellationTokenSource();
            cts.CancelAfter(maxWait);

            while(!cts.IsCancellationRequested)
            {
                // _lastUpdateからminWait時間経過するまで待つ
                var lastUpdate = _lastUpdate;
                var timeSinceLastUpdate = DateTime.Now - lastUpdate;
                if(timeSinceLastUpdate < minWait)
                    await Task.Delay(minWait - timeSinceLastUpdate, cts.Token);
                
                foreach (var reg in regs)
                {
                    if (reg.IsMatch(Peek())) return;    // 条件にマッチすれば正常終了
                }

                // _contextがアップデートされるのを待つ
                while(lastUpdate == _lastUpdate && !cts.IsCancellationRequested)
                    await Task.Delay(100, cts.Token);
            }

            throw new OperationCanceledException();
        }
    }
}