using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Outliner.Controls
{
public class ControlHelpers
{
   public static MessageBoxOptions GetLocalizedMessageBoxOptions()
   {
      if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
         return MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
      else
         return (MessageBoxOptions)0;
   }
}
}
