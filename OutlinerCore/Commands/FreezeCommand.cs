using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using MaxUtils;

namespace Outliner.Commands
{
   public class FreezeCommand : SetNodePropertyCommand<Boolean>
   {
      public FreezeCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean newValue)
         : base(nodes, AnimatableProperty.IsFrozen, newValue) { }

      public override string Description
      {
         get { return OutlinerResources.Command_Freeze; }
      }
   }
}
