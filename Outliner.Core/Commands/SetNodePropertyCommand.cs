using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.MaxUtils;
using Outliner.Scene;
using System.Reflection;

namespace Outliner.Commands
{
public class SetNodePropertyCommand<T> : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private NodeProperty property;
   private PropertyInfo propInfo;
   private T newValue;
   private Dictionary<IMaxNodeWrapper, T> prevValues;

   public SetNodePropertyCommand(IEnumerable<IMaxNodeWrapper> nodes, NodeProperty property, T newValue)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.property = property;
      this.newValue = newValue;
   }

   public SetNodePropertyCommand(IEnumerable<IMaxNodeWrapper> nodes, String propertyName, T newValue)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");
      Throw.IfArgumentIsNull(propertyName, "propertyName");

      this.nodes = nodes.ToList();
      this.propInfo = typeof(IMaxNodeWrapper).GetProperty(propertyName);
      this.property = NodeProperty.None;
      this.newValue = newValue;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetProperty; }
   }

   protected override void Do()
   {      
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
      Throw.IfArgumentIsNull(node, "node");

      if (this.propInfo == null)
         return (T)node.GetNodeProperty(this.property);
      else 
         return (T)this.propInfo.GetValue(node, null);
   }

   protected virtual void SetValue(IMaxNodeWrapper node, T value)
   {
      Throw.IfArgumentIsNull(node, "node");

      if (this.propInfo == null)
         node.SetNodeProperty(this.property, value);
      else
         this.propInfo.SetValue(node, value, null);
      
   }
}
}
