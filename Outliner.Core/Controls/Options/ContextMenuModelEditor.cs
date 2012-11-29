using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.ContextMenu;
using Outliner.Presets;
using Outliner.Controls.Tree.Layout;

namespace Outliner.Controls.Options
{
public partial class ContextMenuModelEditor : OutlinerUserControl
{
   private OutlinerPreset preset;
   private Action updateAction;
   private ContextMenuModel contextMenuModel;

   public ContextMenuModelEditor()
   {
      InitializeComponent();
   }

   public ContextMenuModelEditor(OutlinerPreset preset, Action updateAction)
      : this()
   {
      this.preset = preset;
      this.updateAction = updateAction;
      String path = System.IO.Path.Combine(OutlinerPaths.ContextMenusDir, preset.ContextMenuFile);
      if (System.IO.File.Exists(path))
         this.contextMenuModel = XmlSerializationHelpers.Deserialize<ContextMenuModel>(path);
   }

   protected override void OnLoad(EventArgs e)
   {
      base.OnLoad(e);

      this.itemTree.TreeNodeLayout = new TreeNodeLayout();
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeIndent());
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.itemTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.itemTree.TreeNodeLayout.FullRowSelect = true;

      this.FillItemComboBox();
      this.FillItemTree();
   }

   private String GetMenuItemName(Type modelType)
   {
      if (modelType == typeof(ActionMenuItemModel))
         return "Action item";
      if (modelType == typeof(MxsMenuItemModel))
         return "Maxscript item";
      if (modelType == typeof(NodePropertyMenuItemModel))
         return "Node Property item";
      if (modelType == typeof(SeparatorMenuItemModel))
         return "Separator";
      if (modelType == typeof(IncludeContextMenuModel))
         return "Include context-menu";
      return modelType.Name;
   }

   private void FillItemComboBox()
   {
      List<Type> itemTypes = new List<Type>() {
         typeof(ActionMenuItemModel),
         typeof(MxsMenuItemModel),
         typeof(NodePropertyMenuItemModel),
         typeof(SeparatorMenuItemModel),
         typeof(IncludeContextMenuModel),
      };

      this.itemsComboBox.DataSource = itemTypes.Select(t => new Tuple<Type, String>(t, GetMenuItemName(t)))
                                               .ToList();
      this.itemsComboBox.DisplayMember = "Item2";
      this.itemsComboBox.ValueMember = "Item1";
   }

   private void FillItemTree()
   {
      if (this.contextMenuModel == null)
         return;

      foreach (MenuItemModel item in this.contextMenuModel.Items)
      {
         AddItemToTree(item, this.itemTree.Nodes);
      }
   }

   private Tree.TreeNode AddItemToTree(MenuItemModel item, Tree.TreeNodeCollection parentCollection)
   {
      String text = item.Text;
      if (String.IsNullOrEmpty(text))
         text = String.Format("- {0} -", GetMenuItemName(item.GetType()));
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


}
}
