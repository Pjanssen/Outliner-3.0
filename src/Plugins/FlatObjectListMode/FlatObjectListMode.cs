using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Modes.FlatObjectList
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_FlatObjectList")]
[LocalizedDisplayImage(typeof(Resources), "flatobjectlist_mode_16_dark", "flatobjectlist_mode_24_dark")]
public class FlatObjectListMode : TreeMode
{
   public FlatObjectListMode(TreeView tree) : base(tree) { }

   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      IINode rootNode = MaxInterfaces.COREInterface.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
      {
         this.FillTree(rootNode.GetChildNode(i));
      }

      this.Tree.Sort();

      this.Tree.EndUpdate();
   }

   private void FillTree(IINode node)
   {
      this.AddNode(node, this.Tree.Nodes);
      for (int i = 0; i < node.NumberOfChildren; i++)
      {
         this.FillTree(node.GetChildNode(i));
      }
   }
}
}
