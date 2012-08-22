using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
public class ChangeGroupCommand : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private IMaxNodeWrapper groupHead;
   private Boolean group;
   private List<Tuple<IINodeWrapper, IMaxNodeWrapper, Boolean>> previousParents;

   public ChangeGroupCommand(IEnumerable<IMaxNodeWrapper> nodes, IMaxNodeWrapper groupHead, Boolean group)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(nodes, "nodes");
      ExceptionHelpers.ThrowIfArgumentIsNull(groupHead, "groupHead");

      this.groupHead = groupHead;
      this.nodes = nodes;
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

   public override void Do()
   {
      this.previousParents = new List<Tuple<IINodeWrapper, IMaxNodeWrapper, Boolean>>();
      foreach (IMaxNodeWrapper node in this.nodes)
      {
         IINodeWrapper iinodeWrapper = node as IINodeWrapper;
         if (iinodeWrapper != null)
         {
            this.previousParents.Add(new Tuple<IINodeWrapper, IMaxNodeWrapper, Boolean>(
               iinodeWrapper,
               node.Parent,
               iinodeWrapper.IINode.IsGroupMember));
         }
      }

      if (this.groupHead != null)
         this.groupHead.AddChildNodes(nodes);
      else
         this.nodes.ForEach(n => n.Parent.RemoveChildNode(n));

      this.nodes.Where(n => n is IINodeWrapper)
                .Cast<IINodeWrapper>()
                .ForEach(n => n.IINode.SetGroupMember(this.group));
   }

   public override void Undo()
   {
      if (this.previousParents == null)
         return;

      foreach (Tuple<IINodeWrapper, IMaxNodeWrapper, Boolean> prevParent in this.previousParents)
      {
         prevParent.Item2.AddChildNode(prevParent.Item1);
         prevParent.Item1.IINode.SetGroupMember(prevParent.Item3);
      }
   }
}
}
