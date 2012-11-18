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
   private enum EditorType
   {
      ContextMenu,
      Filters,
      Layout
   }

   private OutlinerPreset editingPreset;
   private Tree.TreeView previewTree;
   private TreeMode previewMode;
   private Outliner.Controls.Tree.TreeNode customPresetsTn;
   private Dictionary<OutlinerPreset, Tree.TreeNode> treeNodes;
   
   public PresetEditor(Tree.TreeView tree)
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

      this.previewTree = tree;

      presetsTree.TreeNodeLayout.FullRowSelect = true;
      ((ExpandButton)presetsTree.TreeNodeLayout.LayoutItems.First()).UseVisualStyles = false;

      this.FillPresetList();
      this.FillPresetProperties();
      this.UpdatePreviewTree();
   }

   private void FillPresetList()
   {
      this.presetsTree.Nodes.Clear();
      this.treeNodes = new Dictionary<OutlinerPreset, Tree.TreeNode>();
      Outliner.Controls.Tree.TreeNode defaultTn = new Outliner.Controls.Tree.TreeNode("Default Presets");
      defaultTn.IsExpanded = true;
      
      foreach (OutlinerPreset preset in OutlinerPresets.Presets.Where(p => p.IsDefaultPreset))
      {
         Tree.TreeNode tn = createPresetTreeNode(preset);
         defaultTn.Nodes.Add(tn);
         this.treeNodes.Add(preset, tn);
      }

      this.customPresetsTn = new Outliner.Controls.Tree.TreeNode("Custom Presets");
      this.customPresetsTn.IsExpanded = true;

      foreach (OutlinerPreset preset in OutlinerPresets.Presets.Where(p => !p.IsDefaultPreset))
      {
         Tree.TreeNode tn = createPresetTreeNode(preset);
         this.customPresetsTn.Nodes.Add(tn);
         this.treeNodes.Add(preset, tn);
      }

      presetsTree.Nodes.Add(defaultTn);
      presetsTree.Nodes.Add(this.customPresetsTn);
   }

   private Tree.TreeNode createPresetTreeNode(OutlinerPreset preset)
   {
      Tree.TreeNode tn = new Tree.TreeNode(preset.Name);
      tn.Tag = preset;

      Tree.TreeNode filterTn = new Tree.TreeNode("Filters");
      //filterTn.Tag = preset.Filters;
      filterTn.Tag = EditorType.Filters;
      tn.Nodes.Add(filterTn);

      Tree.TreeNode layoutTn = new Tree.TreeNode("Layout");
      //layoutTn.Tag = preset.TreeNodeLayout;
      layoutTn.Tag = EditorType.Layout;
      tn.Nodes.Add(layoutTn);
      
      return tn;
   }


   private void FillPresetProperties()
   {
      this.propertiesPanel.Controls.Clear();

      Tree.TreeNode tn = this.presetsTree.SelectedNodes.FirstOrDefault();
      Control editor = null;
      this.SuspendLayout();

      if (tn != null && tn.Tag != null)
      {
         Type tagType = tn.Tag.GetType();

         if (tagType.Equals(typeof(OutlinerPreset)) || tagType.IsSubclassOf(typeof(OutlinerPreset)))
            editor = new PresetPropertiesEditor(tn.Tag as OutlinerPreset, this.UpdatePreviewTree);
         else if (tagType.Equals(typeof(EditorType)))
         {
            EditorType editorType = (EditorType)tn.Tag;
            if (editorType == EditorType.Layout)
               editor = new TreeNodeLayoutEditor(this.editingPreset, this.UpdatePreviewTree);
            else if (editorType == EditorType.Filters)
               editor = new PresetFilterCollectionEditor(this.editingPreset.Filters, this.UpdatePreviewTree);
         }

         if (editor != null)
         {
            editor.Dock = DockStyle.Fill;
            this.propertiesPanel.Controls.Add(editor);
         }
         this.ResumeLayout();
      }
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
      this.editingPreset = null;
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

   private void UpdatePreviewTree()
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
            //tn.Nodes.Add(new Tree.TreeNode("Open a scene for a proper preview"));
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

   private void addBtn_Click(object sender, EventArgs e)
   {
      OutlinerPreset newPreset = new OutlinerPreset();
      newPreset.Name = "NewPreset";
      this.customPresetsTn.Nodes.Add(createPresetTreeNode(newPreset));
   }

   private void deleteBtn_Click(object sender, EventArgs e)
   {
      String action = this.editingPreset.IsDefaultPreset ? "Revert" : "Delete";
      String text = String.Format( "Are you sure you want to {0} the preset \"{1}\"?\nThis action cannot be undone."
                                 , action.ToLower()
                                 , this.editingPreset.Name);
      DialogResult result = MessageBox.Show(text, String.Format("{0} preset", action), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
      if (result == DialogResult.Yes)
      {
         OutlinerPresets.DeletePreset(this.editingPreset);
         this.FillPresetList();
         this.presetsTree.OnSelectionChanged();
      }
   }
}
}
