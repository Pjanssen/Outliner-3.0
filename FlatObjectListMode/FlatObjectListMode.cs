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
[LocalizedDisplayImage(typeof(Resources), "flatobjectlist_mode_16_dark", "flatobjectlist_mode_32_dark")]
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
      TreeNode tn = base.AddNode(wrapper, parentCol);

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
