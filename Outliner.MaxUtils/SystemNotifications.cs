using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.IO;
using System.Reflection;

namespace Outliner.MaxUtils
{
public static class SystemNotifications
{
   /// <summary>
   /// Marshals the INotifyInfo object from a pointer sent by a general event callback.
   /// </summary>
   public static INotifyInfo GetNotifyInfo(IntPtr info)
   {
      return MaxInterfaces.Global.NotifyInfo.Marshal(info);
   }

   /// <summary>
   /// Gets the callparam object from an INotifyInfo pointer.
   /// </summary>
   /// <param name="info">A pointer to a NotifyInfo object</param>
   public static Object GetCallParam(IntPtr info)
   {
      INotifyInfo notifyInfo = SystemNotifications.GetNotifyInfo(info);
      if (notifyInfo != null)
         return notifyInfo.CallParam;
      else
         return null;
   }
}
}
