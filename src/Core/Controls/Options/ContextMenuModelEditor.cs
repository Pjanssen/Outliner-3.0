using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PJanssen.Outliner.Controls.ContextMenu;
using PJanssen.Outliner.Controls.Tree.Layout;
using System.IO;
using PJanssen.Outliner.Configuration;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Controls.Options
{
public partial class ContextMenuModelEditor : OutlinerUserControl
{
   private ContextMenuModel contextMenuModel;
   private Dictionary<MenuItemModel, Tree.TreeNode> treeNodes;

   public ContextMenuModelEditor()
   {
      InitializeComponent();
   }

   public ContextMenuModelEditor(ContextMenuModel contextMenu) : this()
   {
      this.contextMenuModel = contextMenu;
      this.treeNodes = new Dictionary<MenuItemModel, Tree.TreeNode>();
   }

   protected override void OnLoad(EventArgs e)
   {
      base.OnLoad(e);

      this.itemTree.TreeNodeLayout = new TreeNodeLayout();
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeIndent() { UseVisualStyles = false });
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.itemTree.TreeNodeLayout.FullRowSelect = true;
      this.itemTree.Settings.MultiSelect = false;

      this.FillItemComboBox();
      this.FillItemTree();
   }

   private static String GetMenuItemNameComboBox(Type modelType)
   {
      OutlinerPluginData plugin = OutlinerPlugins.GetPlugin(modelType);
      if (plugin != null)
         return plugin.DisplayName;

      return modelType.Name;
   }

   private static String GetMenuItemNameTree(Type modelType)
   {
      if (modelType == typeof(SeparatorMenuItemModel))
         return "-------------------------";
      else
         return GetMenuItemNameComboBox(modelType);
   }

   private void FillItemComboBox()
   {
      IEnumerable<OutlinerPluginData> plugins = OutlinerPlugins.GetPlugins(OutlinerPluginType.ContextMenuItemModel)
                                                               .OrderBy(p => p.DisplayName);

      this.itemsComboBox.DataSource = plugins.ToList();
      this.itemsComboBox.DisplayMember = "DisplayName";
      this.itemsComboBox.ValueMember = "Type";
   }

   private void FillItemTree()
   {
      if (this.contextMenuModel == null)
         return;

      this.itemTree.BeginUpdate();

      this.itemTree.Nodes.Clear();
      this.treeNodes.Clear();
      foreach (MenuItemModel item in this.contextMenuModel.Items)
      {
         AddItemToTree(item, this.itemTree.Nodes);
      }

      this.itemTree.EndUpdate();
   }

   private Tree.TreeNode AddItemToTree(MenuItemModel item, Tree.TreeNodeCollection parentCollection)
   {
      String text = item.Text;
      if (String.IsNullOrEmpty(text))
         text = GetMenuItemNameTree(item.GetType());
      Tree.TreeNode tn = new Tree.TreeNode(text);
      tn.Tag = item;

      if (!(item is IncludeContextMenuModel))
      {
         foreach (MenuItemModel subitem in item.SubItems)
         {
            AddItemToTree(subitem, tn.Nodes);
         }
      }
      parentCollection.Add(tn);
      this.treeNodes.Add(item, tn);

      return tn;
   }

   private void itemTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      Tree.TreeNode selTn = e.Nodes.FirstOrDefault();
      if (selTn != null)
         this.itemPropertyGrid.SelectedObject = selTn.Tag;
      else
         this.itemPropertyGrid.SelectedObject = null;
   }

   private MenuItemModel GetSelectedItem()
   {
      Tree.TreeNode selTn = this.itemTree.SelectedNodes.FirstOrDefault();
      if (selTn != null)
         return selTn.Tag as MenuItemModel;
      else
         return null;
   }

   private MenuItemModel GetSelectedItemParent()
   {
      Tree.TreeNode selTn = this.itemTree.SelectedNodes.FirstOrDefault();
      if (selTn == null || selTn.Parent == null)
         return null;

      return selTn.Parent.Tag as MenuItemModel;
   }

   private List<MenuItemModel> GetSelectedItemList()
   {
      MenuItemModel selItemParent = this.GetSelectedItemParent();

      if (selItemParent != null)
         return selItemParent.SubItems;
      else
         return this.contextMenuModel.Items;
   }

   private void SelectItem(MenuItemModel item)
   {
      Tree.TreeNode tn = null;
      if (this.treeNodes.TryGetValue(item, out tn))
      {
         tn.TreeView.SelectNode(tn, true);
         tn.TreeView.OnSelectionChanged();
      }
   }

   private void addBtn_Click(object sender, EventArgs e)
   {
      OutlinerPluginData selectedItem = this.itemsComboBox.SelectedItem as OutlinerPluginData;
      if (selectedItem == null)
         return;

      List<MenuItemModel> target = this.contextMenuModel.Items;
      MenuItemModel selItem = this.GetSelectedItem();
      if (selItem != null)
         target = selItem.SubItems;

      MenuItemModel newItem = Activator.CreateInstance(selectedItem.Type, null) as MenuItemModel;
      target.Add(newItem);

      this.FillItemTree();
      this.SelectItem(newItem);
   }

   private void deleteBtn_Click(object sender, EventArgs e)
   {
      MenuItemModel selectedItem = this.GetSelectedItem();
      MenuItemModel parentItem = this.GetSelectedItemParent();
      
      if (parentItem != null)
         parentItem.SubItems.Remove(selectedItem);
      else
         this.contextMenuModel.Items.Remove(selectedItem);

      this.FillItemTree();
   }

   private void moveUpBtn_Click(object sender, EventArgs e)
   {
      MenuItemModel selItem = this.GetSelectedItem();
      List<MenuItemModel> itemList = this.GetSelectedItemList();

      int index = itemList.IndexOf(selItem);
      if (index > 0)
      {
         itemList.Remove(selItem);
         itemList.Insert(index - 1, selItem);
      }

      this.FillItemTree();
      this.SelectItem(selItem);
   }

   private void moveDownBtn_Click(object sender, EventArgs e)
   {
      MenuItemModel selItem = this.GetSelectedItem();
      List<MenuItemModel> itemList = this.GetSelectedItemList();

      int index = itemList.IndexOf(selItem);
      if (index < itemList.Count - 1)
      {
         itemList.Remove(selItem);
         itemList.Insert(index + 1, selItem);
      }

      this.FillItemTree();
      this.SelectItem(selItem);
   }

   private void itemPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
   {
      MenuItemModel item = this.itemPropertyGrid.SelectedObject as MenuItemModel;
      Tree.TreeNode tn;
      if (this.treeNodes.TryGetValue(item, out tn))
         tn.Text = item.Text;
   }
}
}
