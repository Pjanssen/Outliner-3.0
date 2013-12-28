using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Outliner.MaxUtils;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml;
using PJanssen;

namespace Outliner.Filters
{
[XmlRoot("Filters")]
public class FilterCollection<T> : ICollection<Filter<T>>
{
   private List<Filter<T>> filters;

   public FilterCollection() 
   {
      this.filters = new List<Filter<T>>();
   }

   public FilterCollection(FilterCollection<T> collection)
   {
      Throw.IfNull(collection, "collection");

      this.filters = collection.filters;
   }

   public Filter<T> Owner { get; internal set; }


   #region Events.

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
      if (this.FilterAdded != null && filter != null)
         this.FilterAdded(this, new FilterChangedEventArgs<T>(filter));
   }

   /// <summary>
   /// Raised when a filter has been removed from the collection.
   /// </summary>
   public event EventHandler<FilterChangedEventArgs<T>> FilterRemoved;
   protected virtual void OnFilterRemoved(Filter<T> filter)
   {
      if (this.FilterRemoved != null && filter != null)
         this.FilterRemoved(this, new FilterChangedEventArgs<T>(filter));
   }

   #endregion


   #region ICollection members

   public void Add(Filter<T> item)
   {
      Throw.IfNull(item, "item");

      if (!this.filters.Contains(item))
      {
         this.filters.Add(item);
         this.OnFilterAdded(item);
      }
   }

   public void Clear()
   {
      this.filters.Clear();
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
      Throw.IfNull(item, "item");

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

   public Filter<T> this[int index]
   {
      get
      {
         if (index < 0 || index > this.filters.Count - 1)
            throw new ArgumentOutOfRangeException("index");

         return this.filters[index];
      }
      set
      {
         if (index < 0 || index > this.filters.Count - 1)
            throw new ArgumentOutOfRangeException("index");

         Filter<T> oldFilter = this.filters[index];
         this.OnFilterRemoved(oldFilter);
         this.filters[index] = value;
         this.OnFilterAdded(value);
      }
   }

   /// <summary>
   /// Retrieves the first found filter in the collection of the supplied type.
   /// </summary>
   public Filter<T> Get(Type filterType)
   {
      return this.filters.Find(f => f.GetType().Equals(filterType));
   }

   #endregion
}
}
