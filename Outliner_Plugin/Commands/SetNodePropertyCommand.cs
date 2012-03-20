using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
public abstract class SetNodePropertyCommand<T> : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private T newValue;
   private Dictionary<IMaxNodeWrapper, T> prevValues;

   protected SetNodePropertyCommand(IEnumerable<IMaxNodeWrapper> nodes, T newValue)
   {
      this.nodes = nodes.ToArray();
      this.newValue = newValue;
   }

   public abstract T GetValue(IMaxNodeWrapper node);
   public abstract void SetValue(IMaxNodeWrapper node, T value);

   public override void Do()
   {
      if (this.nodes == null)
         return;

      this.prevValues = new Dictionary<IMaxNodeWrapper, T>(this.nodes.Count());

      foreach (IMaxNodeWrapper node in this.nodes)
      {
         this.prevValues.Add(node, this.GetValue(node));
         this.SetValue(node, this.newValue);
      }
   }

   public override void Undo()
   {
      if (this.nodes == null)
         return;

      foreach (KeyValuePair<IMaxNodeWrapper, T> n in this.prevValues)
      {
         this.SetValue(n.Key, n.Value);
      }
   }
}
}
