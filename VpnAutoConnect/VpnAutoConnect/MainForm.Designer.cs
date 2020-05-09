namespace VpnAutoConnect
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.registerPassPhraseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.idBox1 = new System.Windows.Forms.TextBox();
            this.buttonLog = new System.Windows.Forms.Button();
            this.idBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.customCommandButton = new System.Windows.Forms.Button();
            this.messageBox = new VpnAutoConnect.BlinkMessageBox();
            this.passLogicControl = new VpnAutoConnect.PassLogicControl();
            ((System.ComponentModel.ISupportInitialize)(this.passLogicControl)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConfig
            // 
            this.buttonConfig.BackgroundImage = global::VpnAutoConnect.Properties.Resources.icon_000010_256;
            this.buttonConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonConfig.Location = new System.Drawing.Point(355, 12);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(27, 23);
            this.buttonConfig.TabIndex = 1;
            this.buttonConfig.UseVisualStyleBackColor = true;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(282, 260);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(100, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "VPN接続";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // registerPassPhraseButton
            // 
            this.registerPassPhraseButton.Location = new System.Drawing.Point(12, 12);
            this.registerPassPhraseButton.Name = "registerPassPhraseButton";
            this.registerPassPhraseButton.Size = new System.Drawing.Size(109, 23);
            this.registerPassPhraseButton.TabIndex = 5;
            this.registerPassPhraseButton.Text = "パスフレーズ登録";
            this.registerPassPhraseButton.UseVisualStyleBackColor = true;
            this.registerPassPhraseButton.Click += new System.EventHandler(this.registerPassPhraseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "ID1";
            // 
            // idBox1
            // 
            this.idBox1.Location = new System.Drawing.Point(155, 209);
            this.idBox1.Name = "idBox1";
            this.idBox1.Size = new System.Drawing.Size(111, 19);
            this.idBox1.TabIndex = 8;
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(296, 12);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(53, 23);
            this.buttonLog.TabIndex = 9;
            this.buttonLog.Text = "Log";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // idBox2
            // 
            this.idBox2.Location = new System.Drawing.Point(155, 234);
            this.idBox2.Name = "idBox2";
            this.idBox2.PasswordChar = '*';
            this.idBox2.Size = new System.Drawing.Size(111, 19);
            this.idBox2.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "ID2";
            // 
            // customCommandButton
            // 
            this.customCommandButton.Location = new System.Drawing.Point(282, 207);
            this.customCommandButton.Name = "customCommandButton";
            this.customCommandButton.Size = new System.Drawing.Size(100, 23);
            this.customCommandButton.TabIndex = 12;
            this.customCommandButton.Text = "カスタムコマンド";
            this.customCommandButton.UseVisualStyleBackColor = true;
            this.customCommandButton.Click += new System.EventHandler(this.customCommandButton_Click);
            // 
            // messageBox
            // 
            this.messageBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.messageBox.Location = new System.Drawing.Point(12, 261);
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.Size = new System.Drawing.Size(254, 19);
            this.messageBox.TabIndex = 6;
            // 
            // passLogicControl
            // 
            this.passLogicControl.Location = new System.Drawing.Point(12, 41);
            this.passLogicControl.Name = "passLogicControl";
            this.passLogicControl.Size = new System.Drawing.Size(370, 151);
            this.passLogicControl.TabIndex = 3;
            this.passLogicControl.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 294);
            this.Controls.Add(this.customCommandButton);
            this.Controls.Add(this.idBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.idBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.registerPassPhraseButton);
            this.Controls.Add(this.passLogicControl);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "VPN Auto Connect";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.passLogicControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button buttonConnect;
        private PassLogicControl passLogicControl;
        private System.Windows.Forms.Button registerPassPhraseButton;
        private BlinkMessageBox messageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idBox1;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.TextBox idBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button customCommandButton;
    }
}

