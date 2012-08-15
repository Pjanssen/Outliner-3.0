using System;
using System.Collections.Generic;
using System.Collections;

namespace Outliner.Filters
{
public class FilterCollection<T> : ICollection<Filter<T>>
{
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


   #region ICollection members
   
   public void Add(Filter<T> item)
   {
      if (item == null)
         return;

      if (!this.filters.Contains(item))
         this.filters.Add(item);

      item.FilterChanged += filterChanged;

      this.OnFilterAdded(item);
   }

   ///<summary>
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
         if (!filter.AlwaysEnabled || clearPermanentFilters)
            filtersToRemove.Add(filter);
      }

      foreach (Filter<T> filter in filtersToRemove)
      {
         this.Remove(filter);
      }
      filtersToRemove.Clear();
      
      this.OnFiltersCleared();
   }

   public bool Contains(Filter<T> item)
   {
      return this.filters.Contains(item);
   }
   
   public Boolean Contains(Type filterType)
   {
      return this.Get(filterType) != null;
   }

   public void CopyTo(Filter<T>[] array, int arrayIndex)
   {
      this.filters.CopyTo(array, arrayIndex);
   }

   public int Count
   {
      get { return this.filters.Count; }
   }

   public bool IsReadOnly
   {
      get { return false; }
   }

   public bool Remove(Filter<T> item)
   {
      if (item == null)
         return false;

      item.FilterChanged -= filterChanged;

      if (this.filters.Remove(item))
      {
         this.OnFilterRemoved(item);
         return true;
      }
      else
         return false;
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

   public IEnumerator<Filter<T>> GetEnumerator()
   {
      return this.filters.GetEnumerator();
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      return this.filters.GetEnumerator();
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

   #endregion


   /// <summary>
   /// Tests whether the supplied node and its children should be shown.
   /// </summary>
   public virtual FilterResults ShowNode(T node)
   {
      FilterResults filterResult = FilterResults.Show;

      // Loop through filters.
      foreach (Filter<T> filter in this.filters)
      {
         if (this.Enabled || filter.AlwaysEnabled)
         {
            if (filter.ShowNode(node) == FilterResults.Hide)
            {
               filterResult = FilterResults.Hide;
               break;
            }
         }
      }

      return filterResult;
   }


   #region Events.

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

   #endregion
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
