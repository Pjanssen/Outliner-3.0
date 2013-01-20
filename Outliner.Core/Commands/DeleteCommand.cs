using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class DeleteCommand : Command
{
   private IEnumerable<IMaxNode> nodes;

   public DeleteCommand(IEnumerable<IMaxNode> nodes)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.Where(n => n.CanDelete)
                        .ToList();
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Delete; }
   }

   public override void Do()
   {
      this.nodes.ForEach(n => n.Delete());
   }
}
}
