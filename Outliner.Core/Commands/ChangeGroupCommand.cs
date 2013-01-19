using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class ChangeGroupCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private IMaxNode groupHead;
   private Boolean group;
   private List<Tuple<INodeWrapper, IMaxNode, Boolean>> previousParents;

   public ChangeGroupCommand( IEnumerable<IMaxNode> nodes
                            , IMaxNode groupHead, Boolean group)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");
      Throw.IfArgumentIsNull(groupHead, "groupHead");

      this.nodes = nodes.ToList();
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

   protected override void Do()
   {
      this.previousParents = new List<Tuple<INodeWrapper, IMaxNode, Boolean>>();
      foreach (IMaxNode node in this.nodes)
      {
         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper != null)
         {
            this.previousParents.Add(new Tuple<INodeWrapper, IMaxNode, Boolean>(
               inodeWrapper,
               node.Parent,
               inodeWrapper.INode.IsGroupMember));
         }
      }

      if (this.groupHead != null)
         this.groupHead.AddChildNodes(this.nodes);
      else
         this.nodes.ForEach(n => n.Parent.RemoveChildNode(n));

      this.nodes.Where(n => n is INodeWrapper)
                .Cast<INodeWrapper>()
                .ForEach(n => n.INode.SetGroupMember(this.group));
   }

   protected override void Undo()
   {
      if (this.previousParents == null)
         return;

      foreach (Tuple<INodeWrapper, IMaxNode, Boolean> prevParent in this.previousParents)
      {
         prevParent.Item2.AddChildNode(prevParent.Item1);
         prevParent.Item1.INode.SetGroupMember(prevParent.Item3);
      }
   }
}
}
