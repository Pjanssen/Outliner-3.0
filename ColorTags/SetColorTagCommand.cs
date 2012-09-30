using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Commands;
using MaxUtils;

namespace Outliner.ColorTags
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
      get { return Resources.Command_Description; }
   }

   protected override void Do()
   {
      this.oldTags = new Dictionary<IMaxNodeWrapper, ColorTag>();
      foreach (IMaxNodeWrapper node in this.nodes)
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

      foreach (KeyValuePair<IMaxNodeWrapper, ColorTag> node in this.oldTags)
      {
         node.Key.SetColorTag(node.Value);
      }
   }
}
}
