using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.Filters;

namespace Outliner.TreeModes
{
public class FlatObjectListMode : TreeMode
{
   public FlatObjectListMode(TreeView tree, Autodesk.Max.IInterface ip) 
      : base(tree, ip) 
   {
      this.Filters = new FlatListFilterCollection<IMaxNodeWrapper>(base.Filters);
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         this.AddNode(rootNode.GetChildNode(i), this.tree.Nodes);

      this.tree.Sort();

      this.tree.EndUpdate();
   }

   public override TreeNode AddNode(Object node, TreeNodeCollection parentCol)
   {
      IINode inode = node as IINode;
      if (inode == null)
         return null;

      TreeNode tn = base.AddNode(node, parentCol);

      for (int i = 0; i < inode.NumberOfChildren; i++)
         AddNode(inode.GetChildNode(i), parentCol);

      return tn;
   }

   public override void RegisterNodeEventCallbacks()
   {
      this.RegisterNodeEventCallbackObject(new FlatListNodeEventCallbacks(this));
      base.RegisterNodeEventCallbacks();
   }

   protected class FlatListNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public FlatListNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            this.treeMode.AddNode(node, this.tree.Nodes);
         }
         this.tree.StartTimedSort(false);
      }
   }
}
}
