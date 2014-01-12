using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Scene;
using System.Reflection;
using Autodesk.Max.Plugins;
using Autodesk.Max;

namespace PJanssen.Outliner.Commands
{
/// <summary>
/// Sets the value of a NodeProperty on a set of nodes.
/// </summary>
/// <typeparam name="T">The type of the NodeProperty value.</typeparam>
public class SetNodePropertyCommand<T> : CustomRestoreObjCommand
{
   private IEnumerable<IMaxNode> nodes;
   private NodeProperty property;
   private T newValue;
   private IEnumerable<Tuple<IMaxNode, object>> oldValues;

   /// <summary>
   /// Initializes a new instance of the SetNodePropertyCommand class.
   /// </summary>
   /// <param name="nodes">The IMaxNodes to set the property on.</param>
   /// <param name="property">The NodeProperty to set.</param>
   /// <param name="newValue">The new value to set.</param>
   public SetNodePropertyCommand(IEnumerable<IMaxNode> nodes, NodeProperty property, T newValue)
   {
      Throw.IfNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.property = property;
      this.newValue = newValue;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetProperty; }
   }

   public override void Redo()
   {
      this.oldValues = this.nodes.Select(n => new Tuple<IMaxNode, object>(n, n.GetNodeProperty(this.property)))
                                 .ToList();
      this.nodes.ForEach(n => n.SetNodeProperty(this.property, this.newValue));
   }

   public override void Restore(bool isUndo)
   {
      foreach (Tuple<IMaxNode, object> oldValue in this.oldValues)
      {
         oldValue.Item1.SetNodeProperty(this.property, oldValue.Item2);
      }
   }
}
}
