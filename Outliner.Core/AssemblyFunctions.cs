using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner
{
public static class AssemblyFunctions
{
   private static GlobalDelegates.Delegate5 ProcPostStart;

   public static void AssemblyMain() 
   {
      ProcPostStart = new GlobalDelegates.Delegate5(PostStart);

      MaxInterfaces.Global.RegisterNotification( ProcPostStart
                                               , null
                                               , SystemNotificationCode.SystemStartup);
   }

   private static void PostStart(IntPtr param, IntPtr info)
   {
      MaxInterfaces.Global.UnRegisterNotification( ProcPostStart
                                                 , null
                                                 , SystemNotificationCode.SystemStartup);

      OutlinerGUP.Start();
   }

   public static void AssemblyShutdown() 
   {
      GroupHelpers.Stop();
      OutlinerPlugins.StopPlugins();

      OutlinerGUP instance = OutlinerGUP.Instance;
      if (instance != null)
         instance.Stop();
   }
}
}
