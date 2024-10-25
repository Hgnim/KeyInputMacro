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
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void LogWindow_Load(object sender, EventArgs e)
        {
            foreach (string log in Logger.logs)
            {
                LogList.Items.Add(log);
            }
            Logger.LogUpdateToUI += LogUpdateToUI;
            LogListAutoScroll();
        }
        void LogUpdateToUI(string newLog)
        {
            this.Invoke(new Action(() =>
            {
                LogList.Items.Add(newLog);
                LogListAutoScroll();
            }));
        }
        void LogListAutoScroll()
        {
            LogList.SelectedIndex = LogList.Items.Count - 1;
        }

        private void LogWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.LogUpdateToUI -= LogUpdateToUI;
        }
    }
    public struct Logger
    {
        public  delegate void AddNewLog(string newLog);
        /// <summary>
        /// 将更新的日志数据同步至UI
        /// </summary>
        public static event AddNewLog? LogUpdateToUI;
#pragma warning disable CA2211
        public static List<string> logs = [];
#pragma warning restore CA2211
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void LogAdd(string message)
        {
            logs.Add(GetTime() + message);            
                LogUpdateToUI?.Invoke(logs[^1]);//如果委托不为null，则调用。参数为数组的最后一位            
        }
        static string GetTime()
        {
            return DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff] ");
        }
    }
}
