using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnAutoConnect
{
    /// <summary>
    /// 各種パラメータ設定用のフォーム
    /// </summary>
    public partial class SettingsForm : Form
    {
        /// <summary>
        /// PassLogicワンタイムパスワード取得用URL
        /// '{ID}'置換前の入力値
        /// </summary>
        public string PassLogicRawUrl { get; set; }

        /// <summary>
        /// PassLogicワンタイムパスワード取得用URL
        /// '{ID}'置換後
        /// </summary>
        public string PassLogicUrl(string ID1, string ID2)
        {
            return PassLogicRawUrl.Replace("{ID1}", ID1).Replace("{ID2}", ID2);
        }

        /// <summary>
        /// AnyConnect CLI（vpncli.exe）のパス
        /// </summary>
        public string AnyConnectCliPath { get; set; }

        /// <summary>
        /// カスタムコマンド
        /// </summary>
        public string CustomCommand { get; set; }

        /// <summary>
        /// カスタムコマンドパラメータ
        /// '{ID}'置換前の入力値
        /// </summary>
        public string CustomCommandParametersRaw { get; set; }

        /// <summary>
        /// カスタムコマンドパラメータ
        /// '{ID}'置換後
        /// </summary>
        public string CustomCommandParameters(string ID1, string ID2)
        {
            return CustomCommandParametersRaw.Replace("{ID1}", ID1).Replace("{ID2}", ID2);
        }

        // Debugメッセージ
        public event EventHandler DebugMessage;
        protected virtual void OnDebugMessage(string text)
        {
            DebugMessage?.Invoke(this, new DebugLogMessageEventArgs(text));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
            PassLogicRawUrl = ConfigurationManager.AppSettings["PassLogicUrl"];
            AnyConnectCliPath = ConfigurationManager.AppSettings["AnyConnectCliPath"];
            CustomCommand = ConfigurationManager.AppSettings["CustomCommand"];
            CustomCommandParametersRaw= ConfigurationManager.AppSettings["CustomCommandParameters"];
            settingsFormBindingSource.DataSource = this;
        }

        /// <summary>
        /// 保存ボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["PassLogicUrl"].Value = PassLogicRawUrl;
            settings["AnyConnectCliPath"].Value = AnyConnectCliPath;
            settings["CustomCommand"].Value = CustomCommand;
            settings["CustomCommandParameters"].Value = CustomCommandParametersRaw;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            OnDebugMessage($"設定保存： PassLogicUrl={PassLogicRawUrl}, AnyConnectCliPath={AnyConnectCliPath}, CustomCommand={CustomCommand} {CustomCommandParametersRaw}");
            Close();
        }

        /// <summary>
        /// キャンセルボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            PassLogicRawUrl = ConfigurationManager.AppSettings["PassLogicUrl"];
            AnyConnectCliPath = ConfigurationManager.AppSettings["AnyConnectCliPath"];
            CustomCommand = ConfigurationManager.AppSettings["CustomCommand"];
            CustomCommandParametersRaw = ConfigurationManager.AppSettings["CustomCommandParameters"];
            Close();
        }
    }
}
