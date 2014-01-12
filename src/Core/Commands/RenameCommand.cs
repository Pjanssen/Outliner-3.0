using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Commands
{
/// <summary>
/// Renames the given nodes.
/// </summary>
public class RenameCommand : SetNodePropertyCommand<String>
{
   /// <summary>
   /// Initializes a new instance of the RenameCommand class.
   /// </summary>
   /// <param name="nodes">The IMaxNodes to rename.</param>
   /// <param name="newName">The new name for the given IMaxNodes.</param>
   public RenameCommand(IEnumerable<IMaxNode> nodes, String newName)
      : base(nodes, NodeProperty.Name, newName) { }

   public override string Description
   {
      get { return OutlinerResources.Command_Rename; }
   }
}
}
