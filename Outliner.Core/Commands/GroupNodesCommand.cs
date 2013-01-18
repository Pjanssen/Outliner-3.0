using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;
using Autodesk.Max;

namespace Outliner.Commands
{
/// <summary>
/// Creates a new group and adds the given nodes to it.
/// </summary>
public class GroupNodesCommand : Command
{
   private IEnumerable<MaxNodeWrapper> nodes;
   private IINodeWrapper groupHead;
   private List<Tuple<IINodeWrapper, MaxNodeWrapper, Boolean>> previousParents;

   public GroupNodesCommand(IEnumerable<MaxNodeWrapper> nodes)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
   }

   public override string Description
   {
      get
      {
         return OutlinerResources.Command_Group;
      }
   }

   protected override void Do()
   {
      StorePreviousParents();

      this.groupHead = GroupHelpers.CreateGroupHead();
      this.groupHead.IINode.SetAFlag(AnimatableFlags.Held);
      GroupHelpers.AddNodesToGroup(this.nodes, this.groupHead);
   }

   private void StorePreviousParents()
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

      if (this.groupHead != null)
      {
         MaxInterfaces.COREInterface.DeleteNode(this.groupHead.IINode, false, false);
         this.groupHead = null;
      }
   }
}
}
