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
public class CreateNewGroupCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private INodeWrapper groupHead;

   public CreateNewGroupCommand(IEnumerable<IMaxNode> nodes)
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

   public override void Do()
   {
      this.groupHead = GroupHelpers.CreateGroupHead();
      ChangeGroupCommand changeGroupCmd = new ChangeGroupCommand(this.nodes, this.groupHead, true);
      changeGroupCmd.Execute(false);
   }
}
}
