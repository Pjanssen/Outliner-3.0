using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.Filters;
using MaxUtils;

namespace Outliner.Modes.FlatList
{
public class FlatObjectListMode : TreeMode
{
   public FlatObjectListMode(TreeView tree) : base(tree) 
   {
      this.RegisterNodeEventCallbacks();
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = MaxInterfaces.COREInterface.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         this.AddNode(rootNode.GetChildNode(i), this.tree.Nodes);

      this.tree.Sort();

      this.tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      wrapper.ChildNodes.ForEach(node => this.AddNode(node, parentCol));

      return tn;
   }


   private void RegisterNodeEventCallbacks()
   {
      this.RegisterNodeEventCallbackObject(new FlatListNodeEventCallbacks(this));
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
