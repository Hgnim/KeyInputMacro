using System.Runtime.InteropServices;
using System.Text;

namespace WindowsAPI
{
    public static class GetWindows
    {
        //该类的部分内容引用于该文章 https://developer.aliyun.com/article/1094650
        /// <summary>
        /// 枚举窗口时的委托参数
        /// </summary>
        private delegate bool WndEnumProc(IntPtr hWnd, int lParam);
        /// <summary>
        /// 枚举所有窗口
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WndEnumProc lpEnumFunc, int lParam);
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lptrString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 获取窗口的父窗口句柄
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref LPRECT rect);
        [StructLayout(LayoutKind.Sequential)]
        private readonly struct LPRECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }
        /// <summary>
        /// 窗体列表
        /// </summary>
        private static List<WindowInfo> windowList = [];
        /// <summary>
        /// 获取 Win32 窗口的一些基本信息。
        /// </summary>
        public struct WindowInfo
        {
            public WindowInfo(IntPtr parentHWnd, IntPtr hWnd, string className, string title, bool isVisible, Rectangle bounds) : this()
            {
                ParentHwnd = parentHWnd;
                Hwnd = hWnd;
                ClassName = className;
                Title = title;
                IsVisible = isVisible;
                Bounds = bounds;
            }

            /// <summary>
            /// 父窗口句柄
            /// </summary>
            public IntPtr ParentHwnd { get; }

            /// <summary>
            /// 获取窗口句柄。
            /// </summary>
            public IntPtr Hwnd { get; }

            /// <summary>
            /// 获取窗口类名。
            /// </summary>
            public string ClassName { get; }

            /// <summary>
            /// 获取窗口标题。
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// 获取当前窗口是否可见。
            /// </summary>
            public bool IsVisible { get; }

            /// <summary>
            /// 获取窗口当前的位置和尺寸。
            /// </summary>
            public Rectangle Bounds { get; }

            /// <summary>
            /// 获取窗口当前是否是最小化的。
            /// </summary>
            public bool IsMinimized => Bounds.Left == -32000 && Bounds.Top == -32000;
        }
        /// <summary>
        /// 查找当前用户空间下所有符合条件的(顶层)窗口。如果不指定条件，将仅查找可见且有标题栏的窗口。
        /// </summary>
        /// <param name="match">过滤窗口的条件。如果设置为 null，将仅查找可见和标题栏不为空的窗口。</param>
        /// <returns>找到的所有窗口信息</returns>
        public static IReadOnlyList<WindowInfo> FindAllWindows(Predicate<WindowInfo> match = null!)
        {
            windowList = [];
            //遍历窗口并查找窗口相关WindowInfo信息
            EnumWindows(OnWindowEnum, 0);
            return windowList.FindAll(match ?? (x => x.IsVisible && !x.IsMinimized && x.Title.Length > 0)/*默认的查找窗口的过滤条件。可见 + 非最小化 + 包含窗口标题。*/);
        }
        /// <summary>
        /// 遍历窗体处理的函数
        /// </summary>
        /// <returns></returns>
        private static bool OnWindowEnum(IntPtr hWnd, int lparam)
        {
            // 仅查找顶层窗口。
            //if (GetParent(hWnd) == IntPtr.Zero)

            // 获取窗口类名。
            var lpString = new StringBuilder(512);
            GetClassName(hWnd, lpString, lpString.Capacity);
            var className = lpString.ToString();

            // 获取窗口标题。
            var lptrString = new StringBuilder(512);
            GetWindowText(hWnd, lptrString, lptrString.Capacity);
            var title = lptrString.ToString().Trim();

            // 获取窗口可见性。
            var isVisible = IsWindowVisible(hWnd);

            // 获取窗口位置和尺寸。
            LPRECT rect = default;
            GetWindowRect(hWnd, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            // 添加到已找到的窗口列表。
            windowList.Add(new WindowInfo(GetParent(hWnd), hWnd, className, title, isVisible, bounds));

            return true;
        }



        /// <summary>
        /// 遍历子窗体(控件)
        /// </summary>
        /// <param name="hwndParent">父窗口句柄</param>
        /// <param name="lpEnumFunc">遍历的回调函数</param>
        /// <param name="lParam">传给遍历时回调函数的额外数据</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WndEnumProc lpEnumFunc, int lParam);
        /// <summary>
        /// 查找父窗口下的子窗口和子控件
        /// </summary>
        /// <param name="parent">父窗口句柄</param>
        /// <param name="match">过滤条件</param>
        /// <returns></returns>
        public static IReadOnlyList<WindowInfo> FindAllChildWindows(IntPtr parent, Predicate<WindowInfo> match = null!)
        {
            windowList = [];
            EnumChildWindows(parent, OnWindowEnum, 0);
            return windowList.FindAll(match ?? (x => x.IsVisible && !x.IsMinimized && x.Title.Length > 0)/*默认的查找窗口的过滤条件。可见 + 非最小化 + 包含窗口标题。*/);
        }

        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="lpClassName">窗体的类名称，比如Form、Window。若不知道，指定为null即可</param>
        /// <param name="lpWindowName">窗体的标题/文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName = null!, string lpWindowName = null!);
        /// <summary>
        /// 查找子窗体（控件）
        /// </summary>
        /// <param name="hwndParent">父窗体句柄，不知道窗体时可指定IntPtr.Zero，但尽可能提供该参数</param>
        /// <param name="hwndChildAfter">子窗体(控件)，通常不知道子窗体(句柄)，指定0即可</param>
        /// <param name="lpszClass">子窗体(控件)的类名，通常指定null，它是window class name，并不等同于C#中的列名Button、Image、PictureBox等，两者并不相同，可通过GetClassName获取正确的类型名</param>
        /// <param name="lpszWindow">子窗体的名字或控件的Title、Text，通常为显示的文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
    }
    public static class KeyInput
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        //文档 https://learn.microsoft.com/zh-cn/windows/win32/inputdev/keyboard-input-notifications
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        /// <summary>
        /// 按键类型，例如按下信息、抬起信息、按压信息
        /// </summary>
        public enum KeyAction
        {
            KeyDown, KeyUp
        }
        /// <summary>
        /// 对指定窗口发送指定的键盘操作
        /// </summary>
        /// <param name="key">将发送的键盘按键</param>
        /// <param name="action">行动方法</param>
        /// <param name="hWnd">目标句柄</param>
        /// <returns>返回int值<br/>
        /// 0: 执行成功
        /// 1: 未找到指定窗口
        /// 2: 错误
        /// </returns>
        public static int SendAction(Keys key, KeyAction action, IntPtr hWnd)
        {
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    int actionValue = action switch
                    {
                        KeyAction.KeyDown => WM_KEYDOWN,
                        KeyAction.KeyUp => WM_KEYUP,
                        _ => throw new NotImplementedException()
                    };
                    /*int lParam = action switch
                    {
                        KeyAction.KeyDown => 0,
                        KeyAction.KeyUp => 0x8000,
                        _ => throw new NotImplementedException()
                    };*/
                    SendMessage(hWnd, actionValue, (int)key, 0);
                    return 0;
                }
                else
                    return 1;
            }
            catch { return 2; }
        }

        //文档 https://learn.microsoft.com/zh-cn/windows/win32/inputdev/mouse-input-notifications
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_XBUTTONDOWN = 0x020B;
        private const int WM_XBUTTONUP = 0x020C;

        public enum MouseAction
        {
            MouseDown, MouseUp
        }
        public enum MouseButton
        {
            LButton=Keys.LButton, MButton=Keys.MButton, RButton=Keys.RButton, XButton1=Keys.XButton1, XButton2=Keys.XButton2
        }
        /// <summary>
        /// 对指定的窗口发送指定的鼠标操作
        /// </summary>
        /// <param name="button">将发送的鼠标按键</param>
        /// <param name="action">行动方法</param>
        /// <param name="hWnd">目标句柄</param>
        /// <returns>返回int值<br/>
        /// 0: 执行成功
        /// 1: 未找到指定窗口
        /// 2: 错误
        /// </returns>
        public static int SendAction(MouseButton button, MouseAction action, IntPtr hWnd)
        {
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    int actionValue = button switch
                    {
                        MouseButton.LButton => WM_LBUTTONDOWN,
                        MouseButton.RButton => WM_RBUTTONDOWN,
                        MouseButton.MButton => WM_MBUTTONDOWN,
                        MouseButton.XButton1 => WM_XBUTTONDOWN,
                        MouseButton.XButton2 => WM_XBUTTONDOWN,
                        _ => throw new NotImplementedException()
                    };//根据按键进行赋值
                    switch (action)//判断操作方式
                    {
                        case MouseAction.MouseDown:
                            break;
                        case MouseAction.MouseUp:
                            actionValue++;
                            break;
                    }

                    int wParam = 0;
                    if (button == MouseButton.XButton1 || button == MouseButton.XButton2)//判断是否是鼠标侧键
                    {
                        switch (button)
                        {
                            case MouseButton.XButton1:
                                wParam = 0x0001; break;
                            case MouseButton.XButton2:
                                wParam = 0x0002; break;
                        }
                    }

                    SendMessage(hWnd, actionValue, wParam, 0);
                    return 0;
                }
                else return 1;
            }
            catch { return 2; }
        }

        private const int WM_CHAR = 0x0102;
        /// <summary>
        　/// 对指定窗口发送指定的Char操作
        　/// </summary>
        /// <param name="sendChar">将发送的Char</param>
        /// <param name="hWnd">目标句柄</param>
        /// <returns>返回int值<br/>
        /// 0: 执行成功
        /// 1: 未找到指定窗口
        /// 2: 错误
        /// </returns>
        public static int SendAction(char sendChar, IntPtr hWnd)
        {
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    SendMessage(hWnd, WM_CHAR, sendChar, 0);
                    return 0;
                }
                else return 1;
            }
            catch { return 2; }
        }

        /// <summary>
        /// 对指定窗口发送指定的字符串操作
        /// </summary>
        /// <param name="sendStr">将发送的字符串</param>
        /// <param name="hWnd">目标句柄</param>
        /// <returns>返回int值<br/>
        /// 0: 执行成功
        /// 1: 未找到指定窗口
        /// 2: 错误
        /// </returns>
        public static int SendAction(string sendStr, IntPtr hWnd)
        {
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    char[] chars= sendStr.ToCharArray();
                    foreach(char c in chars)
                    {
                        int returnValue = SendAction(c, hWnd);
                        if(returnValue != 0)
                        {
                            return returnValue;
                        }
                    }
                    return 0;
                }
                else return 1;
            }
            catch { return 2; }
        }
    }
}
