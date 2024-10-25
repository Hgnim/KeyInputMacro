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
            Logger.LogAdd("��������");

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
                                int interval = 100;//���º�̧��ļ��ʱ��
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
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")//�ж��Ƿ�Ϊ��갴�����������갴��������ִ��
                                {
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseDown, mainTarget);
                                    Thread.Sleep(interval);
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseUp, mainTarget);
                                }
                                Logger.LogAdd($"ִ�а�����ѹ������: {readKeyCode}; ���ʱ��: {interval}ms");
                            }
                            break;
                        case "down":
                            {
                                Keys readKeyCode = (Keys)Enum.Parse(typeof(Keys), xmlE.GetAttribute("key").ToLower(), true);
                                if (xmlE.GetAttribute("type").ToLower() == "key" || xmlE.GetAttribute("type") == "") //(readKeyCode.ToString().ToLower().IndexOf("button") == -1)
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyDown, mainTarget);
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseDown, mainTarget);
                                Logger.LogAdd($"ִ�а������¡�����: {readKeyCode}");
                            }
                            break;
                        case "up":
                            {
                                Keys readKeyCode = (Keys)Enum.Parse(typeof(Keys), xmlE.GetAttribute("key").ToLower(), true);
                                if (xmlE.GetAttribute("type").ToLower() == "key" || xmlE.GetAttribute("type") == "")
                                    KeyInput.SendAction(readKeyCode, KeyInput.KeyAction.KeyUp, mainTarget);
                                else if (xmlE.GetAttribute("type").ToLower() == "mouse")
                                    KeyInput.SendAction((KeyInput.MouseButton)readKeyCode, KeyInput.MouseAction.MouseUp, mainTarget);
                                Logger.LogAdd($"ִ�а���̧�𡣰���: {readKeyCode}");
                            }
                            break;
                        case "text":
                            {
                                KeyInput.SendAction(xmlE.GetAttribute("txt"), mainTarget);
                                Logger.LogAdd($"ִ�������ı����ı�����: {xmlE.GetAttribute("txt")}");
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
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/Hgnim/KeyInputMacro/wiki/KeyInputMacro-Help");
        }
    }
}
