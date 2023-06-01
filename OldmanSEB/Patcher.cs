using System;
using System.Runtime.InteropServices;
using HarmonyLib;

namespace OldmanSEB
{
    public class Patcher
    {

        static int Initialize(String arguments)
        {
            //Console.WriteLine("Injected!");
            var harmony = new Harmony("com.seb.patcher");
            

            // Lets try to get into "SafeExamBrowser.Monitoring.Applications.ApplicationMonitor"
            // and add a prefix on method Start(), which adds the checks and starts the timer to check them
            var mOriginal = AccessTools.Method(AccessTools.TypeByName("SafeExamBrowser.Monitoring.Applications.ApplicationMonitor"), "SystemEvent_WindowChanged");
            if (mOriginal == null)
            {
                //Console.WriteLine("mOriginal not found");
            }
            var mPrefix = SymbolExtensions.GetMethodInfo(() => PrefixAppMonitor());

            var finalizer = SymbolExtensions.GetMethodInfo(() => Finalizer());
            harmony.Patch(mOriginal, prefix: new HarmonyMethod(mPrefix), finalizer: new HarmonyMethod(finalizer));
            Console.WriteLine("Patch for applications added (not ran yet).");

            var mOriginal1 = AccessTools.Method(AccessTools.TypeByName("SafeExamBrowser.Monitoring.Keyboard.KeyboardInterceptor"), "KeyboardHookCallback");
            if (mOriginal1 == null)
            {
                //Console.WriteLine("mOriginal1 not found");
            }
            var mPrefix1 = SymbolExtensions.GetMethodInfo(() => PrefixKeyboardInterceptor());

            var finalizer1 = SymbolExtensions.GetMethodInfo(() => Finalizer());
            harmony.Patch(mOriginal1, prefix: new HarmonyMethod(mPrefix1), finalizer: new HarmonyMethod(finalizer1));
            //Console.WriteLine("Patch for keyboard added (not ran yet).");


            var mOriginal2 = AccessTools.Method(AccessTools.TypeByName("SafeExamBrowser.Monitoring.Applications.ApplicationMonitor"), "Timer_Elapsed");
            if (mOriginal2 == null)
            {
                //Console.WriteLine("mOriginal2 not found");
            }
            var mPrefix2 = SymbolExtensions.GetMethodInfo(() => PrefixAppTimer());

            var finalizer2 = SymbolExtensions.GetMethodInfo(() => Finalizer());
            harmony.Patch(mOriginal2, prefix: new HarmonyMethod(mPrefix2), finalizer: new HarmonyMethod(finalizer2));
            //Console.WriteLine("Patch for app timer added (not ran yet).");


            // Launch a browser process
            var processInfo = new PROCESS_INFORMATION();
            var startupInfo = new STARTUPINFO();

            startupInfo.cb = Marshal.SizeOf(startupInfo);
            startupInfo.lpDesktop = "SafeBrowserExam";

            var success = CreateProcess(null, "cmd", IntPtr.Zero, IntPtr.Zero, true, 0x20, IntPtr.Zero, null, ref startupInfo, ref processInfo);
            if (!success)
            {
                //Console.WriteLine("Failed to launch command line");
            }

            return 1;
        }

        static bool PrefixAppMonitor()
        {
            //Console.WriteLine("Ran application patch");
            return false;
        }

        static bool PrefixKeyboardInterceptor()
        {
            //Console.WriteLine("Ran KeyboardInterceptor patch");
            return false;
        }

        static bool PrefixAppTimer()
        {
            //Console.WriteLine("Killed App Monitor timer");
            return false;
        }

        static Exception Finalizer()
        {
            return null;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }
}