using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class ChangeGroupCommand : CustomRestoreObjCommand
{
   private IEnumerable<INodeWrapper> nodes;
   private IMaxNode groupHead;
   private Boolean group;

   public ChangeGroupCommand( IEnumerable<IMaxNode> nodes
                            , IMaxNode groupHead
                            , Boolean group)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");
      Throw.IfArgumentIsNull(groupHead, "groupHead");

      this.nodes = nodes.Where(n => n is INodeWrapper)
                        .Cast<INodeWrapper>()
                        .ToList();
      this.groupHead = groupHead;
      this.group = group;
   }

   public override string Description
   {
      get
      {
         if (this.group)
            return OutlinerResources.Command_Group;
         else
            return OutlinerResources.Command_Ungroup;
      }
   }

   public override void Redo()
   {
      foreach (INodeWrapper node in this.nodes)
      {
         node.Parent = this.groupHead;
         node.INode.SetGroupMember(this.group);
      }
   }

   public override void Restore(bool isUndo)
   {
      foreach (INodeWrapper node in this.nodes)
      {
         node.INode.SetGroupMember(!this.group);
      }
   }
}
}
