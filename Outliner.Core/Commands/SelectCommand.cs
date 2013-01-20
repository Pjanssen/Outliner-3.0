using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class SelectCommand : Command
{
   private Boolean openGroups;
   private IEnumerable<IMaxNode> nodes;

   public SelectCommand(IEnumerable<IMaxNode> nodes) : this(nodes, true) { }

   public SelectCommand(IEnumerable<IMaxNode> nodes, Boolean openGroups) 
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.openGroups = openGroups;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Select; }
   }

   public override void Do()
   {
      IInterface ip = MaxInterfaces.COREInterface;

      if (this.openGroups)
         GroupHelpers.OpenSelectedGroupHeads(this.nodes);

      //Select new selection.
      ip.ClearNodeSelection(false);
      nodes.ForEach(n => n.IsSelected = true);
   }
}
}
