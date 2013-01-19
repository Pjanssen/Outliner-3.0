using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.ColorTags
{
public class SetColorTagCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private ColorTag tag;
   private Dictionary<IMaxNode, ColorTag> oldTags;

   public SetColorTagCommand(IEnumerable<IMaxNode> nodes, ColorTag tag)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes;
      this.tag = tag;
   }

   public override string Description
   {
      get { return Resources.Command_SetColorTag; }
   }

   protected override void Do()
   {
      this.oldTags = new Dictionary<IMaxNode, ColorTag>();
      foreach (IMaxNode node in this.nodes)
      {
         if (!oldTags.ContainsKey(node))
            oldTags.Add(node, node.GetColorTag());

         node.SetColorTag(this.tag);
      }
   }

   protected override void Undo()
   {
      if (this.oldTags == null)
         return;

      foreach (KeyValuePair<IMaxNode, ColorTag> node in this.oldTags)
      {
         node.Key.SetColorTag(node.Value);
      }
   }
}
}
