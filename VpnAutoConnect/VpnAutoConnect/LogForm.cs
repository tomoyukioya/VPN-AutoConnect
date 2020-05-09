using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnAutoConnect
{
    /// <summary>
    /// デバッグログウィンド
    /// </summary>
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public void OnDebugMessage(object sender, EventArgs e)
        {
            if (!textBox.IsHandleCreated) return;

            var args = (DebugLogMessageEventArgs)e;
            var message = args.Message;
            if (args.TimeStamp) message = DateTime.Now.ToLongTimeString() + ": " + message;
            if (args.LineFeed) message += "\r\n";

            if (textBox.InvokeRequired)
                Invoke(new Action<string>(textBox.AppendText), message);
            else
                textBox.AppendText(message);
        }

        /// <summary>
        /// 右上のクローズボタンでは終了させない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }

    public class DebugLogMessageEventArgs: EventArgs
    {
        public DebugLogMessageEventArgs(string message) : this(message, true, true)
        {
        }
        public DebugLogMessageEventArgs(string message, bool lineFeed): this(message, lineFeed, true)
        {
        }
        public DebugLogMessageEventArgs(string message, bool lineFeed, bool timeStamp)
        {
            Message = message;
            LineFeed = lineFeed;
            TimeStamp = timeStamp;
        }

        public string Message { get; set; }
        public bool LineFeed { get; set; }
        public bool TimeStamp { get; set; }
    }
}
