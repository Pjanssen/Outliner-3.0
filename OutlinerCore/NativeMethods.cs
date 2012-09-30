using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Outliner
{
   internal static class NativeMethods
   {
      [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      public static extern int StrCmpLogicalW(String x, String y);

      private const int WM_SETREDRAW = 0x000B;

      public static void SuspendDrawing(Control c)
      {
         if (c == null)
            throw new ArgumentNullException("c");
         NativeMethods.SendMessage(c.Handle, NativeMethods.WM_SETREDRAW, (Int32)0, (Int32)0);
      }

      public static void ResumeDrawing(Control c)
      {
         if (c == null)
            throw new ArgumentNullException("c");
         NativeMethods.SendMessage(c.Handle, NativeMethods.WM_SETREDRAW, (Int32)1, (Int32)0);
         c.Refresh();
      }

      [DllImport("User32")]
      private static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
   }
}
