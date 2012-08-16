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
      if (nodes == null)
         throw new ArgumentNullException("nodes");

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
         IAnimatable anim = node.WrappedNode as IAnimatable;
         if (anim != null)
         {
            if (!oldTags.ContainsKey(node))
               oldTags.Add(node, ColorTags.GetTag(anim));

            ColorTags.SetTag(anim, this.tag);
         }
      }
   }

   public override void Undo()
   {
      if (this.oldTags == null)
         return;

      foreach (KeyValuePair<IMaxNodeWrapper, ColorTag> node in this.oldTags)
      {
         IAnimatable anim = node.Key.WrappedNode as IAnimatable;
         if (anim != null)
         {
            ColorTags.SetTag(anim, node.Value);
         }
      }
   }
}
}
