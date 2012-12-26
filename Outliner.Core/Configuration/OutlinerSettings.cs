using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Outliner.Configuration
{
   public static class OutlinerSettings
   {
      public const String CoreCategory = "OutlinerCore";

      public const String DragDropMouseButton = "DragDropButton";
      public const String DoubleClickAction = "DoubleClickAction";

      public static void PopulateWithDefaults(SettingsCollection settings)
      {
         SetDefaultValue<MouseButtons>(settings, CoreCategory, DragDropMouseButton, MouseButtons.Left);
         SetDefaultValue<Outliner.Controls.Tree.TreeNodeDoubleClickAction>(settings, CoreCategory, DoubleClickAction, Outliner.Controls.Tree.TreeNodeDoubleClickAction.Rename);
      }

      private static void SetDefaultValue<T>(SettingsCollection settings, String category, String key, T defaultValue)
      {
         if (!settings.ContainsValue(category, key))
         {
            settings.SetValue<T>(category, key, defaultValue);
         }
      }
   }
}
