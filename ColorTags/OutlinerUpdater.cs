using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Autodesk.Max;
using MaxUtils;
using Outliner.Modes;

namespace Outliner.ColorTags
{
   /// <summary>
   /// This class responds to ColorChanged SystemNotifications and updates the
   /// Outliner treeviews.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public static class OutlinerUpdater
   {
      private static GlobalDelegates.Delegate5 ProcColorChanged;


      [OutlinerPluginStart]
      public static void Start()
      {
         ProcColorChanged = new GlobalDelegates.Delegate5(ColorChanged);
         IGlobal global = MaxInterfaces.Global;
         global.RegisterNotification(ProcColorChanged, null, ColorTags.TagChanged);
      }


      [OutlinerPluginStop]
      public static void Stop()
      {
         IGlobal global = MaxInterfaces.Global;
         global.UnRegisterNotification(ProcColorChanged, null, ColorTags.TagChanged);
      }


      private static void ColorChanged(IntPtr param, IntPtr info)
      {
         OutlinerGUP outliner = OutlinerGUP.Instance;

         if (outliner == null)
            return;

         IAnimatable node = MaxUtils.HelperMethods.GetCallParam(info) as IAnimatable;
         foreach (TreeMode treeMode in outliner.TreeModes)
         {
            if (treeMode.Filters.Contains(typeof(ColorTagsFilter)))
               treeMode.UpdateFilter(node);

            treeMode.InvalidateObject(node, false, treeMode.Tree.NodeSorter is ColorTagsSorter);
         }
      }
   }
}
