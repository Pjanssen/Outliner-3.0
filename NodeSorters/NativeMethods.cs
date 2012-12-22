using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Outliner.NodeSorters
{
   internal static class NativeMethods
   {
      [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      public static extern int StrCmpLogicalW(String x, String y);
   }
}
