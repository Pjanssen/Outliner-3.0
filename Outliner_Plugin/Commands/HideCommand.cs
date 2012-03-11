using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Commands
{
public class HideCommand : SetNodePropertyCommand<Boolean>
{
   public HideCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean newValue)
      : base(nodes, newValue) { }

   public override string Description
   {
      get { return OutlinerResources.Command_Hide; }
   }

   public override bool GetValue(IMaxNodeWrapper node)
   {
      return node.IsHidden;
   }

   public override void SetValue(IMaxNodeWrapper node, bool value)
   {
      node.IsHidden = value;
   }
}
}
