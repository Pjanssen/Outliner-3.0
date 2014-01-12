using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Commands
{
/// <summary>
/// Deletes the given IMaxNodes from the scene.
/// </summary>
public class DeleteCommand : Command
{
   private IEnumerable<IMaxNode> nodes;

   /// <summary>
   /// Initializes a new instance of the DeleteCommand class.
   /// </summary>
   /// <param name="nodes">The IMaxNodes to delete.</param>
   public DeleteCommand(IEnumerable<IMaxNode> nodes)
   {
      Throw.IfNull(nodes, "nodes");

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
