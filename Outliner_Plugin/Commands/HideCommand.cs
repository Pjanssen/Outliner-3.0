using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Commands
{
public class HideCommand : Command
{
   protected List<IINode> nodes;
   protected Boolean hide;
   protected Dictionary<IINode, Boolean> prevHiddenStates;

   public HideCommand(List<IINode> nodes, Boolean hide)
   {
      this.nodes = nodes;
      this.hide = hide;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Hide; }
   }

   public override void Do()
   {
      if (this.nodes == null)
         return;

      this.prevHiddenStates = new Dictionary<IINode, Boolean>(this.nodes.Count);

      foreach (IINode node in this.nodes)
      {
         this.prevHiddenStates[node] = node.IsObjectHidden;
         node.Hide(this.hide);
      }
   }

   public override void Undo()
   {
      if (this.nodes == null)
         return;

      foreach (KeyValuePair<IINode, Boolean> n in this.prevHiddenStates)
      {
         n.Key.Hide(n.Value);
      }
   }
}
}
