namespace KeyInputMacro
{
    partial class LogWindow
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
            LogList = new ListBox();
            SuspendLayout();
            // 
            // LogList
            // 
            LogList.BorderStyle = BorderStyle.None;
            LogList.Dock = DockStyle.Fill;
            LogList.FormattingEnabled = true;
            LogList.HorizontalScrollbar = true;
            LogList.IntegralHeight = false;
            LogList.ItemHeight = 17;
            LogList.Location = new Point(0, 0);
            LogList.Name = "LogList";
            LogList.Size = new Size(431, 375);
            LogList.TabIndex = 0;
            // 
            // LogWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(431, 375);
            Controls.Add(LogList);
            MaximizeBox = false;
            MinimumSize = new Size(233, 294);
            Name = "LogWindow";
            ShowIcon = false;
            Text = "按键输入宏 - 执行日志";
            FormClosing += LogWindow_FormClosing;
            Shown += LogWindow_Shown;
            ResumeLayout(false);
        }

        #endregion

        private ListBox LogList;
    }
}