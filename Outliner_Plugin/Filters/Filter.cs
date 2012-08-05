using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Filters
{
   public abstract class Filter<T>
   {
      protected Filter() { }

      public virtual Boolean AlwaysEnabled 
      {
         get { return false; }
      }

      /// <summary>
      /// Returns whether the node should be shown or hidden. 
      /// Must return either FilterResult.Show or FilterResult.Hide, not FilterResult.ShowChildren.
      /// </summary>
      public abstract FilterResults ShowNode(T data);

      public event EventHandler FilterChanged;
      protected virtual void OnFilterChanged()
      {
         if (this.FilterChanged != null)
            this.FilterChanged(this, new EventArgs());
      }
   }
}
