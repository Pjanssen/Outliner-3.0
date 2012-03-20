using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Commands
{
public class SetBoxModeCommand : SetNodePropertyCommand<Boolean>
{
   public SetBoxModeCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean boxMode) 
      : base(nodes, boxMode) { }
   
   public override string Description
   {
      get { return OutlinerResources.Command_SetBoxMode; }
   }

   public override bool GetValue(IMaxNodeWrapper node)
   {
      if (node == null)
         return false;
      else
         return node.BoxMode;
   }

   public override void SetValue(IMaxNodeWrapper node, bool value)
   {
      if (node == null)
         return;
      else
         node.BoxMode = value;
   }
}
}
