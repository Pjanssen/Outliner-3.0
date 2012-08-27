using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Outliner.NodeSorters
{
   internal static class NativeMethods
   {
      [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      public static extern int StrCmpLogicalW(String x, String y);
   }
}