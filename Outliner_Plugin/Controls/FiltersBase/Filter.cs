using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.FiltersBase
{
   public abstract class Filter<T>
   {
      protected Filter()
      {
         this.OverrideEnabled = false;
      }

      public virtual Boolean OverrideEnabled { get; set; }

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
