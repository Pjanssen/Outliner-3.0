using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.LayerTools
{
   public static class LayerNotificationCode
   {
      public const SystemNotificationCode LayerParented = (SystemNotificationCode)0x00000100;
      public const SystemNotificationCode LayerPropertyChanged = (SystemNotificationCode)0x00000101;
   }
}
