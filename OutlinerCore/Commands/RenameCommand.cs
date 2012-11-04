using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class RenameCommand : SetNodePropertyCommand<String>
{
   public RenameCommand(IEnumerable<IMaxNodeWrapper> nodes, String newName)
      : base(nodes, NodeProperty.Name, newName) { }

   public override string Description
   {
      get { return OutlinerResources.Command_Rename; }
   }
}
}
