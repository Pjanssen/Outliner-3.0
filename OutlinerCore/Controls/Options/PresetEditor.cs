using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Presets;
using Outliner.Plugins;
using Autodesk.Max;
using MaxUtils;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Options
{
public partial class PresetEditor : Form
{
   private OutlinerPreset editingPreset;
   
   public PresetEditor()
   {
      InitializeComponent();

      Color backColor       = ColorHelpers.FromMaxGuiColor(GuiColors.Background);
      Color foreColor       = ColorHelpers.FromMaxGuiColor(GuiColors.Text);
      Color windowColor     = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.BackColor = backColor;
      this.ForeColor = foreColor;
      presetsTree.Colors = new Tree.TreeViewColorScheme();
      presetsTree.Colors.Background = new SerializableColor(windowColor);
      presetsTree.Colors.ForegroundLight = new SerializableColor(windowTextColor);
      presetsTree.Colors.ParentBackground = new SerializableColor(windowColor);
      presetsTree.Colors.ParentForeground = new SerializableColor(windowTextColor);
      presetsTree.Colors.AlternateBackground = false;
      presetsTree.TreeNodeLayout.FullRowSelect = true;

      this.propertiesGroupBox.BackColor = backColor;
      this.propertiesGroupBox.ForeColor = foreColor;
      this.filterGroupBox.BackColor = backColor;
      this.filterGroupBox.ForeColor = foreColor;
      this.layoutGroupBox.BackColor = backColor;
      this.layoutGroupBox.ForeColor = foreColor;

      this.nameTextBox.BackColor = windowColor;
      this.nameTextBox.ForeColor = windowTextColor;
      this.modeComboBox.BackColor = windowColor;
      this.modeComboBox.ForeColor = windowTextColor;
      this.sorterComboBox.BackColor = windowColor;
      this.sorterComboBox.ForeColor = windowTextColor;

      this.layoutTree.TreeNodeLayout = new TreeNodeLayout();
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());

      this.propertyGrid1.ViewBackColor = windowColor;
      this.propertyGrid1.ViewForeColor = windowTextColor;

      this.FillPresetList();
      this.FillLists();

      this.FillPresetProperties();
   }

   private void FillPresetList()
   {
      OutlinerGUP outliner = OutlinerGUP.Instance;
      if (outliner == null)
         return;

      Outliner.Controls.Tree.TreeNode defaultTn = new Outliner.Controls.Tree.TreeNode("Default Presets");
      defaultTn.IsExpanded = true;
      
      foreach (OutlinerPreset preset in outliner.Presets.Where(p => p.IsDefaultPreset))
      {
         Tree.TreeNode tn = new Tree.TreeNode(preset.Name);
         tn.Tag = preset;
         defaultTn.Nodes.Add(tn);
      }

      Outliner.Controls.Tree.TreeNode customTn = new Outliner.Controls.Tree.TreeNode("Custom Presets");
      customTn.IsExpanded = true;

      foreach (OutlinerPreset preset in outliner.Presets.Where(p => !p.IsDefaultPreset))
      {
         Tree.TreeNode tn = new Tree.TreeNode(preset.Name);
         tn.Tag = preset;
         customTn.Nodes.Add(tn);
      }

      presetsTree.Nodes.Add(defaultTn);
      presetsTree.Nodes.Add(customTn);
   }

   private void FillLists()
   {
      IEnumerable<OutlinerPluginData> modes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeMode);
      foreach (OutlinerPluginData mode in modes)
      {
         this.modeComboBox.Items.Add(mode.DisplayName);
      }

      IEnumerable<OutlinerPluginData> sorters = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.NodeSorter);
      foreach (OutlinerPluginData sorter in sorters)
      {
         this.sorterComboBox.Items.Add(sorter.DisplayName);
      }

      IEnumerable<OutlinerPluginData> filters = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.Filter);
      foreach (OutlinerPluginData filter in filters)
      {
         this.filtersComboBox.Items.Add(filter.DisplayName);
      }

      IEnumerable<OutlinerPluginData> layoutItems = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeNodeButton);
      foreach (OutlinerPluginData layoutItem in layoutItems)
      {
         this.layoutComboBox.Items.Add(layoutItem.DisplayName);
      }
   }


   private void FillPresetProperties()
   {
      Boolean enabled = this.editingPreset != null;

      foreach (Control control in this.propertiesPanel.Controls)
      {
         if (control != presetsTree && control != addDeletePanel
                                    && control != okCancelPanel)
         {
            if (control is GroupBox)
            {
               foreach (Control groupControl in control.Controls)
               {
                  groupControl.Enabled = enabled;
               }
            }
            else
               control.Enabled = enabled;
         }
      }

      if (enabled)
      {
         this.nameTextBox.Text = this.editingPreset.Name;

         //this.filterEnabledCheckBox.Checked = editingPreset.Filters.Enabled;
         this.filtersTree.Nodes.Clear();
         foreach (Filter<IMaxNodeWrapper> item in this.editingPreset.Filters)
         {
            this.filtersTree.Nodes.Add(new Tree.TreeNode(item.GetType().Name));
         }

         this.layoutTree.Nodes.Clear();
         foreach (TreeNodeLayoutItem item in this.editingPreset.TreeNodeLayout.LayoutItems)
         {
            Tree.TreeNode tn = new Tree.TreeNode(item.GetType().Name);
            tn.Tag = item;
            this.layoutTree.Nodes.Add(tn);
         }
      }
   }

   private void presetsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
   {
      if (e.Item != null)
      {
         this.editingPreset = e.Item.Tag as OutlinerPreset;
      }

      this.FillPresetProperties();
   }

   private void cancelBtn_Click(object sender, EventArgs e)
   {
      this.Close();
   }

   private void presetsTree_BeforeNodeTextEdit(object sender, Tree.BeforeNodeTextEditEventArgs e)
   {
      e.Cancel = true;
   }

   private void presetsTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      if (e.Nodes.Count() > 0)
         this.editingPreset = e.Nodes.First().Tag as OutlinerPreset;
      else
         this.editingPreset = null;

      this.FillPresetProperties();
   }

   private void layoutTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      this.propertyGrid1.SelectedObject = e.Nodes.First().Tag;
   }
}
}
