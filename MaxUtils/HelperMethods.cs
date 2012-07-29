using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.IO;
using System.Reflection;

namespace MaxUtils
{
public static class HelperMethods
{
   /// <summary>
   /// Iterates over all elements in the collection with the supplied function.
   /// </summary>
   public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
   {
      if (items == null)
         throw new ArgumentNullException("items");
      if (action == null)
         throw new ArgumentNullException("action");

      foreach (T item in items)
         action(item);
   }

   /// <summary>
   /// Converts the ITab to a more convenient IEnumerable.
   /// </summary>
   public static IEnumerable<T> ToIEnumerable<T>(this ITab<T> tab)
   {
      if (tab == null)
         throw new ArgumentNullException("tab");

      List<T> lst = new List<T>(tab.Count);
      for (int i = 0; i < tab.Count; i++)
         lst.Add(tab[(IntPtr)i]);

      return lst;
   }

   /// <summary>
   /// Retrieves the IINodes from a ITab of handles.
   /// </summary>
   public static IEnumerable<IINode> NodeKeysToINodeList(this ITab<UIntPtr> handles)
   {
      return handles.ToIEnumerable().Select(MaxInterfaces.Global.NodeEventNamespace.GetNodeByKey);
   }


   public static void RunResourceScript(Assembly assembly, String res)
   {
      String script = String.Empty;

      using (Stream stream = assembly.GetManifestResourceStream(res))
      using (StreamReader sr = new StreamReader(stream))
      {
         script = sr.ReadToEnd();
      }

      if (script != String.Empty)
      {
         MaxInterfaces.Global.ExecuteMAXScriptScript(script, true, null);
      }
   }

   public static void WriteToListener(String text)
   {
      MaxInterfaces.Global.TheListener.EditStream.Wputs(text + "\n");
      MaxInterfaces.Global.TheListener.EditStream.Wflush();
   }


   public static IntPtr GetMtlEditorHWND()
   {
      IntPtr mtlPtr = IntPtr.Zero;
      NativeMethods.EnumWindows(
          (IntPtr hwnd, IntPtr lparam) =>
          {
             if (HwndIsMtlEditor(hwnd))
             {
                mtlPtr = hwnd;
                return false;
             }
             return true;
          }
          , IntPtr.Zero);
      return mtlPtr;
   }

   public static IntPtr GetSlateMtlEditorHWND()
   {
      IntPtr mtlPtr = IntPtr.Zero;
      NativeMethods.EnumWindows(
          (IntPtr hwnd, IntPtr lparam) =>
          {
             if (HwndIsSlateMtlEditor(hwnd))
             {
                mtlPtr = hwnd;
                return false;
             }
             return true;
          }
          , IntPtr.Zero);
      return mtlPtr;
   }


   private static String getHwndTitle(IntPtr hwnd)
   {
      int textLength = NativeMethods.GetWindowTextLength(hwnd);
      StringBuilder windowText = new StringBuilder(textLength + 1);
      NativeMethods.GetWindowText(hwnd, windowText, windowText.Capacity);
      return windowText.ToString();
   }

   private static bool HwndIsMtlEditor(IntPtr hwnd)
   {
      return getHwndTitle(hwnd).StartsWith("Material Editor");
   }

   private static bool HwndIsSlateMtlEditor(IntPtr hwnd)
   {
      return getHwndTitle(hwnd) == "Slate Material Editor";
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
