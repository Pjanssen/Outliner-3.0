using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Commands
{
public class FreezeCommand : SetNodePropertyCommand<Boolean>
{
   public FreezeCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean freeze)
      : base (nodes, freeze) { }

   public override string Description
   {
      get { return OutlinerResources.Command_Freeze; }
   }

   public override bool GetValue(IMaxNodeWrapper node)
   {
      return node.IsFrozen;
   }

   public override void SetValue(IMaxNodeWrapper node, bool value)
   {
      node.IsFrozen = value;
   }
}
}
