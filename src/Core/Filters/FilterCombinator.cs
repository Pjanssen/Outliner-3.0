using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen.Outliner.Controls.Options;

namespace PJanssen.Outliner.Filters
{
public class FilterCombinator<T> : Filter<T>
{
   private FilterCollection<T> filters;
   private BinaryPredicate<Boolean> predicate;

   public FilterCombinator() 
      : this(Functor.Or) { }

   public FilterCombinator(BinaryPredicate<Boolean> predicate)
      : this(predicate, new FilterCollection<T>()) { }

   public FilterCombinator(BinaryPredicate<Boolean> predicate, FilterCollection<T> filters) //List<Filter<T>> filters)
   {
      this.predicate = predicate;
      this.Filters = filters;
   }

   [TypeConverter(typeof(PredicateConverter))]
   [XmlIgnore]
   public BinaryPredicate<Boolean> Predicate 
   {
      get { return this.predicate; }
      set
      {
         this.predicate = value;
         this.OnFilterChanged();
      }
   }

   [XmlAttribute("predicate")]
   [Browsable(false)]
   public String PredicateString 
   { 
      get { return Functor.PredicateToString<Boolean>(this.Predicate); }
      set { this.Predicate = Functor.PredicateFromString(value); }
   }

   [XmlArray("Filters")]
   [Browsable(false)]
   public FilterCollection<T> Filters
   {
      get { return this.filters; }
      set
      {
         if (this.filters != null)
         {
            this.filters.FilterAdded -= filterAdded;
            this.filters.FilterRemoved -= filterRemoved;
         }

         this.filters = value;
         this.filters.Owner = this;
         this.filters.FilterAdded += filterAdded;
         this.filters.FilterRemoved += filterRemoved;
      }
   }

   private void filterAdded (object sender, FilterChangedEventArgs<T> e)
   {
      e.Filter.FilterChanged += this.childFilterChanged;
      this.OnFilterChanged();
   }

   private void filterRemoved(object sender, FilterChangedEventArgs<T> e)
   {
      e.Filter.FilterChanged -= this.childFilterChanged;
      this.OnFilterChanged();
   }

   protected override bool ShowNodeInternal(T data)
   {
      if (this.Filters == null)
         return true;

      Int32 filterCount = this.Filters.Count;
      if (filterCount == 0)
         return true;

      Boolean result = this.Filters[0].ShowNode(data);
      for (int i = 1; i < filterCount; i++)
      {
         result = this.Predicate(result, this.Filters[i].ShowNode(data));
      }

      return result;
   }

   void childFilterChanged(object sender, EventArgs e)
   {
      this.OnFilterChanged();
   }
}
}
