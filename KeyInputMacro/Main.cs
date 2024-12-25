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


        /// <summary>
        /// ��д
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312://WM_HOTKEY
                    switch (m.WParam.ToInt32())
                    {
                        case 0x001:
                            if (Main_ToolStrip_Script_Run.Enabled)
                                Main_ToolStrip_Script_Run_Click(null!, null!);
                            else if (Main_ToolStrip_Script_Stop.Enabled)
                                Main_ToolStrip_Script_Stop_Click(null!, null!);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Logger.LogAdd("��������");

            if (Program.programArgs!.Length > 0)
                if (File.Exists(Program.programArgs[0]))
                    OpenFileToScriptEditBox(Program.programArgs[0]);

            if (!HotKey.RegisterHotKey(Handle, 0x001, HotKey.FsModifiers.MOD_CONTROL, Keys.OemPipe))
                MessageBox.Show("����: ��ݼ����ڳ�ͻ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            HotKey.UnregisterHotKey(Handle, 0x001);
        }
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                Main_ToolStrip_Script_Open_Click(null!, null!);
            }
            else if(e.Control && e.KeyCode == Keys.S)
            {
                Main_ToolStrip_Script_SaveAs_Click(null!, null!);
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                Main_ToolStrip_Script_New_Click(null!, null!);
            }
            else if(e.Control && e.KeyCode == Keys.F)
            {
                Main_ToolStrip_Edit_Format_Click(null!, null!);
            }
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
        /// ���Խű��༭������ݽ��в���ʱ�����༭�����Ƿ�������ݣ���������Ӧ������
        /// <br/>����༭���ڰ������ݣ���ѯ���û��Ƿ�������û�����ǣ��򷵻�true��ʾͨ��������Ϊfalse
        /// <br/>����༭����û���κ����ݣ��򷵻�true��ʾͨ����
        /// </summary>
        /// <returns>����true��ʾͨ��������false��ʾ��ͨ��</returns>
        bool EditBoxTextCheck()
        {
            if (scriptEditBox.Text != "")
            {
                DialogResult input = MessageBox.Show("��ǰ�༭���ڰ������ݣ��Ƿ������", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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
        /// ��Ŀ���ļ����ı��༭����
        /// </summary>
        /// <param name="filePath">Ŀ���ļ�λ��</param>
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
                MessageBox.Show("�ļ���ʧ�ܣ�", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("�ļ�����ʧ�ܣ�", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Main_ToolStrip_Script_Run_Click(object sender, EventArgs e)
        {
            Main_ToolStrip_Script_Run.Enabled = false;
            ScriptThreadStop = false;//���ýű��߳���ֹװ�õ�״̬
            Main_ToolStrip_Script_Stop.Enabled = true;
            scriptEditBox.Enabled = false;

            string script = scriptEditBox.Text;
            Logger.LogAdd("����׼�����нű�...");
            Thread thread = new(() =>
            {
                void ErrorMessage(string cause = "������")
                {
                    string errstr = $"ִ�нű�ʱ��������ԭ��: {cause}";
                    Logger.LogAdd(errstr);
                    MessageBox.Show(errstr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.LogAdd("�ű�ִ���߳������������ֹ");
                    this.Invoke(new Action(() =>
                    {
                        Main_ToolStrip_Script_Stop_Click(null!, null!);
                    }));
                }
                try
                {
                    while (script.IndexOf("<!--") != -1) //�Խű����д���
                    {
                        int[] index = new int[2];
                        index[0] = script.IndexOf("<!--");//ȥ������ע��
                        index[1] = script.IndexOf("-->", index[0]);
                        script = script.Remove(index[0], index[1] + 3 - index[0]);
                    }
                    IntPtr parentWindow = 0, subWindow = 0;//�����ھ�����Ӵ���/�ؼ����
                    IntPtr mainTarget;//��ҪĿ����

                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(script);
                    script = null!;//�ͷŸñ���
                    XmlNode xmlRoot = xmlDoc.SelectSingleNode("KeyInputMacro")!;
                    XmlNodeList xmlNL = xmlRoot.ChildNodes;
                    XmlElement xmlE;
                    foreach (XmlNode xn in xmlNL)
                    {
                        xmlE = (XmlElement)xn;
                        if (xmlE.Name == "TargetObject")
                        {
                            parentWindow = GetWindows.FindWindow(xmlE.GetAttribute("windowClass"), xmlE.GetAttribute("windowName"));//��ȡxml�ļ��е�Ŀ����Ϣ
                            subWindow = GetWindows.FindWindowEx(parentWindow, 0, xmlE.GetAttribute("subClass"), xmlE.GetAttribute("subName"));
                            Logger.LogAdd($"�Ѷ�ȡ�ű��е�Ŀ����Ϣ: ({xmlE.GetAttribute("windowName")}; {xmlE.GetAttribute("windowClass")}; {xmlE.GetAttribute("subName")}; {xmlE.GetAttribute("subClass")})");
                        }
                    }
                    if (subWindow != 0)//�ж��Ӷ����Ƿ�Ϊ�գ���Ϊ����ֵ����Ҫ����
                        mainTarget = subWindow;
                    else if (parentWindow != 0)//ͬ��
                        mainTarget = parentWindow;
                    else//����������㣬�����
                    { ErrorMessage("δ�ҵ���δָ��ִ��Ŀ�ꡣ"); goto over; }


                    xmlRoot = xmlDoc.SelectSingleNode("KeyInputMacro")!.SelectSingleNode("Script")!;
                    xmlNL = xmlRoot.ChildNodes;
                    ScriptRunFramework srf = new()
                    {
                        mainTarget = mainTarget
                    };
                    Logger.LogAdd("׼����ϣ���ʼִ�нű�");
                    srf.Run(xmlNL);//��ʼִ�нű�����

                    Logger.LogAdd("�ű�ִ���߳���ֹ");
                    this.Invoke(new Action(() =>
                    {
                        Main_ToolStrip_Script_Stop_Click(null!, null!);//ִ����Ϻ���ֹ
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
            ScriptThreadStop = true;//��ֹ�ű�ִ���߳�
            Main_ToolStrip_Script_Run.Enabled = true;
            scriptEditBox.Enabled = true;

            Logger.LogAdd("ֹͣ�ű�ִ��");
        }
        /// <summary>
        /// ������ֹ�ű������̣߳�����Ϊtrue���߳̽�������ֹ
        /// </summary>
        static bool ScriptThreadStop = false;
        struct ScriptRunFramework
        {
            /// <summary>
            /// ��ҪĿ����
            /// </summary>
            public IntPtr mainTarget;
            /// <summary>
            /// ִ�а�������
            /// </summary>
            /// <param name="actionType">������������</param>
            /// <param name="keyStr">����ID</param>
            /// <param name="keyType">��������</param>
            /// <param name="intervalStr">press���͵ĵȴ�ʱ��</param>
            /// <param name="mousePos">�����в�����λ������</param>
            private readonly void KeyActionRun(string actionType, string keyStr, string keyType, string intervalStr = "", string mousePos = "")
            {
                Keys keyCode = (Keys)Enum.Parse(typeof(Keys), keyStr.ToLower(), true);
                int interval = 100;//���º�̧��ļ��ʱ��
                {
                    string GetIntervalTime = intervalStr;
                    if (GetIntervalTime != "")
                        interval = int.Parse(GetIntervalTime);
                }

                if (keyType.ToLower() == "key" || keyType == "") //(keyCode.ToString().ToLower().IndexOf("button") == -1)
                {
                    if (actionType == "up") goto keyUp;
                    KeyInput.SendAction(keyCode, KeyInput.KeyAction.KeyDown, mainTarget);
                    if (actionType == "down") goto keyActionOver;
                    Thread.Sleep(interval);
keyUp:;
                    KeyInput.SendAction(keyCode, KeyInput.KeyAction.KeyUp, mainTarget);
keyActionOver:;
                }
                else if (keyType.ToLower() == "mouse")//�ж��Ƿ�Ϊ��갴�����������갴��������ִ��
                {
                    Point mousePosPoint;
                    if (mousePos == "") mousePosPoint = new(0, 0);
                    else mousePosPoint = new(int.Parse(mousePos.Split(',')[0]), int.Parse(mousePos.Split(',')[1]));

                    if (actionType == "up") goto mouseUp;
                    KeyInput.SendAction((KeyInput.MouseButton)keyCode, KeyInput.MouseAction.MouseDown, mainTarget, mousePosPoint);
                    if (actionType == "down") goto mouseActionOver;
                    Thread.Sleep(interval);
mouseUp:;
                    KeyInput.SendAction((KeyInput.MouseButton)keyCode, KeyInput.MouseAction.MouseUp, mainTarget, mousePosPoint);
mouseActionOver:;
                }

                Thread logT = new(() =>
                {
                    string logStr = "";
                    logStr += "ִ�а���";
                    switch (actionType)
                    {
                        case "press":
                            logStr += "��ѹ"; break;
                        case "down":
                            logStr += "����"; break;
                        case "up":
                            logStr += "̧��"; break;
                    }
                    logStr += $"������: {keyCode}";
                    if (actionType == "press")
                        logStr += $"; ���ʱ��: {interval}ms";
                    if (keyType == "mouse" && mousePos != "")
                        logStr += $"; λ��: [x: {mousePos.Split(',')[0]}; y: {int.Parse(mousePos.Split(',')[1])}]";
                    Logger.LogAdd(logStr);
                }); logT.Start();
            }
            public void Run(XmlNodeList xmlNL)
            {
                foreach (XmlNode xn in xmlNL)
                {
                    XmlElement xmlE;
                    xmlE = (XmlElement)xn;
                    switch (xmlE.Name)
                    {
                        case "press":
                            KeyActionRun(xmlE.Name, xmlE.GetAttribute("key"), xmlE.GetAttribute("type"), xmlE.GetAttribute("ms"), xmlE.GetAttribute("mouse_pos"));
                            break;
                        case "down":
                        case "up":
                            KeyActionRun(xmlE.Name, xmlE.GetAttribute("key"), xmlE.GetAttribute("type"), mousePos: xmlE.GetAttribute("mouse_pos"));
                            break;
                        case "mouse_move":
                            {
                                string posStr = xmlE.GetAttribute("pos");
                                /*string keyStr = xmlE.GetAttribute("key").ToLower();*/
                                Point pos = new(int.Parse(posStr.Split(',')[0]), int.Parse(posStr.Split(',')[1]));
                                KeyInput.MouseButton inputMouseButton = KeyInput.MouseButton.none;
                                /*if (keyStr != "") 
                                    inputMouseButton = (KeyInput.MouseButton)Enum.Parse(typeof(Keys),keyStr, true); */

                                KeyInput.SendAction(inputMouseButton, KeyInput.MouseAction.MouseMove, mainTarget, pos);
                                Logger.LogAdd($"ִ������ƶ���λ��: [x: {pos.X}; y: {pos.Y}]");
                            }
                            break;
                        case "mouse_wheel":
                            {
                                string wheelValue = xmlE.GetAttribute("wheel");
                                string posStr = xmlE.GetAttribute("pos");
                                Point pos;
                                if (posStr == "")
                                    pos = new(0, 0);
                                else
                                    pos = new(int.Parse(posStr.Split(',')[0]), int.Parse(posStr.Split(',')[1]));
                                KeyInput.SendAction(KeyInput.MouseButton.none, KeyInput.MouseAction.MouseWheel, mainTarget, pos, int.Parse(wheelValue));
                                Logger.LogAdd($"ִ�������ֹ���������: {wheelValue}; λ��: [x: {pos.X}; y: {pos.Y}]");
                            }
                            break;
                        case "text":
                            {
                                string targetText = xmlE.GetAttribute("txt");
                                KeyInput.SendAction(targetText, mainTarget);
                                Logger.LogAdd($"ִ�������ı����ı�����: {targetText}");
                            }
                            break;
                        case "loop":
                            {
                                Logger.LogAdd($"����ѭ��ִ��");
                                ScriptRunFramework srf = new()
                                {
                                    mainTarget = mainTarget
                                };
                                while (!ScriptThreadStop)
                                    srf.Run(xn.ChildNodes);
                                Logger.LogAdd($"�˳�ѭ��ִ��");
                            }
                            break;
                        case "wait":
                            Logger.LogAdd($"ִ�еȴ����ȴ�ʱ��: {xmlE.GetAttribute("ms")}ms");
                            Thread.Sleep(int.Parse(xmlE.GetAttribute("ms")));
                            break;
                    }
                    if (ScriptThreadStop)
                        break;
                }
            }
            /*/// <summary>
            /// ����xml�е��ַ�����ȡkey����
            /// </summary>
            /// <param name="input">������ַ���</param>
            /// <returns></returns>
            Keys GetKey(string input)
            {
                switch (input)
                {

                }
            }*/

            /*/// <summary>
            /// �����б�ö��
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
            catch { MessageBox.Show("д��ʧ�ܣ�", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        /// <summary>
        /// ��ʽ��XmlDocument�����е�xml����
        /// </summary>
        /// <param name="xmldoc">Ŀ��XmlDocument����</param>
        /// <returns>���ظ�ʽ�����xml�ı�</returns>
        /// 
        static string FormatXML(XmlDocument xmldoc)
        {
            System.IO.StringWriter sw = new();
            System.Xml.XmlTextWriter xtw = new(sw)
            {
                Indentation = 4,//��������
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
            catch { MessageBox.Show("��ʽ��ʧ�ܣ�", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
$@"������: KeyInputMacro
����: ���������
�汾: V{Program.version} 
Copyright (C) 2024 Hgnim, All rights reserved.
Github: https://github.com/Hgnim/KeyInputMacro",
"����");
        }

        private void Main_ToolStrip_help_helpDoc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/Hgnim/KeyInputMacro/wiki/%E5%B8%AE%E5%8A%A9%E6%96%87%E6%A1%A3");
        }


    }
}
