using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Presets;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Plugins;
using System.Reflection;
using System.Resources;
using Outliner.NodeSorters;

namespace Outliner.Controls.Options
{
public partial class PresetTreePropertiesEditor : UserControl
{
   private OutlinerPreset preset;
   private Action updateAction;

   public PresetTreePropertiesEditor()
   {
      InitializeComponent();
   }

   public PresetTreePropertiesEditor(OutlinerPreset preset, Action updateAction) : this()
   {
      this.preset = preset;
      this.updateAction = updateAction;
   }

   protected override void OnLoad(EventArgs e)
   {
      Color windowColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.SetControlColor(this.modeComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.sorterComboBox, windowColor, windowTextColor);

      //Mode combobox data source.
      IEnumerable<OutlinerPluginData> modes = OutlinerPlugins.GetPlugins(OutlinerPluginType.TreeMode);
      this.modeComboBox.DataSource = modes.ToList();
      this.modeComboBox.ValueMember = "Type";
      this.modeComboBox.DisplayMember = "DisplayName";


      IEnumerable<OutlinerPluginData> sorters = OutlinerPlugins.GetPlugins(OutlinerPluginType.NodeSorter);
      this.sorterComboBox.DataSource = sorters.ToList();
      this.sorterComboBox.ValueMember = "Type";
      this.sorterComboBox.DisplayMember = "DisplayName";

      this.sorterComboBox.SelectedValue = this.preset.NodeSorter.GetType();
      this.sorterComboBox.SelectedValueChanged += this.sorterComboBox_SelectedValueChanged;
      
      this.presetBindingSource.DataSource = preset;
      this.nodeSorterBindingSource.DataSource = preset.NodeSorter;

       base.OnLoad(e);
   }


   private void SetControlColor(Control c, Color backColor, Color foreColor)
   {
      c.BackColor = backColor;
      c.ForeColor = foreColor;
   }




   private void presetBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();
      }
   }

   private void nodeSorterBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();
      }
   }


   

   private void sorterComboBox_SelectedValueChanged(object sender, EventArgs e)
   {
      Boolean ascending = this.preset.NodeSorter.Ascending;
      Type selectedType = ((OutlinerPluginData)this.sorterComboBox.SelectedItem).Type;

      this.preset.NodeSorter = Activator.CreateInstance(selectedType) as NodeSorter;
      this.preset.NodeSorter.Ascending = ascending;

      if (this.updateAction != null)
         this.updateAction();
   }


   private String browseXmlFile(String filename)
   {
      openContextMenuFileDialog.InitialDirectory = OutlinerPaths.ContextMenusDir;
      openContextMenuFileDialog.FileName = filename;
      if (openContextMenuFileDialog.ShowDialog() == DialogResult.OK)
      {
         Uri localFile = new Uri(openContextMenuFileDialog.FileName);
         Uri presetsPath = new Uri(OutlinerPaths.ContextMenusDir);
         Uri relativePath = presetsPath.MakeRelativeUri(localFile);
         return Uri.UnescapeDataString(relativePath.ToString());
      }
      else
         return null;
   }

   private void contextMenuBrowseBtn_Click(object sender, EventArgs e)
   {
      String browseResult = browseXmlFile(this.preset.ContextMenuFile);
      if (!String.IsNullOrEmpty(browseResult))
      {
         this.preset.ContextMenuFile = browseResult;
         this.presetBindingSource.ResetCurrentItem();
      }
   }



}
}
