using System;
using System.Collections.Generic;

namespace Outliner.Filters
{
public class FilterCollection<T>
{
   public delegate IEnumerable<T> GetChildNodes(T node);
   public GetChildNodes GetChildNodesFn { get; set; }

   private Boolean enabled;
   private List<Filter<T>> filters;

   public FilterCollection() : this(null) { }
   public FilterCollection(FilterCollection<T> collection)
   {
      if (collection == null)
      {
         this.enabled = false;
         this.filters = new List<Filter<T>>();
      }
      else
      {
         this.enabled = collection.Enabled;
         this.filters = collection.filters;
      }
   }

   
   /// <summary>
   /// Gets or sets whether the FilterCollection is enabled.
   /// </summary>
   public Boolean Enabled
   {
      get { return enabled; }
      set
      {
         enabled = value;
         this.OnFiltersEnabled();
      }
   }

   /// <summary>
   /// Adds the supplied filter to the collection as non-permanent.
   /// </summary>
   public void Add(Filter<T> filter)
   {
      if (filter == null)
         return;

      if (!this.filters.Contains(filter))
         this.filters.Add(filter);

      filter.FilterChanged += filterChanged;

      this.OnFilterAdded(filter);
   }
   


   /// <summary>
   /// Removes the supplied filter from the collection.
   /// </summary>
   public void Remove(Filter<T> filter)
   {
      if (filter == null)
         return;

      this.filters.Remove(filter);

      filter.FilterChanged -= filterChanged;

      this.OnFilterRemoved(filter);
   }
   /// <summary>
   /// Removes all filters of the supplied type from the collection.
   /// </summary>
   public void Remove(Type filterType)
   {
      List<Filter<T>> filtersToRemove = new List<Filter<T>>();

      foreach (Filter<T> filter in this.filters)
      {
         if (filter.GetType().Equals(filterType))
            filtersToRemove.Add(filter);
      }
      
      foreach (Filter<T> filter in filtersToRemove)
      {
         this.Remove(filter);
      }
      filtersToRemove.Clear();
   }


   /// <summary>
   /// Removes all non-permanent filters from the collection.
   /// </summary>
   public void Clear()
   {
      this.Clear(false);
   }
   /// <summary>
   /// Removes all filters from the collection.
   /// </summary>
   /// <param name="clearPermanentFilters">If true also removes permanent filters.</param>
   public void Clear(Boolean clearPermanentFilters)
   {
      List<Filter<T>> filtersToRemove = new List<Filter<T>>();

      foreach (Filter<T> filter in this.filters)
      {
         if (!filter.OverrideEnabled || clearPermanentFilters)
            filtersToRemove.Add(filter);
      }

      foreach (Filter<T> filter in filtersToRemove)
      {
         this.Remove(filter);
      }
      filtersToRemove.Clear();

      this.OnFiltersCleared();
   }

   /// <summary>
   /// The number of filters in the FilterCollection.
   /// </summary>
   public Int32 Count
   {
      get { return this.filters.Count; }
   }

   public Filter<T> Get(Int32 index)
   {
      if (index < 0 || index > this.filters.Count - 1)
         throw new ArgumentOutOfRangeException("index");

      return this.filters[index];
   }
   /// <summary>
   /// Retrieves the first found filter in the collection of the supplied type.
   /// </summary>
   public Filter<T> Get(Type filterType)
   {
      return this.filters.Find(f => f.GetType().Equals(filterType));
   }


   /// <summary>
   /// Returns true if the collection contains the supplied filter.
   /// </summary>
   public Boolean Contains(Filter<T> filter)
   {
      return this.filters.Contains(filter);
   }
   /// <summary>
   /// Returns true if the collection contains a filter of the supplied type.
   /// </summary>
   public Boolean Contains(Type filterType)
   {
      return this.Get(filterType) != null;
   }



   /// <summary>
   /// Tests whether the supplied node and its children should be shown.
   /// </summary>
   public virtual FilterResults ShowNode(T node)
   {
      FilterResults filterResult = FilterResults.Show;

      // Loop through filters.
      foreach (Filter<T> filter in this.filters)
      {
         if (this.Enabled || filter.OverrideEnabled)
         {
            if (filter.ShowNode(node) == FilterResults.Hide)
            {
               filterResult = FilterResults.Hide;
               break;
            }
         }
      }

      // If any of the filters return FilterResult.Hide, loop through children too.
      if (filterResult == FilterResults.Hide)
         return this.ShowChildNodes(node);
      else
         return filterResult;
   }

   /// <summary>
   /// Tests whether the children of the supplied node should be shown.
   /// </summary>
   protected virtual FilterResults ShowChildNodes(T node)
   {
      if (node == null || this.GetChildNodesFn == null)
         return FilterResults.Hide;

      IEnumerable<T> childNodes = this.GetChildNodesFn(node);
      if (childNodes != null)
      {
         foreach (T child in childNodes)
         {
            if (this.ShowNode(child) == FilterResults.Show
               || this.ShowChildNodes(child) == FilterResults.ShowChildren)
               return FilterResults.ShowChildren;
         }
      }

      return FilterResults.Hide;
   }



   // Events.

   /// <summary>
   /// Raised when the filter collection's Enabled property has been changed.
   /// </summary>
   public event EventHandler FiltersEnabled;
   protected virtual void OnFiltersEnabled()
   {
      if (this.FiltersEnabled != null)
         this.FiltersEnabled(this, new EventArgs());
   }

   /// <summary>
   /// Raised when the Clear method has been called.
   /// </summary>
   public event EventHandler FiltersCleared;
   protected virtual void OnFiltersCleared()
   {
      if (this.FiltersCleared != null)
         this.FiltersCleared(this, new EventArgs());
   }

   /// <summary>
   /// Raised when a filter has been added to the collection.
   /// </summary>
   public event EventHandler<FilterChangedEventArgs<T>> FilterAdded;
   protected virtual void OnFilterAdded(Filter<T> filter)
   {
      if (this.FilterAdded != null)
         this.FilterAdded(this, new FilterChangedEventArgs<T>(filter));
   }

   /// <summary>
   /// Raised when a filter has been removed from the collection.
   /// </summary>
   public event EventHandler<FilterChangedEventArgs<T>> FilterRemoved;
   protected virtual void OnFilterRemoved(Filter<T> filter)
   {
      if (this.FilterRemoved != null)
         this.FilterRemoved(this, new FilterChangedEventArgs<T>(filter));
   }

   /// <summary>
   /// Raised when the properties of a filter in the collection has been changed.
   /// </summary>
   public event EventHandler<FilterChangedEventArgs<T>> FilterChanged;
   protected void filterChanged(object sender, EventArgs e)
   {
      if (this.FilterChanged != null)
         this.FilterChanged(this, new FilterChangedEventArgs<T>(sender as Filter<T>));
   }
}


public class FilterChangedEventArgs<T> : EventArgs
{
   public Filter<T> Filter { get; private set; }

   public FilterChangedEventArgs(Filter<T> filter)
   {
      this.Filter = filter;
   }
}
}
