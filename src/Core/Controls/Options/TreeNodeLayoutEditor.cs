using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.MaxUtils;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Controls.Options
{
public partial class TreeNodeLayoutEditor : OutlinerUserControl
{
   private TreeNodeLayout layout;

   public TreeNodeLayoutEditor()
   {
      InitializeComponent();
   }

   public TreeNodeLayoutEditor(TreeNodeLayout layout) : this()
   {
      this.layout = layout;
   }

   public Action UpdateTreeAction { get; set; }

   protected override void OnLoad(EventArgs e)
   {
      this.layoutTree.TreeNodeLayout = new TreeNodeLayout();
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.layoutTree.TreeNodeLayout.FullRowSelect = true;
      this.layoutTree.Settings.MultiSelect = false;

      this.fullRowSelectCheckBox.Checked = this.layout.FullRowSelect;
      this.itemHeightSpinner.Value = this.layout.ItemHeight;
      this.paddingLeftSpinner.Value = this.layout.PaddingLeft;
      this.paddingRightSpinner.Value = this.layout.PaddingRight;

      this.FillItemComboBox();
      this.FillItemsTree();

      this.layoutBindingSource.DataSource = this.layout;

      base.OnLoad(e);
   }

   private void FillItemComboBox()
   {
      IEnumerable<OutlinerPluginData> layoutItems = OutlinerPlugins.GetPlugins(OutlinerPluginType.TreeNodeButton);
      foreach (OutlinerPluginData layoutItem in layoutItems.OrderBy(i => i.DisplayName))
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
         Tree.TreeNode tn = new Tree.TreeNode(GetItemName(item));
         tn.Tag = item;
         this.layoutTree.Nodes.Add(tn);
      }
   }

   private static String GetItemName(TreeNodeLayoutItem item)
   {
      OutlinerPluginData plugin = OutlinerPlugins.GetPlugin(item.GetType());
      return plugin.DisplayName;
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
      if (this.UpdateTreeAction != null)
         this.UpdateTreeAction();
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
            if (this.UpdateTreeAction != null)
               this.UpdateTreeAction();
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
         if (this.UpdateTreeAction != null)
            this.UpdateTreeAction();
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
            if (this.UpdateTreeAction != null)
               this.UpdateTreeAction();
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
            if (this.UpdateTreeAction != null)
               this.UpdateTreeAction();
         }
      }
   }

   private void layoutBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.UpdateTreeAction != null)
            this.UpdateTreeAction();
      }
   }
}
}
