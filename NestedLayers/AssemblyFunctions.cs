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
         IGlobal global = MaxInterfaces.Global;
         NestedLayers.Start(global);
         NestedLayersMXS.Start(global);
         AutoInheritProperties.Start(global);
         AutoInheritPropertiesMXS.Start(global);
         ColorTags.Start(global);
         ColorTagsMXS.Start(global);
      }

      public static void AssemblyShutdown()
      {
         IGlobal global = MaxInterfaces.Global;
         NestedLayers.Stop(global);
         AutoInheritProperties.Stop(global);
      }
   }
}
