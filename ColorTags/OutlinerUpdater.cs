using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.Scene;
using Outliner.Filters;


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
      private static IGlobal global;

      [OutlinerPluginStart]
      public static void Start()
      {
         ProcColorChanged = new GlobalDelegates.Delegate5(ColorChanged);
         global = MaxInterfaces.Global;
         global.RegisterNotification(ProcColorChanged, null, ColorTags.TagChanged);
      }

      [OutlinerPluginStop]
      public static void Stop()
      {
         if (global != null && ProcColorChanged != null)
            global.UnRegisterNotification(ProcColorChanged, null, ColorTags.TagChanged);

         global = null;
         ProcColorChanged = null;
      }

      private static void ColorChanged(IntPtr param, IntPtr info)
      {
         OutlinerGUP outliner = OutlinerGUP.Instance;
         if (outliner == null)
            return;

         IAnimatable node = MaxUtils.HelperMethods.GetCallParam(info) as IAnimatable;
         foreach (TreeMode treeMode in outliner.TreeModes.Values)
         {
            treeMode.InvalidateObject(node, false, treeMode.Tree.NodeSorter is ColorTagSorter);

            if (ContainsColorTagsFilter(treeMode.Filters.Filters))
               treeMode.UpdateFilter(node);
         }
      }

      private static Boolean ContainsColorTagsFilter(FilterCollection<IMaxNodeWrapper> collection)
      {
         foreach (Filter<IMaxNodeWrapper> filter in collection)
         {
            if (filter is ColorTagsFilter)
               return true;

            FilterCombinator<IMaxNodeWrapper> combinator = filter as FilterCombinator<IMaxNodeWrapper>;
            if (combinator != null && ContainsColorTagsFilter(combinator.Filters))
               return true;
         }

         return false;
      }
   }
}

