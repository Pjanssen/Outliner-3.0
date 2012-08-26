using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner
{
   public static class AssemblyFunctions
   {
      private static GlobalDelegates.Delegate5 ProcPostStart;

      public static void AssemblyMain() 
      {
         ProcPostStart = new GlobalDelegates.Delegate5(PostStart);

         IGlobal global = MaxUtils.MaxInterfaces.Global;
         global.RegisterNotification(ProcPostStart, null, SystemNotificationCode.SystemStartup);
      }

      private static void PostStart(IntPtr param, IntPtr info)
      {
         IGlobal global = MaxUtils.MaxInterfaces.Global;
         global.UnRegisterNotification(ProcPostStart, null, SystemNotificationCode.SystemStartup);

         OutlinerGUP.Start();
      }

      public static void AssemblyShutdown() 
      {
         OutlinerGUP instance = OutlinerGUP.Instance;
         if (instance != null)
            instance.Stop();
      }
   }
}
