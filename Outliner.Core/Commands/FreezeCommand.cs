using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   public class FreezeCommand : SetNodePropertyCommand<Boolean>
   {
      public FreezeCommand(IEnumerable<IMaxNode> nodes, Boolean newValue)
         : base(nodes, NodeProperty.IsFrozen, newValue) { }

      public override string Description
      {
         get { return OutlinerResources.Command_Freeze; }
      }
   }
}
