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
      if (node == null)
         return false;
      else
         return node.IsFrozen;
   }

   public override void SetValue(IMaxNodeWrapper node, bool value)
   {
      if (node == null)
         return;
      else
         node.IsFrozen = value;
   }
}
}
