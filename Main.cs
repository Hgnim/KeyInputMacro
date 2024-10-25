using System.Diagnostics;
using WindowsAPI;
using static WindowsAPI.GetWindows;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.Windows.Forms;
using System;


namespace KeyInputMacro
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Logger.LogAdd("程序启动");

            if (Program.programArgs!.Length > 0)
                if (File.Exists(Program.programArgs[0]))
                    OpenFileToScriptEditBox(Program.programArgs[0]);
        }

        IReadOnlyList<WindowInfo>? windowsList;
        IReadOnlyList<WindowInfo>? subWindowsList;
        private void FlushWindowsList_Click(object sender, EventArgs e)
        {
            parentObjList.Items.Clear();
            subObjList.Items.Clear();
            subWindowsList = null;

            windowsList = GetWindows.FindAllWindows();
            foreach (var wind in windowsList)
            {
                parentObjList.Items.Add($"{wind.Title}; {wind.ClassName}");
            }
        }

        private void ParentObjList_SelectedIndexChanged(object sender, EventArgs e)
        {
            subObjList.Items.Clear();

            subWindowsList = GetWindows.FindAllChildWindows(windowsList![parentObjList.SelectedIndex].Hwnd, x => true);
            foreach (var wind in subWindowsList)
            {
                subObjList.Items.Add($"{wind.Title}; {wind.ClassName}");
            }
        }

        private void Main_ToolStrip_OpenTestWindow_Click(object sender, EventArgs e)
        {
            TestWindow tw = new();
            tw.Show();
        }
        private void Main_ToolStrip_OpenLogWindow_Click(object sender, EventArgs e)
        {
            LogWindow lw = new();
            lw.Show();
        }
        /// <summary>
        /// 当对脚本编辑框的内容进行操作时，检查编辑框内是否包含内容，并做出相应反馈。
        /// <br/>如果编辑框内包含内容，则询问用户是否继续。用户点击是，则返回true表示通过，否则为false
        /// <br/>如果编辑框内没用任何内容，则返回true表示通过。
        /// </summary>
        /// <returns>返回true表示通过；返回false表示不通过</returns>
        bool EditBoxTextCheck()
        {
            if (scriptEditBox.Text != "")
            {
                DialogResult input = MessageBox.Show("当前编辑框内包含内容，是否继续？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (input == DialogResult.Yes)
                    return true;
                else
                    return false;
            }
            return true;
        }
        private void Main_ToolStrip_Script_New_Click(object sender, EventArgs e)
        {
            if (!EditBoxTextCheck())
                return;
            scriptEditBox.Text =
@"<KeyInputMacro>
    <TargetObject/>
    <Script>
    </Script>
</KeyInputMacro>"
;
        }
        private void Main_ToolStrip_Script_Open_Click(object sender, EventArgs e)
        {
            if (!EditBoxTextCheck())
                return;
            if (OpenScriptFile.ShowDialog() == DialogResult.OK)
            {
                OpenFileToScriptEditBox(OpenScriptFile.FileName);
            }
        }
        /// <summary>
        /// 打开目标文件至文本编辑框内
        /// </summary>
        /// <param name="filePath">目标文件位置</param>
        void OpenFileToScriptEditBox(string filePath)
        {
            try
            {
                StreamReader sr = new(filePath);
                scriptEditBox.Text = sr.ReadToEnd();
                sr.Close();
            }
            catch
            {
                MessageBox.Show("文件打开失败！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Main_ToolStrip_Script_SaveAs_Click(object sender, EventArgs e)
        {
            if (SaveAsScriptFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(scriptEditBox.Text);
                    xmlDoc.Save(SaveAsScriptFile.FileName);
                }
                catch
                {
                    MessageBox.Show("文件保存失败！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Main_ToolStrip_Script_Run_Click(object sender, EventArgs e)
        {
            Main_ToolStrip_Script_Run.Enabled = false;
            ScriptThreadStop = false;//重置脚本线程终止装置的状态
            Main_ToolStrip_Script_Stop.Enabled = true;
            scriptEditBox.Enabled = false;

            string script = scriptEditBox.Text;
            Logger.LogAdd("正在准备运行脚本...");
            Thread thread = new(() =>
            {
                void ErrorMessage(string cause = "不明。")
                {
                    string errstr = $"执行脚本时发生错误！原因: {cause}";
                    Logger.LogAdd(errstr);
                    MessageBox.Show(errstr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.LogAdd("脚本执行线程因发生错误而终止");
                    this.Invoke(new Action(() =>
                    {
                        Main_ToolStrip_Script_Stop_Click(null!, null!);
                    }));
                }
                try
                {
                    while (script.IndexOf("<!--") != -1) //对脚本进行处理
                    {
                        int[] index = new int[2];
                        index[0] = script.IndexOf("<!--");//去除所有注释
                        index[1] = script.IndexOf("-->", index[0]);
                        script = script.Remove(index[0], index[1] + 3 - index[0]);
                    }
                    IntPtr parentWindow = 0, subWindow = 0;//父窗口句柄与子窗口/控件句柄
                    IntPtr mainTarget;//主要目标句柄

                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(script);
                    script = null!;//释放该变量
                    XmlNode xmlRoot = xmlDoc.SelectSingleNode("KeyInputMacro")!;
                    XmlNodeList xmlNL = xmlRoot.ChildNodes;
                    XmlElement xmlE;
                    foreach (XmlNode xn in xmlNL)
                    {
                        xmlE = (XmlElement)xn;
                        if (xmlE.Name == "TargetObject")
                        {
                            parentWindow = GetWindows.FindWindow(xmlE.GetAttribute("windowClass"), xmlE.GetAttribute("windowName"));//读取xml文件中的目标信息
                            subWindow = GetWindows.FindWindowEx(parentWindow, 0, xmlE.GetAttribute("subClass"), xmlE.GetAttribute("subName"));
                            Logger.LogAdd($"已读取脚本中的目标信息: ({xmlE.GetAttribute("windowName")}; {xmlE.GetAttribute("windowClass")}; {xmlE.GetAttribute("subName")}; {xmlE.GetAttribute("subClass")})");
                        }
                    }
                    if (subWindow != 0)//判断子对象是否为空，不为空则赋值至主要变量
                        mainTarget = subWindow;
                    else if (parentWindow != 0)//同理
                        mainTarget = parentWindow;
                    else//如果都不满足，则错误
                    { ErrorMessage("未找到或未指定执行目标。"); goto over; }


                    xmlRoot = xmlDoc.SelectSingleNode("KeyInputMacro")!.SelectSingleNode("Script")!;
                    xmlNL = xmlRoot.ChildNodes;
                    ScriptRunFramework srf = new()
                    {
                        mainTarget = mainTarget
                    };
                    Logger.LogAdd("准备完毕，开始执行脚本");
                    srf.Run(xmlNL);//开始执行脚本内容

                    Logger.LogAdd("脚本执行线程终止");
                    this.Invoke(new Action(() =>
                    {
                        Main_ToolStrip_Script_Stop_Click(null!, null!);//执行完毕后终止
                    }));
                }
                catch
#if DEBUG
                (Exception ex)
#endif
                {
#if DEBUG
                    Debug.WriteLine(ex.ToString());
#endif
                    ErrorMessage(); goto over;
                }
over:;
            }); thread.Start();
        }
        private void Main_ToolStrip_Script_Stop_Click(object sender, EventArgs e)
        {
            Main_ToolStrip_Script_Stop.Enabled = false;
            ScriptThreadStop = true;//终止脚本执行线程
            Main_ToolStrip_Script_Run.Enabled = true;
            scriptEditBox.Enabled = true;

            Logger.LogAdd("停止脚本执行");
        }
        /// <summary>
        /// 用于终止脚本运行线程，设置为true后线程将控制终止
        /// </summary>
        static bool ScriptThreadStop = false;
        struct ScriptRunFramework
        {
            /// <summary>
            /// 主要目标句柄
            /// </summary>
            public IntPtr mainTarget;
            public void Run(XmlNodeList xmlNL)
            {
                foreach (XmlNode xn in xmlNL)
                {
                    XmlElement xmlE;
                    xmlE = (XmlElement)xn;
                    switch (xmlE.Name)
                    {
                        case "press":
                            {
                                int interval = 100;//按下和抬起的间隔时间
                                {
                                    string GetIntervalTime = xmlE.GetAttribute("ms");
                                    if (GetIntervalTime != "")
                                        interval = int.Parse(GetIntervalTime);
                                }
                                Keys readKeyCode = (Keys)Enum.Parse(typeof(Keys), xmlE.GetAttribute("key").ToLower(), true);
                                if (xmlE.GetAttribute("type").ToLower() == "key" || xmlE.GetAttribute("type") == "") //(readKeyCode.ToString().ToLower().IndexOf("button") == -1)
                                {
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyDown, mainTarget);
                                    Thread.Sleep(interval);
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyUp, mainTarget);
                                }
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")//判断是否为鼠标按键，如果是鼠标按键则另外执行
                                {
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseDown, mainTarget);
                                    Thread.Sleep(interval);
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseUp, mainTarget);
                                }
                                Logger.LogAdd($"执行按键按压。按键: {readKeyCode}; 间隔时间: {interval}ms");
                            }
                            break;
                        case "down":
                            {
                                Keys readKeyCode = (Keys)Enum.Parse(typeof(Keys), xmlE.GetAttribute("key").ToLower(), true);
                                if (xmlE.GetAttribute("type").ToLower() == "key" || xmlE.GetAttribute("type") == "") //(readKeyCode.ToString().ToLower().IndexOf("button") == -1)
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyDown, mainTarget);
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseDown, mainTarget);
                                Logger.LogAdd($"执行按键按下。按键: {readKeyCode}");
                            }
                            break;
                        case "up":
                            {
                                Keys readKeyCode = (Keys)Enum.Parse(typeof(Keys), xmlE.GetAttribute("key").ToLower(), true);
                                if (xmlE.GetAttribute("type").ToLower() == "key" || xmlE.GetAttribute("type") == "")
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyUp, mainTarget);
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseUp, mainTarget);
                                Logger.LogAdd($"执行按键抬起。按键: {readKeyCode}");
                            }
                            break;
                        case "text":
                            {
                                KeyInput.SendAction(xmlE.GetAttribute("txt"), mainTarget);
                                Logger.LogAdd($"执行输入文本。文本内容: {xmlE.GetAttribute("txt")}");
                            }
                            break;
                        case "loop":
                            {
                                Logger.LogAdd($"进入循环执行");
                                ScriptRunFramework srf = new()
                                {
                                    mainTarget = mainTarget
                                };
                                while (!ScriptThreadStop)
                                    srf.Run(xn.ChildNodes);
                                Logger.LogAdd($"退出循环执行");
                            }
                            break;
                        case "wait":
                            Logger.LogAdd($"执行等待。等待时间: {xmlE.GetAttribute("ms")}ms");
                            Thread.Sleep(int.Parse(xmlE.GetAttribute("ms")));
                            break;
                    }
                    if (ScriptThreadStop)
                        break;
                }
            }
            /*/// <summary>
            /// 根据xml中的字符串获取key类型
            /// </summary>
            /// <param name="input">输入的字符串</param>
            /// <returns></returns>
            Keys GetKey(string input)
            {
                switch (input)
                {

                }
            }*/

            /*/// <summary>
            /// 按键列表枚举
            /// </summary>
            enum KeysList
            {
                a=Keys.A, b=Keys.B,c=Keys.C,d=Keys.D,e=Keys.E,f=Keys.F,g=Keys.G,h=Keys.H,i=Keys.I,j=Keys.J,k=Keys.K,l=Keys.L,m=Keys.M,n=Keys.N,o=Keys.O,p=Keys.P,q=Keys.Q,r=Keys.R,s=Keys.S,t=Keys.T,u=Keys.U,v=Keys.V,w=Keys.W,x=Keys.X,y=Keys.Y,z=Keys.Z,
                lwin=Keys.LWin, rwin=Keys.RWin,menu=Keys.Apps,
                numpad0=Keys.NumPad0,numpad1=Keys.NumPad1,numpad2=Keys.NumPad2,numpad3=Keys.NumPad3,numpad4=Keys.NumPad4,numpad5=Keys.NumPad5,numpad6=Keys.NumPad6,numpad7=Keys.NumPad7,numpad8=Keys.NumPad8,numpad9=Keys.NumPad9,
            }*/
        }
        private void WriteObjToScriptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] writeText = new string[4];
                if (windowsList != null && windowsList.Count > 0 && parentObjList.SelectedIndex != -1)
                {
                    writeText[0] = windowsList[parentObjList.SelectedIndex].Title;
                    writeText[1] = windowsList[parentObjList.SelectedIndex].ClassName;
                }
                if (subWindowsList != null && subWindowsList.Count > 0 && subObjList.SelectedIndex != -1)
                {
                    writeText[2] = subWindowsList[subObjList.SelectedIndex].Title;
                    writeText[3] = subWindowsList[subObjList.SelectedIndex].ClassName;
                }

                XmlDocument xmlDoc = new();
                XmlNodeList xmlNL;
                XmlElement xmlEle;
                xmlDoc.LoadXml(scriptEditBox.Text);
                xmlNL = xmlDoc.SelectSingleNode("KeyInputMacro")!.ChildNodes;
                foreach (XmlNode xn in xmlNL)
                {
                    xmlEle = (XmlElement)xn;
                    if (xmlEle.Name == "TargetObject")
                    {
                        xmlEle.SetAttribute("windowName", writeText[0]);
                        xmlEle.SetAttribute("windowClass", writeText[1]);
                        xmlEle.SetAttribute("subName", writeText[2]);
                        xmlEle.SetAttribute("subClass", writeText[3]);
                        break;
                    }
                }
                scriptEditBox.Text = FormatXML(xmlDoc);
            }
            catch { MessageBox.Show("写入失败！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        /// <summary>
        /// 格式化XmlDocument类型中的xml内容
        /// </summary>
        /// <param name="xmldoc">目标XmlDocument类型</param>
        /// <returns>返回格式化后的xml文本</returns>
        /// 
        static string FormatXML(XmlDocument xmldoc)
        {
            System.IO.StringWriter sw = new();
            System.Xml.XmlTextWriter xtw = new(sw)
            {
                Indentation = 4,//缩进长度
                Formatting = System.Xml.Formatting.Indented
            };
            xmldoc.WriteContentTo(xtw);
            xtw.Close();
            return sw.ToString();
        }

        private void Main_ToolStrip_Edit_Format_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(scriptEditBox.Text);
                scriptEditBox.Text = FormatXML(xmlDoc);
            }
            catch { MessageBox.Show("格式化失败！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ScriptEditBox_DragDrop(object sender, DragEventArgs e)
        {
            Array inputData = (Array)e.Data!.GetData(DataFormats.FileDrop)!;
            string[] filePathData = new string[inputData.Length];
            for (int i = 0; i < inputData.Length; i++)
            {
                filePathData[i] = inputData.GetValue(i)!.ToString()!;
            }

            OpenFileToScriptEditBox(filePathData[0]);
        }

        private void ScriptEditBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Main_ToolStrip_help_about_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
$@"程序名: KeyInputMacro
别名: 按键输入宏
版本: V{Program.version} 
Copyright (C) 2024 Hgnim, All rights reserved.
Github: https://github.com/Hgnim/KeyInputMacro",
"关于");
        }

        private void Main_ToolStrip_help_helpDoc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/Hgnim/KeyInputMacro/wiki/%E5%B8%AE%E5%8A%A9%E6%96%87%E6%A1%A3");
        }
    }
}
