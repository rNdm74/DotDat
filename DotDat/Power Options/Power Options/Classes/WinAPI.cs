using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Power_Options.Classes
{
    /// <summary>
    /// Access the WINAPI
    /// </summary>
    public class WinAPI
    {
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public override string ToString()
            {
                return "(" + left + ", " + top + ") --> (" + right + ", " + bottom + ")";
            }
        }

        public enum IDC
        {
            HAND = 32649,
            // other values omitted
        }

        public static IntPtr GetTrayHandle()
        {
            var taskBarHandle = WinAPI.FindWindow("Shell_TrayWnd", null);
            return !taskBarHandle.Equals(IntPtr.Zero) ? WinAPI.FindWindowEx(taskBarHandle, IntPtr.Zero, "TrayNotifyWnd", IntPtr.Zero) : IntPtr.Zero;
        }

        public static Rectangle GetTrayRectangle()
        {
            RECT rect;
            GetWindowRect(WinAPI.GetTrayHandle(), out rect);
            return new Rectangle(new Point(rect.left, rect.top), new Size((rect.right - rect.left) + 1, (rect.bottom - rect.top) + 1));
        }
        
        #region DLL imports

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC cursor);

        #endregion
    }
}
