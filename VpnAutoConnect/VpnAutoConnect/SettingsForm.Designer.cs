namespace VpnAutoConnect
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.passLogicUrlBox = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.anyConnectPathBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.customCommandBox = new System.Windows.Forms.TextBox();
            this.customCommandParameters = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.settingsFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.settingsFormBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "PassLogicキーのURL\r\n(\'{ID}\'が設定されたIDで置換されます)";
            // 
            // passLogicUrlBox
            // 
            this.passLogicUrlBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsFormBindingSource, "PassLogicRawUrl", true));
            this.passLogicUrlBox.Location = new System.Drawing.Point(209, 16);
            this.passLogicUrlBox.Name = "passLogicUrlBox";
            this.passLogicUrlBox.Size = new System.Drawing.Size(545, 19);
            this.passLogicUrlBox.TabIndex = 1;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(597, 166);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(678, 166);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "AnyConnectCLIのインストールパス";
            // 
            // anyConnectPathBox
            // 
            this.anyConnectPathBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsFormBindingSource, "AnyConnectCliPath", true));
            this.anyConnectPathBox.Location = new System.Drawing.Point(209, 51);
            this.anyConnectPathBox.Name = "anyConnectPathBox";
            this.anyConnectPathBox.Size = new System.Drawing.Size(545, 19);
            this.anyConnectPathBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "カスタムコマンド";
            // 
            // customCommandBox
            // 
            this.customCommandBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsFormBindingSource, "CustomCommand", true));
            this.customCommandBox.Location = new System.Drawing.Point(209, 88);
            this.customCommandBox.Name = "customCommandBox";
            this.customCommandBox.Size = new System.Drawing.Size(545, 19);
            this.customCommandBox.TabIndex = 7;
            // 
            // customCommandParameters
            // 
            this.customCommandParameters.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsFormBindingSource, "CustomCommandParametersRaw", true));
            this.customCommandParameters.Location = new System.Drawing.Point(209, 124);
            this.customCommandParameters.Name = "customCommandParameters";
            this.customCommandParameters.Size = new System.Drawing.Size(545, 19);
            this.customCommandParameters.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "カスタムコマンド引数\r\n(\'{ID}\'が設定されたIDで置換されます)";
            // 
            // settingsFormBindingSource
            // 
            this.settingsFormBindingSource.DataSource = typeof(VpnAutoConnect.SettingsForm);
            // 
            // settingsFormBindingSource1
            // 
            this.settingsFormBindingSource1.DataSource = typeof(VpnAutoConnect.SettingsForm);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 201);
            this.Controls.Add(this.customCommandParameters);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.customCommandBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.anyConnectPathBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.passLogicUrlBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passLogicUrlBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox anyConnectPathBox;
        private System.Windows.Forms.BindingSource settingsFormBindingSource;
        private System.Windows.Forms.BindingSource settingsFormBindingSource1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox customCommandBox;
        private System.Windows.Forms.TextBox customCommandParameters;
        private System.Windows.Forms.Label label4;
    }
}