using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Outliner
{
   public static class NativeMethods
   {
      internal const int SB_HOR = 0;
      internal const int SB_VER = 1;

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      internal static extern int GetScrollPos(IntPtr hWnd, int nBar);
      
      [DllImport("user32.dll")]
      internal static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

      public const int GWL_STYLE = -16;
      public const int WS_VSCROLL = 0x00200000;
      public const int WS_HSCROLL = 0x00100000;

      [DllImport("user32.dll", SetLastError = true)]
      public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

      public static ScrollBars GetVisibleScrollbars(Control ctl)
      {
         int wndStyle = GetWindowLong(ctl.Handle, GWL_STYLE);
         bool hsVisible = (wndStyle & WS_HSCROLL) != 0;
         bool vsVisible = (wndStyle & WS_VSCROLL) != 0;

         if (hsVisible)
            return vsVisible ? ScrollBars.Both : ScrollBars.Horizontal;
         else
            return vsVisible ? ScrollBars.Vertical : ScrollBars.None;
      }

      [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      internal static extern int StrCmpLogicalW(String x, String y);
   }
}
