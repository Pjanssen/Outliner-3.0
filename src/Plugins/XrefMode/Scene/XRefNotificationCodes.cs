using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;

namespace PJanssen.Outliner.Scene
{
   public static class XRefNotificationCodes
   {
      public const SystemNotificationCode XRefSceneFlagsChanged = (SystemNotificationCode)0x00000120;
      public const SystemNotificationCode XRefSceneDeleted = (SystemNotificationCode)0x00000121;
      public const SystemNotificationCode XRefObjectRecordDeleted = (SystemNotificationCode)0x00000122;
   }
}
