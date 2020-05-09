using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VpnAutoConnect
{
    /// <summary>
    /// 
    /// </summary>
    public class WatchDog
    {
        public delegate Task<bool> WatchFunction();
        public delegate Task IfFailFunction();
        private readonly TimeSpan WATCH_DURATION = TimeSpan.FromSeconds(10);
        private CancellationTokenSource _cts { get; set; }
        private WatchFunction _watch { get; set; }
        private IfFailFunction _ifFail { get; set; }

        // Debugメッセージ
        public event EventHandler DebugMessage;
        protected virtual void OnDebugMessage(string text)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(text));
        }

        public WatchDog(WatchFunction watch, IfFailFunction ifFail)
        {
            _watch = watch;
            _ifFail = ifFail;
        }

        public async Task Start()
        {
            OnDebugMessage($"WatchDog: Start");
            _cts = new CancellationTokenSource();

            try
            {
               while (!_cts.IsCancellationRequested)
                {
                    await Task.Delay(WATCH_DURATION, _cts.Token);
                    if (!_cts.IsCancellationRequested && !await _watch() && !_cts.IsCancellationRequested)
                    {
                        OnDebugMessage($"WatchDog: Fail");
                        await _ifFail();
                        OnDebugMessage($"WatchDog: ReStart");
                    }
                    else
                    {
                        OnDebugMessage($"WatchDog: CheckPoint");
                    }
                }
            }
            catch(OperationCanceledException)
            {
                // キャンセルされた場合、ここに来ることもある
            }
 
            OnDebugMessage($"WatchDog: Exit");
        }

        public void Stop()
        {
            OnDebugMessage($"WatchDog: Cancel");
            _cts.Cancel();
        }
    }
}
