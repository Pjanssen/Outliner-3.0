using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.IO;
using System.Reflection;

namespace Outliner.MaxUtils
{
public static class HelperMethods
{
   /// <summary>
   /// Converts the ITab to a more convenient IEnumerable.
   /// </summary>
   public static IEnumerable<T> ITabToIEnumerable<T>(ITab<T> tab)
   {
      Throw.IfArgumentIsNull(tab, "tab");
      
      for (int i = 0; i < tab.Count; i++)
         yield return tab[(IntPtr)i];
   }


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
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null)
         return notifyInfo.CallParam;
      else
         return null;
   }
}
}
