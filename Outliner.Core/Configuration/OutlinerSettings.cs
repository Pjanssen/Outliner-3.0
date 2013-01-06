using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Max;
using Autodesk.Max.IColorManager;
using Outliner.Controls.Tree;

namespace Outliner.Configuration
{
   public static class OutlinerSettings
   {
      public const String CoreCategory = "OutlinerCore";

      public const String ColorSchemeFile = "ColorScheme";
      public const String DragDropMouseButton = "DragDropButton";
      public const String DoubleClickAction = "DoubleClickAction";

      public static void PopulateWithDefaults(SettingsCollection settings)
      {
         Throw.IfArgumentIsNull(settings, "settings");

         SetDefaultValue<String>(settings, CoreCategory, ColorSchemeFile, GetDefaultColorScheme() + ".xml");
         SetDefaultValue<MouseButtons>(settings, CoreCategory, DragDropMouseButton, MouseButtons.Left);
         SetDefaultValue<TreeNodeDoubleClickAction>(settings, CoreCategory, DoubleClickAction, TreeNodeDoubleClickAction.Rename);
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
