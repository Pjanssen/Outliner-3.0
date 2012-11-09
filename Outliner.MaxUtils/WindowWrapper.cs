using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.MaxUtils
{
   public class WindowWrapper : System.Windows.Forms.IWin32Window
   {
      public WindowWrapper(IntPtr handle)
      {
         this.Handle = handle;
      }

      public IntPtr Handle { get; private set; }
   }
}
