using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
public class SetRenderableCommand : SetNodePropertyCommand<Boolean>
{
   public SetRenderableCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean renderable)
      : base(nodes, renderable) { }

   public override string Description
   {
      get { return OutlinerResources.Command_SetRenderable; }
   }

   public override bool GetValue(IMaxNodeWrapper node)
   {
      if (node == null)
         return false;
      else
         return node.Renderable;
   }

   public override void SetValue(IMaxNodeWrapper node, bool value)
   {
      if (node == null)
         return;
      else
         node.Renderable = value;
   }
}
}
