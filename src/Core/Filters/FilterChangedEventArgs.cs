using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Filters
{
   public class FilterChangedEventArgs<T> : EventArgs
   {
      public Filter<T> Filter { get; private set; }

      public FilterChangedEventArgs(Filter<T> filter)
      {
         this.Filter = filter;
      }
   }
}
