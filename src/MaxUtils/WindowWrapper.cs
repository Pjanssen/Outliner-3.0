using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.MaxUtils
{
   /// <summary>
   /// The WindowWrapper implements IWin32Window and wraps an IntPtr window handle.
   /// </summary>
   public class WindowWrapper : System.Windows.Forms.IWin32Window
   {
      /// <summary>
      /// Initializes a new instance of the WindowWrapper class.
      /// </summary>
      /// <param name="handle">The handle of the window.</param>
      public WindowWrapper(IntPtr handle)
      {
         this.Handle = handle;
      }

      /// <summary>
      /// The handle to the window.
      /// </summary>
      public IntPtr Handle { get; private set; }
   }
}
