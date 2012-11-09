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

namespace Outliner.Controls.Options
{
public partial class TreeNodeLayoutEditor : UserControl
{
   private PresetEditor owningEditor;
   private TreeNodeLayout layout;

   public TreeNodeLayoutEditor()
   {
      InitializeComponent();
   }

   public TreeNodeLayoutEditor(PresetEditor owningEditor, TreeNodeLayout layout) : this()
   {
      this.owningEditor = owningEditor;
      this.layout = layout;

      Color windowColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.SetControlColor(this.layoutTree, windowColor, windowTextColor);
      this.SetControlColor(this.itemProperties, windowColor, windowTextColor);
      this.SetControlColor(this.layoutComboBox, windowColor, windowTextColor);

      this.itemProperties.ViewBackColor = windowColor;
      this.itemProperties.ViewForeColor = windowTextColor;
      this.itemProperties.LineColor = Color.Gray;
      
      this.layoutTree.TreeNodeLayout = new TreeNodeLayout();
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.layoutTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.layoutTree.TreeNodeLayout.FullRowSelect = true;

      this.SetControlColor(this.itemHeightSpinner, windowColor, windowTextColor);
      this.SetControlColor(this.paddingLeftSpinner, windowColor, windowTextColor);
      this.SetControlColor(this.paddingRightSpinner, windowColor, windowTextColor);
      
      this.fullRowSelectCheckBox.Checked = this.layout.FullRowSelect;
      this.itemHeightSpinner.Value = this.layout.ItemHeight;
      this.paddingLeftSpinner.Value = this.layout.PaddingLeft;
      this.paddingRightSpinner.Value = this.layout.PaddingRight;

      this.layoutBindingSource.DataSource = this.layout;

      this.FillItemComboBox();
      this.FillItemsTree();
   }

   private void SetControlColor(Control c, Color backColor, Color foreColor)
   {
      c.BackColor = backColor;
      c.ForeColor = foreColor;
   }

   private void FillItemComboBox()
   {
      IEnumerable<OutlinerPluginData> layoutItems = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeNodeButton);
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
      this.owningEditor.UpdatePreviewTree();
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
            this.owningEditor.UpdatePreviewTree();
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
         this.owningEditor.UpdatePreviewTree();
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
            this.owningEditor.UpdatePreviewTree();
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
            this.owningEditor.UpdatePreviewTree();
         }
      }
   }

   private void layoutBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
         this.owningEditor.UpdatePreviewTree();
   }
}
}
