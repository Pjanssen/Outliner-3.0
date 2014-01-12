using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace PJanssen.Outliner.LayerTools
{
   /// <summary>
   /// Provides custom SystemNotificationCodes for layer operations performed by classes in this assembly.
   /// </summary>
   public static class LayerNotificationCode
   {
      /// <summary>
      /// The layer has been parented or unparented.
      /// </summary>
      public const SystemNotificationCode LayerParented = (SystemNotificationCode)0x00000100;

      /// <summary>
      /// A property of the layer has changed.
      /// </summary>
      public const SystemNotificationCode LayerPropertyChanged = (SystemNotificationCode)0x00000101;

      /// <summary>
      /// The layer's current state has been changed.
      /// </summary>
      public const SystemNotificationCode LayerCurrentChanged = (SystemNotificationCode)0x00000102;
   }
}
