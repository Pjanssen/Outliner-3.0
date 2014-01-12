using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.MaxUtils;
using Autodesk.Max;

namespace PJanssen.Outliner.Commands
{
/// <summary>
/// Creates a new group and adds the given nodes to it.
/// </summary>
public class CreateNewGroupCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private INodeWrapper groupHead;

   /// <summary>
   /// Initializes a new instance of the CreateNewGroupCommand.
   /// </summary>
   /// <param name="nodes">The nodes to add to the created group.</param>
   public CreateNewGroupCommand(IEnumerable<IMaxNode> nodes)
   {
      Throw.IfNull(nodes, "nodes");

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
