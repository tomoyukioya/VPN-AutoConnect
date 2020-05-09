using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnAutoConnect
{
    /// <summary>
    /// テキストBlink対応のTextBox
    /// </summary>
    public partial class BlinkMessageBox : TextBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlinkMessageBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private CancellationTokenSource _cts { get; set; }

        /// <summary>
        /// 指定したテキストをBlink表示
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task  StartBlink(string text)
        {
            if (InvokeRequired)
                await (Task)Invoke(new Func<string, Task>(_StartBlink), text);
            else
                await _StartBlink(text);
        }
        private async Task _StartBlink(string text)
        {
            _cts = new CancellationTokenSource();

            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    Text = text;
                    await Task.Delay(300, _cts.Token);
                    Text = "";
                    await Task.Delay(300, _cts.Token);
                }
            }
            catch(TaskCanceledException)
            { }
            finally
            {
                Text = "";
            }
        }

        /// <summary>
        /// Blink表示停止
        /// </summary>
        public void StopBlink()
        {
            _cts.Cancel();
        }
    }
}
