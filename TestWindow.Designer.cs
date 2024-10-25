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
            SuspendLayout();
            // 
            // OutputBox
            // 
            OutputBox.BorderStyle = BorderStyle.None;
            OutputBox.Dock = DockStyle.Fill;
            OutputBox.FormattingEnabled = true;
            OutputBox.IntegralHeight = false;
            OutputBox.ItemHeight = 17;
            OutputBox.Location = new Point(0, 0);
            OutputBox.Name = "OutputBox";
            OutputBox.ScrollAlwaysVisible = true;
            OutputBox.Size = new Size(317, 441);
            OutputBox.TabIndex = 0;
            OutputBox.KeyDown += OutputBox_KeyDown;
            OutputBox.KeyUp += OutputBox_KeyUp;
            OutputBox.MouseDown += OutputBox_MouseDown;
            OutputBox.MouseUp += OutputBox_MouseUp;
            // 
            // TestWindows
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(317, 441);
            Controls.Add(OutputBox);
            MaximizeBox = false;
            Name = "TestWindows";
            ShowIcon = false;
            Text = "按键输入宏 - 测试窗口";
            KeyDown += TestWindows_KeyDown;
            KeyUp += TestWindows_KeyUp;
            MouseDown += TestWindows_MouseDown;
            MouseUp += TestWindows_MouseUp;
            ResumeLayout(false);
        }

        #endregion

        private ListBox OutputBox;
    }
}