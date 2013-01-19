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
   private IEnumerable<IMaxNode> nodes;
   private INodeWrapper groupHead;
   private List<Tuple<INodeWrapper, IMaxNode, Boolean>> previousParents;

   public GroupNodesCommand(IEnumerable<IMaxNode> nodes)
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
      this.groupHead.INode.SetAFlag(AnimatableFlags.Held);
      GroupHelpers.AddNodesToGroup(this.nodes, this.groupHead);
   }

   private void StorePreviousParents()
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

      if (this.groupHead != null)
      {
         MaxInterfaces.COREInterface.DeleteNode(this.groupHead.INode, false, false);
         this.groupHead = null;
      }
   }
}
}
