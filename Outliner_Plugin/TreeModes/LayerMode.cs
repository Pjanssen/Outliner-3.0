using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.FiltersBase;

namespace Outliner.TreeModes
{
public class LayerMode : TreeMode
{
   public LayerMode(Outliner.Controls.TreeView tree, Autodesk.Max.IInterface ip)
      : base(tree, ip)
   { }

   private const int IILAYERMANAGER_REF_INDEX = 10;

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
      {
         addNode(rootNode.GetChildNode(i));
      }

      this.tree.EndUpdate();
      this.tree.TimedSort(false);
   }

   private TreeNode addNode(IILayer layer, TreeNodeCollection parentCol)
   {
      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(layer);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(layer))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         ((OutlinerTreeNodeData)tn.Tag).FilterResult = filterResult;

         this.nodes.Add(layer, tn);
         parentCol.Add(tn);
         return tn;
      }
      return null;
   }

   private void addNode(IINode node)
   {
      if (HelperMethods.IsPFHelper(node))
         return;

      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(node);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(node))
      {
         //Add layer node if it doesn't exist yet.
         IILayer l = (IILayer)node.GetReference((int)ReferenceNumbers.NodeLayerRef);
         TreeNode parentTn = null;
         if (!this.nodes.TryGetValue(l, out parentTn))
            parentTn = this.addNode(l, this.tree.Nodes);

         if (parentTn != null)
         {
            TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
            ((OutlinerTreeNodeData)tn.Tag).FilterResult = filterResult;

            this.nodes.Add(node, tn);
            parentTn.Nodes.Add(tn);

            if (node.Selected)
               this.tree.SelectNode(tn, true);

            for (int i = 0; i < node.NumberOfChildren; i++)
               this.addNode(node.GetChildNode(i));
         }
      }
   }
}
}
