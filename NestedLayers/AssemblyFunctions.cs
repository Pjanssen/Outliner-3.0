using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Runtime.InteropServices;
using MaxUtils;

namespace Outliner.LayerTools
{
   public static class AssemblyFunctions
   {
      public static void AssemblyMain()
      {
         IGlobal global = MaxInterfaces.Global;

         global.RegisterNotification(postStart, null, SystemNotificationCode.SystemStartup);
      }

      private static void postStart(IntPtr param, IntPtr info)
      {
         NestedLayers.Start();
         NestedLayersMxs.Start();
         AutoInheritProperties.Start();
         AutoInheritPropertiesMxs.Start();
         ColorTags.Start();
         ColorTagsMxs.Start();
      }

      public static void AssemblyShutdown()
      {
         NestedLayers.Stop();
         AutoInheritProperties.Stop();
      }
   }
}
