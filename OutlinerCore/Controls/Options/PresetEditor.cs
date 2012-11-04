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
using Outliner.MaxUtils;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.Scene;
using Outliner.Modes;

namespace Outliner.Controls.Options
{
public partial class PresetEditor : Form
{
   private OutlinerPreset editingPreset;
   private TreeMode previewMode;
   
   public PresetEditor()
   {
      InitializeComponent();

      Color backColor       = ColorHelpers.FromMaxGuiColor(GuiColors.Background);
      Color foreColor       = ColorHelpers.FromMaxGuiColor(GuiColors.Text);
      Color windowColor     = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);
      
      this.BackColor = backColor;
      this.ForeColor = foreColor;
      Tree.TreeViewColorScheme treeColors = new Tree.TreeViewColorScheme();
      treeColors.Background = new SerializableColor(windowColor);
      treeColors.ForegroundLight = new SerializableColor(windowTextColor);
      treeColors.ParentBackground = new SerializableColor(windowColor);
      treeColors.ParentForeground = new SerializableColor(windowTextColor);
      treeColors.AlternateBackground = false;
      this.presetsTree.Colors = treeColors;
      this.previewTree.Colors = treeColors;

      presetsTree.TreeNodeLayout.FullRowSelect = true;
      ((ExpandButton)presetsTree.TreeNodeLayout.LayoutItems.First()).UseVisualStyles = false;

      this.FillPresetList();
      this.FillPresetProperties();
      this.UpdatePreviewTree();
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
         Tree.TreeNode tn = createPresetTreeNode(preset);
         defaultTn.Nodes.Add(tn);
      }

      Outliner.Controls.Tree.TreeNode customTn = new Outliner.Controls.Tree.TreeNode("Custom Presets");
      customTn.IsExpanded = true;

      foreach (OutlinerPreset preset in outliner.Presets.Where(p => !p.IsDefaultPreset))
      {
         Tree.TreeNode tn = createPresetTreeNode(preset);
         customTn.Nodes.Add(tn);
      }

      presetsTree.Nodes.Add(defaultTn);
      presetsTree.Nodes.Add(customTn);
   }

   private Tree.TreeNode createPresetTreeNode(OutlinerPreset preset)
   {
      Tree.TreeNode tn = new Tree.TreeNode(preset.Name);
      tn.Tag = preset;

      Tree.TreeNode filterTn = new Tree.TreeNode("Filters");
      filterTn.Tag = preset.Filters;
      tn.Nodes.Add(filterTn);

      Tree.TreeNode layoutTn = new Tree.TreeNode("Layout");
      layoutTn.Tag = preset.TreeNodeLayout;
      tn.Nodes.Add(layoutTn);
      
      return tn;
   }


   private void FillPresetProperties()
   {
      this.propertiesPanel.Controls.Clear();

      Tree.TreeNode tn = this.presetsTree.SelectedNodes.FirstOrDefault();
      Control editor = null;

      if (tn != null && tn.Tag != null)
      {
         Type tagType = tn.Tag.GetType();

         if (tagType.Equals(typeof(OutlinerPreset)) || tagType.IsSubclassOf(typeof(OutlinerPreset)))
            editor = new PresetPropertiesEditor(this, tn.Tag as OutlinerPreset);
         else if (tagType == typeof(FilterCombinator<IMaxNodeWrapper>))
            editor = new FilterCollectionEditor(this, this.editingPreset);
         else if (tagType == typeof(TreeNodeLayout))
            editor = new TreeNodeLayoutEditor(this, tn.Tag as TreeNodeLayout);

         if (editor != null)
         {
            editor.Dock = DockStyle.Fill;
            this.propertiesPanel.Controls.Add(editor);
         }
      }

      this.previewGroupBox.Visible = editor != null;
   }

   private void SetDeleteButtonState()
   {
      if (this.editingPreset != null)
      {
         this.deleteBtn.Enabled = true;
         this.deleteBtn.Text = this.editingPreset.IsDefaultPreset ? "Reset" : "Delete";
      }
      else
         this.deleteBtn.Enabled = false;
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
      Tree.TreeNode tn = e.Nodes.FirstOrDefault();
      if (tn != null && tn.Tag != null)
      {
         this.editingPreset = tn.Tag as OutlinerPreset;
         if (this.editingPreset == null)
         {
            this.editingPreset = tn.Parent.Tag as OutlinerPreset;
         }
      }
      else
         this.editingPreset = null;

      this.FillPresetProperties();
      this.SetDeleteButtonState();
      this.UpdatePreviewTree();
   }

   internal void UpdatePreviewTree()
   {
      if (this.previewMode != null)
      {
         this.previewMode.Stop();
      }

      this.previewTree.Nodes.Clear();

      if (this.editingPreset != null)
      {
         this.previewTree.TreeNodeLayout = this.editingPreset.TreeNodeLayout;
         this.previewTree.NodeSorter = this.editingPreset.NodeSorter;
         this.previewMode = this.editingPreset.CreateTreeMode(this.previewTree);
         this.previewMode.Start();

         if (this.previewTree.Nodes.Count == 0)
         {
            Tree.TreeNode tn = new Tree.TreeNode("No nodes to show");
            tn.Nodes.Add(new Tree.TreeNode("Open a scene for a proper preview"));
            this.previewTree.Nodes.Add(tn);
         }

         if (this.previewTree.SelectedNodes.Count() == 0)
         {
            Tree.TreeNode tn = this.previewTree.Nodes.FirstOrDefault();
            if (tn != null)
            {
               this.previewTree.SelectNode(tn, true);
            }
         }

         this.previewTree.Root.ExpandAll();
      }
   }
}
}
