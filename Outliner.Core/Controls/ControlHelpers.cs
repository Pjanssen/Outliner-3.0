using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Outliner.Controls
{
public class ControlHelpers
{
   public static MessageBoxOptions GetMessageBoxOptionsForControl(Control c)
   {
      if (c.RightToLeft == RightToLeft.Yes)
      {
         return MessageBoxOptions.ServiceNotification
                | MessageBoxOptions.RightAlign
                | MessageBoxOptions.RtlReading;
      }
      else
      {
         return MessageBoxOptions.ServiceNotification;
      }
   }
}
}
