using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Configuration;
using Outliner.Plugins;
using Outliner.Controls.Tree.Layout;
using Outliner.NodeSorters;
using PJanssen;

namespace Outliner.Controls.Options
{
public partial class SorterConfigurationEditor : OutlinerUserControl
{
   private ISorterConfiguration sorterConfiguration;
   private Dictionary<NodeSorter, Tree.TreeNode> treeNodes;

   public SorterConfigurationEditor()
   {
      InitializeComponent();
   }

   public SorterConfigurationEditor(ISorterConfiguration config) : this() 
   {
      Throw.IfNull(config, "config");

      this.sorterConfiguration = config;
   }

   protected override void OnLoad(EventArgs e)
   {
      this.sortersTree.TreeNodeLayout = new TreeNodeLayout();
      this.sortersTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.sortersTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.sortersTree.TreeNodeLayout.FullRowSelect = true;

      this.FillSorterTypesComboBox();
      this.FillSorterTree();

      base.OnLoad(e);
   }

   private void FillSorterTypesComboBox()
   {
      IEnumerable<OutlinerPluginData> filters = OutlinerPlugins.GetPlugins(OutlinerPluginType.NodeSorter);
      foreach (OutlinerPluginData filter in filters)
      {
         this.sorterTypesComboBox.Items.Add(filter);
      }
      this.sorterTypesComboBox.DisplayMember = "DisplayName";
      this.sorterTypesComboBox.SelectedIndex = 0;
   }

   private void FillSorterTree()
   {
      this.treeNodes = new Dictionary<NodeSorter, Tree.TreeNode>();
      this.sortersTree.Nodes.Clear();
      this.AddSorterToTree(this.sorterConfiguration.Sorter, sortersTree.Nodes);
   }

   private void AddSorterToTree(NodeSorter sorter, Tree.TreeNodeCollection parentCollection)
   {
      if (sorter == null)
         return;

      Tree.TreeNode tn = new Tree.TreeNode(this.GetSorterName(sorter));
      tn.Tag = sorter;
      tn.IsExpanded = true;

      this.treeNodes.Add(sorter, tn);

      parentCollection.Add(tn);
      this.AddSorterToTree(sorter.SecondarySorter, parentCollection);
   }


   private String GetSorterName(NodeSorter sorter)
   {
      String displayName = OutlinerPlugins.GetPlugin(sorter.GetType()).DisplayName;
      if (sorter is NodePropertySorter)
         displayName += " (" + ((NodePropertySorter)sorter).Property + ")";
      return displayName;
   }

   private void SelectSorter(NodeSorter sorter)
   {
      Tree.TreeNode tn = null;
      if (this.treeNodes.TryGetValue(sorter, out tn))
      {
         this.sortersTree.SelectNode(tn, true);
         this.sortersTree.OnSelectionChanged();
      }
   }

   private NodeSorter GetParent(NodeSorter sorter)
   {
      if (sorter == null)
         return null;

      NodeSorter parent = this.sorterConfiguration.Sorter;
      while (parent != null && parent.SecondarySorter != sorter)
      {
         parent = parent.SecondarySorter;
      }

      if (parent == null || parent.SecondarySorter != sorter)
         return null;
      else
         return parent;
   }


   private void sortersTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      Tree.TreeNode selTn = e.Nodes.FirstOrDefault();
      if (selTn == null)
         this.sorterPropertyGrid.SelectedObject = null;
      else
         this.sorterPropertyGrid.SelectedObject = selTn.Tag;

      NodeSorter sorter = selTn == null ? null : selTn.Tag as NodeSorter;
      this.deleteButton.Enabled = sorter != null;
      this.upButton.Enabled = sorter != null && this.sorterConfiguration.Sorter != sorter;
      this.downButton.Enabled = sorter != null && sorter.SecondarySorter != null;
   }

   private void sorterPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
   {
      Tree.TreeNode tn = this.sortersTree.SelectedNodes.FirstOrDefault();
      if (tn == null)
         return;

      NodeSorter sorter = tn.Tag as NodeSorter;
      if (sorter != null)
         tn.Text = this.GetSorterName(sorter);
   }



   private void addButton_Click(object sender, EventArgs e)
   {
      OutlinerPluginData selPlugin = this.sorterTypesComboBox.SelectedItem as OutlinerPluginData;
 
      NodeSorter newSorter = Activator.CreateInstance(selPlugin.Type, null) as NodeSorter;
      NodeSorter lastSorter = this.sorterConfiguration.Sorter;
      while (lastSorter != null && lastSorter.SecondarySorter != null)
      {
         lastSorter = lastSorter.SecondarySorter;
      }
      if (lastSorter == null)
         this.sorterConfiguration.Sorter = newSorter;
      else
         lastSorter.SecondarySorter = newSorter;
         
      this.FillSorterTree();
      this.SelectSorter(newSorter);
   }

   private void deleteButton_Click(object sender, EventArgs e)
   {
      Tree.TreeNode selTn = this.sortersTree.SelectedNodes.FirstOrDefault();
      if (selTn != null)
      {
         NodeSorter sorter = selTn.Tag as NodeSorter;
         if (sorter == null)
            return;

         if (selTn.PreviousVisibleNode != null)
         {
            NodeSorter prevSorter = selTn.PreviousVisibleNode.Tag as NodeSorter;
            if (prevSorter != null)
               prevSorter.SecondarySorter = sorter.SecondarySorter;
         }
         else
         {
            this.sorterConfiguration.Sorter = sorter.SecondarySorter;
         }
      }

      this.FillSorterTree();
      this.sorterPropertyGrid.SelectedObject = null;
   }

   private void upButton_Click(object sender, EventArgs e)
   {
      Tree.TreeNode selTn = this.sortersTree.SelectedNodes.FirstOrDefault();
      if (selTn == null)
         return;

      NodeSorter sorter = selTn.Tag as NodeSorter;
      this.MoveUp(sorter);
      this.FillSorterTree();
      this.SelectSorter(sorter);
   }

   private void downButton_Click(object sender, EventArgs e)
   {
      Tree.TreeNode selTn = this.sortersTree.SelectedNodes.FirstOrDefault();
      if (selTn == null)
         return;

      NodeSorter sorter = selTn.Tag as NodeSorter;
      this.MoveDown(sorter);
      this.FillSorterTree();
      this.SelectSorter(sorter);
   }



   private void MoveUp(NodeSorter sorter)
   {
      if (this.sorterConfiguration.Sorter == null || this.sorterConfiguration.Sorter == sorter)
         return;

      NodeSorter parent = this.GetParent(sorter);
      this.MoveDown(parent);
   }

   private void MoveDown(NodeSorter sorter)
   {
      if (sorter == null || sorter.SecondarySorter == null)
         return;

      NodeSorter parent = this.GetParent(sorter);
      NodeSorter child = sorter.SecondarySorter;
      if (parent == null)
      {
         this.sorterConfiguration.Sorter = child;
      }
      else
         parent.SecondarySorter = child;

      sorter.SecondarySorter = child.SecondarySorter;
      child.SecondarySorter = sorter;
   }
}
}
