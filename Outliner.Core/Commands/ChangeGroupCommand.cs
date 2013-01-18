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
   private IEnumerable<MaxNodeWrapper> nodes;
   private MaxNodeWrapper groupHead;
   private Boolean group;
   private List<Tuple<IINodeWrapper, MaxNodeWrapper, Boolean>> previousParents;

   public ChangeGroupCommand( IEnumerable<MaxNodeWrapper> nodes
                            , MaxNodeWrapper groupHead, Boolean group)
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
      this.previousParents = new List<Tuple<IINodeWrapper, MaxNodeWrapper, Boolean>>();
      foreach (MaxNodeWrapper node in this.nodes)
      {
         IINodeWrapper iinodeWrapper = node as IINodeWrapper;
         if (iinodeWrapper != null)
         {
            this.previousParents.Add(new Tuple<IINodeWrapper, MaxNodeWrapper, Boolean>(
               iinodeWrapper,
               node.Parent,
               iinodeWrapper.IINode.IsGroupMember));
         }
      }

      if (this.groupHead != null)
         this.groupHead.AddChildNodes(this.nodes);
      else
         this.nodes.ForEach(n => n.Parent.RemoveChildNode(n));

      this.nodes.Where(n => n is IINodeWrapper)
                .Cast<IINodeWrapper>()
                .ForEach(n => n.IINode.SetGroupMember(this.group));
   }

   protected override void Undo()
   {
      if (this.previousParents == null)
         return;

      foreach (Tuple<IINodeWrapper, MaxNodeWrapper, Boolean> prevParent in this.previousParents)
      {
         prevParent.Item2.AddChildNode(prevParent.Item1);
         prevParent.Item1.IINode.SetGroupMember(prevParent.Item3);
      }
   }
}
}
