using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace KeyInputMacro
{
    internal static class Program
    {
        public const string version = "1.0.0.20241025";

       internal static string[]? programArgs;        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            programArgs = args;
#if DEBUG
            {
                string all="程序启动参数: ";
                foreach (string arg in args)
                {
                    all += arg+" ";
                }
                Logger.LogAdd(all);
            }
#endif
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());         
        }        
    }
}