using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyInputMacro
{
    public partial class TestWindow : Form
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        static string GetNowTime()
        {
            return $"[{DateTime.Now}] ";
        }
        void OutputBoxAutoScroll()
        {
            OutputBox.SelectedIndex = OutputBox.Items.Count - 1;
            //OutputBox.SelectedIndex = -1;
        }

        private void OutputBox_KeyDown(object sender, KeyEventArgs e)
        {
            OutputBox.Items.Add(GetNowTime() + "检测到按键\"" + e.KeyData.ToString() + "\"按下");
            OutputBoxAutoScroll();
            e.Handled = true;
        }

        private void OutputBox_KeyUp(object sender, KeyEventArgs e)
        {
            OutputBox.Items.Add(GetNowTime() + "检测到按键\"" + e.KeyData.ToString() + "\"弹起");
            OutputBoxAutoScroll();
            e.Handled = true;
        }

        private void OutputBox_MouseDown(object sender, MouseEventArgs e)
        {
            OutputBox.Items.Add($"{GetNowTime()}检测到鼠标\"{e.Button}\"按下，位置: [x: {e.X}, y: {e.Y}]");
            OutputBoxAutoScroll();
        }

        private void OutputBox_MouseUp(object sender, MouseEventArgs e)
        {
            OutputBox.Items.Add($"{GetNowTime()}检测到鼠标\"{e.Button}\"弹起，位置: [x: {e.X}, y: {e.Y}]");
            OutputBoxAutoScroll();
        }

        private void OutputBox_MouseMove(object sender, MouseEventArgs e)
        {
            OutputBox.Items.Add($"{GetNowTime()}检测到鼠标\"{e.Button}\"移动，位置: [x: {e.X}, y: {e.Y}]");
            OutputBoxAutoScroll();
        }
        private void OutputBox_MouseWheel(object sender, MouseEventArgs e)
        {
            OutputBox.Items.Add($"{GetNowTime()}检测到鼠标滚动，距离: {e.Delta}; 位置: [x: {e.X}, y: {e.Y}]");
            OutputBoxAutoScroll();
        }


        private void TestWindows_MouseDown(object sender, MouseEventArgs e)
        {
            OutputBox_MouseDown(sender, e);
        }

        private void TestWindows_MouseUp(object sender, MouseEventArgs e)
        {
            OutputBox_MouseUp(sender, e);
        }

        private void TestWindows_KeyDown(object sender, KeyEventArgs e)
        {
            OutputBox_KeyDown(sender, e);
        }

        private void TestWindows_KeyUp(object sender, KeyEventArgs e)
        {
            OutputBox_KeyUp(sender, e);
        }

        private void TestWindow_MouseMove(object sender, MouseEventArgs e)
        {
            OutputBox_MouseMove(sender, e);
        }
        private void TestWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            OutputBox_MouseWheel(sender, e);
        }

        private void MainMenuStrip_Options_MouseMoveCheck_CheckedChanged(object sender, EventArgs e)
        {
            switch (MainMenuStrip_Options_MouseMoveCheck.Checked)
            {
                case true:
                    this.MouseMove += TestWindow_MouseMove!;
                    OutputBox.MouseMove += OutputBox_MouseMove!;
                    break;
                case false:
                    this.MouseMove -= TestWindow_MouseMove!;
                    OutputBox.MouseMove -= OutputBox_MouseMove!;
                    break;
            }
        }
        private void MainMenuStrip_Options_MouseWheelCheck_CheckedChanged(object sender, EventArgs e)
        {
            switch (MainMenuStrip_Options_MouseWheelCheck.Checked)
            {
                case true:
                    this.MouseWheel += TestWindow_MouseWheel!;
                    OutputBox.MouseWheel += OutputBox_MouseWheel!;
                    break;
                case false:
                    this.MouseWheel -= TestWindow_MouseWheel!;
                    OutputBox.MouseWheel -= OutputBox_MouseWheel!;
                    break;
            }
        }

        private void MainMenuStrip_Clear_Click(object sender, EventArgs e)
        {
            OutputBox.Items.Clear();
        }


    }
}
