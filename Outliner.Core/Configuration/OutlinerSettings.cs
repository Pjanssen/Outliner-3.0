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

      public static SettingsCollection DefaultSettings
      {
         get
         {
            SettingsCollection settings = new SettingsCollection();

            settings.SetValue<MouseButtons>(CoreCategory, DragDropMouseButton, MouseButtons.Left);
            settings.SetValue<Outliner.Controls.Tree.TreeNodeDoubleClickAction>(CoreCategory, DoubleClickAction, Controls.Tree.TreeNodeDoubleClickAction.Rename);

            return settings;
         }
      }
   }
}
