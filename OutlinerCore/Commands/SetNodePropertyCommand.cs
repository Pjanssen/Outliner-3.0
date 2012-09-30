using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxUtils;
using Outliner.Scene;
using System.Reflection;

namespace Outliner.Commands
{
public class SetNodePropertyCommand<T> : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private PropertyInfo propInfo;
   private T newValue;
   private Dictionary<IMaxNodeWrapper, T> prevValues;

   public SetNodePropertyCommand(IEnumerable<IMaxNodeWrapper> nodes, AnimatableProperty property, T newValue)
      : this(nodes, Enum.GetName(typeof(AnimatableProperty), property), newValue) { }

   public SetNodePropertyCommand(IEnumerable<IMaxNodeWrapper> nodes, String propertyName, T newValue)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(nodes, "nodes");
      ExceptionHelpers.ThrowIfArgumentIsNull(propertyName, "propertyName");

      this.nodes = nodes;
      this.propInfo = typeof(IMaxNodeWrapper).GetProperty(propertyName);
      this.newValue = newValue;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetProperty; }
   }

   protected override void Do()
   {
      ExceptionHelpers.ThrowIfNull(this.nodes, "inputNodes cannot be null. Execute must be called first.");
      
      this.prevValues = new Dictionary<IMaxNodeWrapper, T>(this.nodes.Count());

      foreach (IMaxNodeWrapper node in this.nodes)
      {
         this.prevValues.Add(node, this.GetValue(node));
         this.SetValue(node, this.newValue);
      }
   }

   protected override void Undo()
   {
      foreach (KeyValuePair<IMaxNodeWrapper, T> n in this.prevValues)
      {
         this.SetValue(n.Key, n.Value);
      }
   }

   protected virtual T GetValue(IMaxNodeWrapper node)
   {
      return (T)propInfo.GetValue(node, null);
   }

   protected virtual void SetValue(IMaxNodeWrapper node, T value)
   {
      this.propInfo.SetValue(node, value, null);
   }
}
}
