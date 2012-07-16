using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MaxUtils
{
internal static class NativeMethods
{
   internal delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

   [DllImport("user32.dll")]
   internal static extern bool EnumWindows(WindowEnumProc callPtr, IntPtr lParam);

   [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
   internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

   [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
   internal static extern int GetWindowTextLength(IntPtr hWnd);
}
}
