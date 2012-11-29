using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.Tree.Layout;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Plugins;
using Outliner.Presets;

namespace Outliner.Controls.Options
{
public partial class TreeNodeLayoutEditor : OutlinerUserControl
{
   private OutlinerPreset preset;
   private TreeNodeLayout layout;
   private Action updateAction;

   public TreeNodeLayoutEditor()
   {
      InitializeComponent();
   }

   public TreeNodeLayoutEditor(OutlinerPreset preset, Action updateAction) : this()
   {
      this.preset = preset;
      this.layout = preset.TreeNodeLayout;
      this.updateAction = updateAction;
      
      this.layoutTree.TreeNodeLayout = new TreeNodeLayout();
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.layoutTree.TreeNodeLayout.FullRowSelect = true;

      this.fullRowSelectCheckBox.Checked = this.layout.FullRowSelect;
      this.itemHeightSpinner.Value = this.layout.ItemHeight;
      this.paddingLeftSpinner.Value = this.layout.PaddingLeft;
      this.paddingRightSpinner.Value = this.layout.PaddingRight;

      this.FillItemComboBox();
      this.FillItemsTree();
      this.FillFileComboBox();

      this.layoutBindingSource.DataSource = this.layout;
      this.presetBindingSource.DataSource = this.preset;
   }

   private void FillItemComboBox()
   {
      IEnumerable<OutlinerPluginData> layoutItems = OutlinerPlugins.GetPlugins(OutlinerPluginType.TreeNodeButton);
      foreach (OutlinerPluginData layoutItem in layoutItems)
      {
         this.layoutComboBox.Items.Add(layoutItem);
      }
      this.layoutComboBox.DisplayMember = "DisplayName";
      this.layoutComboBox.SelectedIndex = 0;
   }

   private void FillItemsTree()
   {
      this.layoutTree.Nodes.Clear();
      foreach (TreeNodeLayoutItem item in this.layout.LayoutItems)
      {
         Tree.TreeNode tn = new Tree.TreeNode(item.GetType().Name);
         tn.Tag = item;
         this.layoutTree.Nodes.Add(tn);
      }
   }

   private void FillFileComboBox()
   {
      System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(OutlinerPaths.LayoutsDir);
      List<System.IO.FileInfo> files = dirInfo.GetFiles("*.xml").ToList();
      layoutFileComboBox.DataSource = files;
      layoutFileComboBox.ValueMember = "Name";
      layoutFileComboBox.DisplayMember = "Name";
      
   }

   private void layoutTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      Tree.TreeNode tn = e.Nodes.FirstOrDefault();
      if (tn != null)
         this.itemProperties.SelectedObject = tn.Tag;
      else
         this.itemProperties.SelectedObject = null;
   }

   private void itemProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
   {
      if (this.updateAction != null)
         this.updateAction();
   }

   private void addButton_Click(object sender, EventArgs e)
   {
      OutlinerPluginData selItem = this.layoutComboBox.SelectedItem as OutlinerPluginData;
      if (selItem != null)
      {
         TreeNodeLayoutItem newItem = Activator.CreateInstance(selItem.Type, null) as TreeNodeLayoutItem;
         if (newItem != null)
         {
            this.layout.LayoutItems.Add(newItem);
            this.FillItemsTree();
            if (this.updateAction != null)
               this.updateAction();
         }
      }
   }

   private TreeNodeLayoutItem GetSelectedLayoutItem()
   {
      Tree.TreeNode tn = this.layoutTree.SelectedNodes.FirstOrDefault();
      if (tn != null)
         return tn.Tag as TreeNodeLayoutItem;
      else
         return null;
   }

   private void deleteButton_Click(object sender, EventArgs e)
   {
      TreeNodeLayoutItem item = this.GetSelectedLayoutItem();
      if (item != null)
      {
         this.layout.LayoutItems.Remove(item);
         this.FillItemsTree();
         this.itemProperties.SelectedObject = null;
         if (this.updateAction != null)
            this.updateAction();
      }
   }

   private void upButton_Click(object sender, EventArgs e)
   {
      TreeNodeLayoutItem item = this.GetSelectedLayoutItem();
      if (item != null)
      {
         Int32 index = this.layout.LayoutItems.IndexOf(item);
         if (index > 0)
         {
            this.layout.LayoutItems.Remove(item);
            this.layout.LayoutItems.Insert(index - 1, item);
            this.FillItemsTree();
            this.layoutTree.SelectNode(this.layoutTree.Nodes.Where(tn => tn.Tag == item).First(), true);
            if (this.updateAction != null)
               this.updateAction();
         }
      }
   }

   private void downButton_Click(object sender, EventArgs e)
   {
      TreeNodeLayoutItem item = this.GetSelectedLayoutItem();
      if (item != null)
      {
         Int32 index = this.layout.LayoutItems.IndexOf(item);
         if (index < this.layout.LayoutItems.Count - 1)
         {
            this.layout.LayoutItems.Remove(item);
            this.layout.LayoutItems.Insert(index + 1, item);
            this.FillItemsTree();
            this.layoutTree.SelectNode(this.layoutTree.Nodes.Where(tn => tn.Tag == item).First(), true);
            if (this.updateAction != null)
               this.updateAction();
         }
      }
   }

   private void layoutBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();
      }
   }

   private void newLayoutFileBtn_Click(object sender, EventArgs e)
   {
      saveFileDialog.InitialDirectory = OutlinerPaths.LayoutsDir;
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
         String file = saveFileDialog.FileName;
         Uri layoutDirUri = new Uri(OutlinerPaths.LayoutsDir);
         Uri fileUri = layoutDirUri.MakeRelativeUri(new Uri(saveFileDialog.FileName));

         TreeNodeLayout newLayout = null;
         if (MessageBox.Show("Copy current layout to new file?", "Use current layout?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            newLayout = new TreeNodeLayout(this.layout);
         else
            newLayout = new TreeNodeLayout();

         XmlSerializationHelpers.Serialize<TreeNodeLayout>(file, newLayout);

         this.presetBindingSource.SuspendBinding();
         this.layoutBindingSource.SuspendBinding();

         this.preset.LayoutFile = Uri.UnescapeDataString(fileUri.ToString());
         this.layout = this.preset.TreeNodeLayout;
         this.FillItemsTree();
         this.FillFileComboBox();

         this.layoutBindingSource.DataSource = this.layout;
         this.presetBindingSource.ResumeBinding();
         this.layoutBindingSource.ResumeBinding();
      }
   }

   private void presetBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();

         this.layout = this.preset.TreeNodeLayout;
         this.layoutBindingSource.DataSource = this.layout;
         this.FillItemsTree();
      }
   }
}
}
