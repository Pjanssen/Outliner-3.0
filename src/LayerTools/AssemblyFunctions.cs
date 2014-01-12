using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Runtime.InteropServices;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.LayerTools
{
   public static class AssemblyFunctions
   {
      private static GlobalDelegates.Delegate5 ProcPostStart;

      public static void AssemblyMain()
      {
         ProcPostStart = new GlobalDelegates.Delegate5(PostStart);

         IGlobal global = MaxInterfaces.Global;
         global.RegisterNotification(ProcPostStart, null, SystemNotificationCode.SystemStartup);
      }

      private static void PostStart(IntPtr param, IntPtr info)
      {
         IGlobal global = MaxInterfaces.Global;
         global.UnRegisterNotification(ProcPostStart, null, SystemNotificationCode.SystemStartup);

         NestedLayers.Start();
         NestedLayersMxs.Start();
         AutoInheritProperties.Start();
         AutoInheritPropertiesMxs.Start();
      }

      public static void AssemblyShutdown()
      {
         NestedLayers.Stop();
         AutoInheritProperties.Stop();
      }
   }
}
