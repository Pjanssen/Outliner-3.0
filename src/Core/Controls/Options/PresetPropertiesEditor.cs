using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Plugins;
using Outliner.Configuration;
using Outliner.Controls.Tree.Layout;
using System.IO;
using Outliner.Controls.ContextMenu;

namespace Outliner.Controls.Options
{
   public partial class PresetPropertiesEditor : OutlinerUserControl
   {
      private OutlinerPreset preset;

      public PresetPropertiesEditor()
      {
         InitializeComponent();
      }

      public PresetPropertiesEditor(OutlinerPreset preset) : this()
      {
         this.preset = preset;
      }

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);

         IEnumerable<OutlinerPluginData> modes = OutlinerPlugins.GetPlugins(OutlinerPluginType.TreeMode);
         this.modesComboBox.DataSource = modes.ToArray();
         this.modesComboBox.DisplayMember = "DisplayName";
         this.modesComboBox.ValueMember = "Type";

         IEnumerable<String> layoutFiles = Configurations.GetConfigurationFiles(OutlinerPaths.LayoutsDir)
                                                               .Select(f => Path.GetFileName(f));
         this.layoutComboBox.DataSource = layoutFiles.ToArray();

         IEnumerable<String> contextMenuFiles = Configurations.GetConfigurationFiles(OutlinerPaths.ContextMenusDir)
                                                                    .Select(f => Path.GetFileName(f));
         this.contextMenuComboBox.DataSource = contextMenuFiles.ToArray();

         this.outlinerPresetBindingSource.DataSource = this.preset;
      }
   }
}
