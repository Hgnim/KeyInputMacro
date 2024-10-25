
namespace KeyInputMacro
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            GetWindows_GroupBox = new GroupBox();
            WriteObjToScriptButton = new Button();
            flushWindowsList = new Button();
            ParentObjListAndSubObjList_SplitContainer = new SplitContainer();
            parentObjList = new ListBox();
            subObjList = new ListBox();
            scriptEditBox = new TextBox();
            Main_ToolStrip = new MenuStrip();
            Main_ToolStrip_Script = new ToolStripMenuItem();
            Main_ToolStrip_Script_New = new ToolStripMenuItem();
            Main_ToolStrip_Script_Open = new ToolStripMenuItem();
            Main_ToolStrip_Script_SaveAs = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            Main_ToolStrip_Script_Run = new ToolStripMenuItem();
            Main_ToolStrip_Script_Stop = new ToolStripMenuItem();
            Main_ToolStrip_OpenTestWindow = new ToolStripMenuItem();
            Main_ToolStrip_Edit = new ToolStripMenuItem();
            Main_ToolStrip_Edit_Format = new ToolStripMenuItem();
            Main_ToolStrip_OpenLogWindow = new ToolStripMenuItem();
            Main_ToolStrip_help = new ToolStripMenuItem();
            Main_ToolStrip_help_helpDoc = new ToolStripMenuItem();
            Main_ToolStrip_help_about = new ToolStripMenuItem();
            Root_SplitContainer = new SplitContainer();
            OpenScriptFile = new OpenFileDialog();
            SaveAsScriptFile = new SaveFileDialog();
            GetWindows_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ParentObjListAndSubObjList_SplitContainer).BeginInit();
            ParentObjListAndSubObjList_SplitContainer.Panel1.SuspendLayout();
            ParentObjListAndSubObjList_SplitContainer.Panel2.SuspendLayout();
            ParentObjListAndSubObjList_SplitContainer.SuspendLayout();
            Main_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Root_SplitContainer).BeginInit();
            Root_SplitContainer.Panel1.SuspendLayout();
            Root_SplitContainer.Panel2.SuspendLayout();
            Root_SplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // GetWindows_GroupBox
            // 
            GetWindows_GroupBox.Controls.Add(WriteObjToScriptButton);
            GetWindows_GroupBox.Controls.Add(flushWindowsList);
            GetWindows_GroupBox.Controls.Add(ParentObjListAndSubObjList_SplitContainer);
            GetWindows_GroupBox.Dock = DockStyle.Fill;
            GetWindows_GroupBox.Location = new Point(0, 0);
            GetWindows_GroupBox.Name = "GetWindows_GroupBox";
            GetWindows_GroupBox.Size = new Size(612, 204);
            GetWindows_GroupBox.TabIndex = 2;
            GetWindows_GroupBox.TabStop = false;
            GetWindows_GroupBox.Text = "获取目标窗口";
            // 
            // WriteObjToScriptButton
            // 
            WriteObjToScriptButton.Anchor = AnchorStyles.Bottom;
            WriteObjToScriptButton.Location = new Point(288, 175);
            WriteObjToScriptButton.Name = "WriteObjToScriptButton";
            WriteObjToScriptButton.Size = new Size(138, 23);
            WriteObjToScriptButton.TabIndex = 4;
            WriteObjToScriptButton.Text = "将选择的目标写入脚本";
            WriteObjToScriptButton.UseVisualStyleBackColor = true;
            WriteObjToScriptButton.Click += WriteObjToScriptButton_Click;
            // 
            // flushWindowsList
            // 
            flushWindowsList.Anchor = AnchorStyles.Bottom;
            flushWindowsList.Location = new Point(187, 175);
            flushWindowsList.Name = "flushWindowsList";
            flushWindowsList.Size = new Size(95, 23);
            flushWindowsList.TabIndex = 3;
            flushWindowsList.Text = "刷新窗口列表";
            flushWindowsList.UseVisualStyleBackColor = true;
            flushWindowsList.Click += FlushWindowsList_Click;
            // 
            // ParentObjListAndSubObjList_SplitContainer
            // 
            ParentObjListAndSubObjList_SplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ParentObjListAndSubObjList_SplitContainer.Location = new Point(8, 22);
            ParentObjListAndSubObjList_SplitContainer.Name = "ParentObjListAndSubObjList_SplitContainer";
            // 
            // ParentObjListAndSubObjList_SplitContainer.Panel1
            // 
            ParentObjListAndSubObjList_SplitContainer.Panel1.Controls.Add(parentObjList);
            // 
            // ParentObjListAndSubObjList_SplitContainer.Panel2
            // 
            ParentObjListAndSubObjList_SplitContainer.Panel2.Controls.Add(subObjList);
            ParentObjListAndSubObjList_SplitContainer.Size = new Size(598, 147);
            ParentObjListAndSubObjList_SplitContainer.SplitterDistance = 288;
            ParentObjListAndSubObjList_SplitContainer.TabIndex = 2;
            // 
            // parentObjList
            // 
            parentObjList.Dock = DockStyle.Fill;
            parentObjList.FormattingEnabled = true;
            parentObjList.HorizontalScrollbar = true;
            parentObjList.IntegralHeight = false;
            parentObjList.ItemHeight = 17;
            parentObjList.Location = new Point(0, 0);
            parentObjList.Name = "parentObjList";
            parentObjList.Size = new Size(288, 147);
            parentObjList.TabIndex = 0;
            parentObjList.SelectedIndexChanged += ParentObjList_SelectedIndexChanged;
            // 
            // subObjList
            // 
            subObjList.Dock = DockStyle.Fill;
            subObjList.FormattingEnabled = true;
            subObjList.HorizontalScrollbar = true;
            subObjList.IntegralHeight = false;
            subObjList.ItemHeight = 17;
            subObjList.Location = new Point(0, 0);
            subObjList.Name = "subObjList";
            subObjList.Size = new Size(306, 147);
            subObjList.TabIndex = 1;
            // 
            // scriptEditBox
            // 
            scriptEditBox.AcceptsReturn = true;
            scriptEditBox.AcceptsTab = true;
            scriptEditBox.AllowDrop = true;
            scriptEditBox.Dock = DockStyle.Fill;
            scriptEditBox.Location = new Point(0, 0);
            scriptEditBox.Multiline = true;
            scriptEditBox.Name = "scriptEditBox";
            scriptEditBox.Size = new Size(612, 270);
            scriptEditBox.TabIndex = 3;
            scriptEditBox.DragDrop += ScriptEditBox_DragDrop;
            scriptEditBox.DragEnter += ScriptEditBox_DragEnter;
            // 
            // Main_ToolStrip
            // 
            Main_ToolStrip.Items.AddRange(new ToolStripItem[] { Main_ToolStrip_Script, Main_ToolStrip_OpenTestWindow, Main_ToolStrip_Edit, Main_ToolStrip_OpenLogWindow, Main_ToolStrip_help });
            Main_ToolStrip.Location = new Point(0, 0);
            Main_ToolStrip.Name = "Main_ToolStrip";
            Main_ToolStrip.Size = new Size(612, 25);
            Main_ToolStrip.TabIndex = 5;
            Main_ToolStrip.Text = "menuStrip1";
            // 
            // Main_ToolStrip_Script
            // 
            Main_ToolStrip_Script.DropDownItems.AddRange(new ToolStripItem[] { Main_ToolStrip_Script_New, Main_ToolStrip_Script_Open, Main_ToolStrip_Script_SaveAs, toolStripMenuItem1, Main_ToolStrip_Script_Run, Main_ToolStrip_Script_Stop });
            Main_ToolStrip_Script.Name = "Main_ToolStrip_Script";
            Main_ToolStrip_Script.Size = new Size(44, 21);
            Main_ToolStrip_Script.Text = "脚本";
            // 
            // Main_ToolStrip_Script_New
            // 
            Main_ToolStrip_Script_New.Name = "Main_ToolStrip_Script_New";
            Main_ToolStrip_Script_New.Size = new Size(112, 22);
            Main_ToolStrip_Script_New.Text = "新建";
            Main_ToolStrip_Script_New.Click += Main_ToolStrip_Script_New_Click;
            // 
            // Main_ToolStrip_Script_Open
            // 
            Main_ToolStrip_Script_Open.Name = "Main_ToolStrip_Script_Open";
            Main_ToolStrip_Script_Open.Size = new Size(112, 22);
            Main_ToolStrip_Script_Open.Text = "打开";
            Main_ToolStrip_Script_Open.Click += Main_ToolStrip_Script_Open_Click;
            // 
            // Main_ToolStrip_Script_SaveAs
            // 
            Main_ToolStrip_Script_SaveAs.Name = "Main_ToolStrip_Script_SaveAs";
            Main_ToolStrip_Script_SaveAs.Size = new Size(112, 22);
            Main_ToolStrip_Script_SaveAs.Text = "另存为";
            Main_ToolStrip_Script_SaveAs.Click += Main_ToolStrip_Script_SaveAs_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(109, 6);
            // 
            // Main_ToolStrip_Script_Run
            // 
            Main_ToolStrip_Script_Run.Name = "Main_ToolStrip_Script_Run";
            Main_ToolStrip_Script_Run.Size = new Size(112, 22);
            Main_ToolStrip_Script_Run.Text = "运行";
            Main_ToolStrip_Script_Run.Click += Main_ToolStrip_Script_Run_Click;
            // 
            // Main_ToolStrip_Script_Stop
            // 
            Main_ToolStrip_Script_Stop.Enabled = false;
            Main_ToolStrip_Script_Stop.Name = "Main_ToolStrip_Script_Stop";
            Main_ToolStrip_Script_Stop.Size = new Size(112, 22);
            Main_ToolStrip_Script_Stop.Text = "停止";
            Main_ToolStrip_Script_Stop.Click += Main_ToolStrip_Script_Stop_Click;
            // 
            // Main_ToolStrip_OpenTestWindow
            // 
            Main_ToolStrip_OpenTestWindow.Alignment = ToolStripItemAlignment.Right;
            Main_ToolStrip_OpenTestWindow.Name = "Main_ToolStrip_OpenTestWindow";
            Main_ToolStrip_OpenTestWindow.Size = new Size(68, 21);
            Main_ToolStrip_OpenTestWindow.Text = "测试窗口";
            Main_ToolStrip_OpenTestWindow.Click += Main_ToolStrip_OpenTestWindow_Click;
            // 
            // Main_ToolStrip_Edit
            // 
            Main_ToolStrip_Edit.DropDownItems.AddRange(new ToolStripItem[] { Main_ToolStrip_Edit_Format });
            Main_ToolStrip_Edit.Name = "Main_ToolStrip_Edit";
            Main_ToolStrip_Edit.Size = new Size(44, 21);
            Main_ToolStrip_Edit.Text = "编辑";
            // 
            // Main_ToolStrip_Edit_Format
            // 
            Main_ToolStrip_Edit_Format.Name = "Main_ToolStrip_Edit_Format";
            Main_ToolStrip_Edit_Format.Size = new Size(136, 22);
            Main_ToolStrip_Edit_Format.Text = "格式化文本";
            Main_ToolStrip_Edit_Format.Click += Main_ToolStrip_Edit_Format_Click;
            // 
            // Main_ToolStrip_OpenLogWindow
            // 
            Main_ToolStrip_OpenLogWindow.Alignment = ToolStripItemAlignment.Right;
            Main_ToolStrip_OpenLogWindow.Name = "Main_ToolStrip_OpenLogWindow";
            Main_ToolStrip_OpenLogWindow.Size = new Size(68, 21);
            Main_ToolStrip_OpenLogWindow.Text = "执行日志";
            Main_ToolStrip_OpenLogWindow.Click += Main_ToolStrip_OpenLogWindow_Click;
            // 
            // Main_ToolStrip_help
            // 
            Main_ToolStrip_help.DropDownItems.AddRange(new ToolStripItem[] { Main_ToolStrip_help_helpDoc, Main_ToolStrip_help_about });
            Main_ToolStrip_help.Name = "Main_ToolStrip_help";
            Main_ToolStrip_help.Size = new Size(44, 21);
            Main_ToolStrip_help.Text = "帮助";
            // 
            // Main_ToolStrip_help_helpDoc
            // 
            Main_ToolStrip_help_helpDoc.Name = "Main_ToolStrip_help_helpDoc";
            Main_ToolStrip_help_helpDoc.Size = new Size(180, 22);
            Main_ToolStrip_help_helpDoc.Text = "帮助文档";
            Main_ToolStrip_help_helpDoc.Click += Main_ToolStrip_help_helpDoc_Click;
            // 
            // Main_ToolStrip_help_about
            // 
            Main_ToolStrip_help_about.Name = "Main_ToolStrip_help_about";
            Main_ToolStrip_help_about.Size = new Size(180, 22);
            Main_ToolStrip_help_about.Text = "关于";
            Main_ToolStrip_help_about.Click += Main_ToolStrip_help_about_Click;
            // 
            // Root_SplitContainer
            // 
            Root_SplitContainer.Dock = DockStyle.Fill;
            Root_SplitContainer.Location = new Point(0, 25);
            Root_SplitContainer.Name = "Root_SplitContainer";
            Root_SplitContainer.Orientation = Orientation.Horizontal;
            // 
            // Root_SplitContainer.Panel1
            // 
            Root_SplitContainer.Panel1.Controls.Add(GetWindows_GroupBox);
            // 
            // Root_SplitContainer.Panel2
            // 
            Root_SplitContainer.Panel2.Controls.Add(scriptEditBox);
            Root_SplitContainer.Size = new Size(612, 478);
            Root_SplitContainer.SplitterDistance = 204;
            Root_SplitContainer.TabIndex = 6;
            // 
            // OpenScriptFile
            // 
            OpenScriptFile.DefaultExt = "xml.kims";
            OpenScriptFile.Filter = "脚本文件|*.xml.kims;*.kims|xml文件|*.xml|文本文件|*.txt";
            OpenScriptFile.Title = "打开脚本文件";
            // 
            // SaveAsScriptFile
            // 
            SaveAsScriptFile.DefaultExt = "xml.kims";
            SaveAsScriptFile.Filter = "脚本文件|*.xml.kims|脚本文件|*.kims";
            SaveAsScriptFile.Title = "另存为脚本文件";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(612, 503);
            Controls.Add(Root_SplitContainer);
            Controls.Add(Main_ToolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = Main_ToolStrip;
            MinimumSize = new Size(288, 444);
            Name = "Main";
            Text = "按键输入宏";
            Load += Main_Load;
            GetWindows_GroupBox.ResumeLayout(false);
            ParentObjListAndSubObjList_SplitContainer.Panel1.ResumeLayout(false);
            ParentObjListAndSubObjList_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ParentObjListAndSubObjList_SplitContainer).EndInit();
            ParentObjListAndSubObjList_SplitContainer.ResumeLayout(false);
            Main_ToolStrip.ResumeLayout(false);
            Main_ToolStrip.PerformLayout();
            Root_SplitContainer.Panel1.ResumeLayout(false);
            Root_SplitContainer.Panel2.ResumeLayout(false);
            Root_SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Root_SplitContainer).EndInit();
            Root_SplitContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox GetWindows_GroupBox;
        private ListBox subObjList;
        private ListBox parentObjList;
        private SplitContainer ParentObjListAndSubObjList_SplitContainer;
        private Button flushWindowsList;
        private Button WriteObjToScriptButton;
        private TextBox scriptEditBox;
        private MenuStrip Main_ToolStrip;
        private ToolStripMenuItem Main_ToolStrip_Script;
        private ToolStripMenuItem Main_ToolStrip_OpenTestWindow;
        private ToolStripMenuItem Main_ToolStrip_Script_New;
        private ToolStripMenuItem Main_ToolStrip_Script_Open;
        private ToolStripMenuItem Main_ToolStrip_Script_SaveAs;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem Main_ToolStrip_Script_Run;
        private ToolStripMenuItem Main_ToolStrip_Script_Stop;
        private SplitContainer Root_SplitContainer;
        private ToolStripMenuItem Main_ToolStrip_Edit;
        private ToolStripMenuItem Main_ToolStrip_Edit_Format;
        private ToolStripMenuItem Main_ToolStrip_OpenLogWindow;
        private OpenFileDialog OpenScriptFile;
        private SaveFileDialog SaveAsScriptFile;
        private ToolStripMenuItem Main_ToolStrip_help;
        private ToolStripMenuItem Main_ToolStrip_help_helpDoc;
        private ToolStripMenuItem Main_ToolStrip_help_about;
    }
}
