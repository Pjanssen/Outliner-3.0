using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.Filters;
using MaxUtils;
using Outliner.Plugins;

namespace Outliner.Modes.FlatObjectList
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_DisplayName")]
[LocalizedDisplayImage(typeof(Resources), "flatobjectlist_mode_16_dark", "flatobjectlist_mode_24_dark")]
public class FlatObjectListMode : TreeMode
{
   public FlatObjectListMode(TreeView tree) : base(tree) { }

   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      IINode rootNode = MaxInterfaces.COREInterface.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         this.AddNode(rootNode.GetChildNode(i), this.Tree.Nodes);

      this.Tree.Sort();

      this.Tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol)
   {
      return this.AddNode(wrapper, parentCol, true);
   }

   public TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol, Boolean recursive)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      if (recursive)
         wrapper.ChildNodes.ForEach(node => this.AddNode(node, parentCol));

      return tn;
   }


   public override void Start()
   {
      this.RegisterNodeEventCallbackObject(new FlatListNodeEventCallbacks(this));
      base.Start();
   }


   protected class FlatListNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      private FlatObjectListMode flatListMode;
      public FlatListNodeEventCallbacks(FlatObjectListMode treeMode) : base(treeMode)
      {
         flatListMode = treeMode;
      }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            this.flatListMode.AddNode(IMaxNodeWrapper.Create(node), this.tree.Nodes, false);
         }
         this.tree.StartTimedSort(false);
      }
   }
}
}
