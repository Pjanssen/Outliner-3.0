using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;

namespace Outliner.Commands
{
public class SetColorTagCommand : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private ColorTag tag;
   private Dictionary<IMaxNodeWrapper, ColorTag> oldTags;

   public SetColorTagCommand(IEnumerable<IMaxNodeWrapper> nodes, ColorTag tag)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes;
      this.tag = tag;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetColorTag; }
   }

   public override void Do()
   {
      this.oldTags = new Dictionary<IMaxNodeWrapper, ColorTag>();
      foreach (IMaxNodeWrapper node in this.nodes)
      {
         if (!oldTags.ContainsKey(node))
            oldTags.Add(node, node.ColorTag);

         node.ColorTag = this.tag;
      }
   }

   public override void Undo()
   {
      if (this.oldTags == null)
         return;

      foreach (KeyValuePair<IMaxNodeWrapper, ColorTag> node in this.oldTags)
      {
         node.Key.ColorTag = node.Value;
      }
   }
}
}
