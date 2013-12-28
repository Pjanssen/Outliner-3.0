using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Max;
using Autodesk.Max.IColorManager;
using Outliner.Controls.Tree;
using PJanssen;

namespace Outliner.Configuration
{
   internal static class OutlinerSettings
   {
      public const String CoreCategory = "Core";
      public const String TreeCategory = "TreeView";

      public const String ColorSchemeFile = "ColorScheme";
      public const String DragDropMouseButton = "DragDropButton";
      public const String DoubleClickAction = "DoubleClickAction";
      public const String ScrollToSelection = "ScrollToSelection";
      public const String AutoExpandSelectionParents = "AutoExpandSelectionParents";
      public const String CollapseAutoExpandedParents = "CollapseAutoExpandedParents";

      public static void PopulateWithDefaults(SettingsCollection settings)
      {
         Throw.IfNull(settings, "settings");

         SetDefaultValue<String>(settings, CoreCategory, ColorSchemeFile, GetDefaultColorScheme() + ".xml");
         SetDefaultValue<MouseButtons>(settings, TreeCategory, DragDropMouseButton, MouseButtons.Left);
         SetDefaultValue<TreeNodeDoubleClickAction>(settings, TreeCategory, DoubleClickAction, TreeNodeDoubleClickAction.Rename);
         SetDefaultValue<Boolean>(settings, TreeCategory, ScrollToSelection, true);
         SetDefaultValue<Boolean>(settings, TreeCategory, AutoExpandSelectionParents, true);
         SetDefaultValue<Boolean>(settings, TreeCategory, CollapseAutoExpandedParents, true);         
      }

      private static void SetDefaultValue<T>(SettingsCollection settings, String category, String key, T defaultValue)
      {
         if (!settings.ContainsValue(category, key))
         {
            settings.SetValue<T>(category, key, defaultValue);
         }
      }

      private static String GetDefaultColorScheme()
      {
         if (MaxUtils.MaxInterfaces.ColorThemeLightActive)
            return "light";
         else
            return "dark";
      }
   }
}
