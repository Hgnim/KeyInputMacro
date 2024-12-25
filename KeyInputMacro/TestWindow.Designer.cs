namespace KeyInputMacro
{
    partial class TestWindow
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
            OutputBox = new ListBox();
            MainMenuStrip = new MenuStrip();
            MainMenuStrip_Options = new ToolStripMenuItem();
            MainMenuStrip_Options_MouseMoveCheck = new ToolStripMenuItem();
            MainMenuStrip_Options_MouseWheelCheck = new ToolStripMenuItem();
            MainMenuStrip_Clear = new ToolStripMenuItem();
            MainMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // OutputBox
            // 
            OutputBox.BorderStyle = BorderStyle.None;
            OutputBox.Dock = DockStyle.Fill;
            OutputBox.FormattingEnabled = true;
            OutputBox.IntegralHeight = false;
            OutputBox.ItemHeight = 17;
            OutputBox.Location = new Point(0, 25);
            OutputBox.Name = "OutputBox";
            OutputBox.ScrollAlwaysVisible = true;
            OutputBox.Size = new Size(430, 428);
            OutputBox.TabIndex = 0;
            OutputBox.KeyDown += OutputBox_KeyDown;
            OutputBox.KeyUp += OutputBox_KeyUp;
            OutputBox.MouseDown += OutputBox_MouseDown;
            OutputBox.MouseUp += OutputBox_MouseUp;
            // 
            // MainMenuStrip
            // 
            MainMenuStrip.Items.AddRange(new ToolStripItem[] { MainMenuStrip_Options, MainMenuStrip_Clear });
            MainMenuStrip.Location = new Point(0, 0);
            MainMenuStrip.Name = "MainMenuStrip";
            MainMenuStrip.Size = new Size(430, 25);
            MainMenuStrip.TabIndex = 1;
            MainMenuStrip.Text = "menuStrip1";
            // 
            // MainMenuStrip_Options
            // 
            MainMenuStrip_Options.DropDownItems.AddRange(new ToolStripItem[] { MainMenuStrip_Options_MouseMoveCheck, MainMenuStrip_Options_MouseWheelCheck });
            MainMenuStrip_Options.Name = "MainMenuStrip_Options";
            MainMenuStrip_Options.Size = new Size(44, 21);
            MainMenuStrip_Options.Text = "选项";
            // 
            // MainMenuStrip_Options_MouseMoveCheck
            // 
            MainMenuStrip_Options_MouseMoveCheck.CheckOnClick = true;
            MainMenuStrip_Options_MouseMoveCheck.Name = "MainMenuStrip_Options_MouseMoveCheck";
            MainMenuStrip_Options_MouseMoveCheck.Size = new Size(180, 22);
            MainMenuStrip_Options_MouseMoveCheck.Text = "鼠标移动检测";
            MainMenuStrip_Options_MouseMoveCheck.CheckedChanged += MainMenuStrip_Options_MouseMoveCheck_CheckedChanged;
            // 
            // MainMenuStrip_Options_MouseWheelCheck
            // 
            MainMenuStrip_Options_MouseWheelCheck.CheckOnClick = true;
            MainMenuStrip_Options_MouseWheelCheck.Name = "MainMenuStrip_Options_MouseWheelCheck";
            MainMenuStrip_Options_MouseWheelCheck.Size = new Size(180, 22);
            MainMenuStrip_Options_MouseWheelCheck.Text = "鼠标滚轮检测";
            MainMenuStrip_Options_MouseWheelCheck.CheckedChanged += MainMenuStrip_Options_MouseWheelCheck_CheckedChanged;
            // 
            // MainMenuStrip_Clear
            // 
            MainMenuStrip_Clear.Alignment = ToolStripItemAlignment.Right;
            MainMenuStrip_Clear.Name = "MainMenuStrip_Clear";
            MainMenuStrip_Clear.Size = new Size(44, 21);
            MainMenuStrip_Clear.Text = "清空";
            MainMenuStrip_Clear.Click += MainMenuStrip_Clear_Click;
            // 
            // TestWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(430, 453);
            Controls.Add(OutputBox);
            Controls.Add(MainMenuStrip);
            MaximizeBox = false;
            Name = "TestWindow";
            ShowIcon = false;
            Text = "按键输入宏 - 测试窗口";
            KeyDown += TestWindows_KeyDown;
            KeyUp += TestWindows_KeyUp;
            MouseDown += TestWindows_MouseDown;
            MouseUp += TestWindows_MouseUp;
            MainMenuStrip.ResumeLayout(false);
            MainMenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox OutputBox;
        private MenuStrip MainMenuStrip;
        private ToolStripMenuItem MainMenuStrip_Options;
        private ToolStripMenuItem MainMenuStrip_Options_MouseMoveCheck;
        private ToolStripMenuItem MainMenuStrip_Clear;
        private ToolStripMenuItem MainMenuStrip_Options_MouseWheelCheck;
    }
}