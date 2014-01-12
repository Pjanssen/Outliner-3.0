using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using ManagedServices;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using System.Threading;

namespace PJanssen.Outliner
{
public static class AssemblyFunctions
{
   private static GlobalDelegates.Delegate5 ProcPostStart;

   private static Mutex appMutex;
   private static Boolean appMutexCreated;

   public static void AssemblyMain() 
   {
      appMutex = new Mutex(true, "3dsmax", out appMutexCreated);
      
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
      MaxscriptSDK.ExecuteMaxscriptCommand(@"Outliner = (dotnetclass ""PJanssen.Outliner.OutlinerGUP"").Instance");
   }

   public static void AssemblyShutdown() 
   {
      GroupHelpers.Stop();
      OutlinerPlugins.StopPlugins();

      OutlinerGUP instance = OutlinerGUP.Instance;
      if (instance != null)
         instance.Stop();

      if (appMutexCreated && appMutex != null)
         appMutex.ReleaseMutex();
   }
}
}
