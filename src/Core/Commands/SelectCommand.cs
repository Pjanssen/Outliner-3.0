using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Commands
{
/// <summary>
/// Selects nodes in the scene.
/// </summary>
public class SelectCommand : Command
{
   private Boolean openGroups;
   private IEnumerable<IMaxNode> nodes;

   /// <summary>
   /// Initializes a new instance of the SelectCommand class.
   /// </summary>
   /// <param name="nodes">The IMaxNodes to select.</param>
   public SelectCommand(IEnumerable<IMaxNode> nodes) : this(nodes, true) { }

   /// <summary>
   /// Initializes a new instance of the SelectCommand class.
   /// </summary>
   /// <param name="nodes">The IMaxNodes to select.</param>
   /// <param name="openGroups">Determines whether closed groups should be opened automatically to select the nodes inside them.</param>
   public SelectCommand(IEnumerable<IMaxNode> nodes, Boolean openGroups) 
   {
      Throw.IfNull(nodes, "nodes");

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
