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
            OutputBox.Items.Add(GetNowTime() + "检测到鼠标\"" + e.Button.ToString() + "\"按下");
            OutputBoxAutoScroll();
        }

        private void OutputBox_MouseUp(object sender, MouseEventArgs e)
        {
            OutputBox.Items.Add(GetNowTime() + "检测到鼠标\"" + e.Button.ToString() + "\"弹起");
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
    }
}
