using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MaxUtils;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml;

namespace Outliner.Filters
{
[XmlRoot("Filters")]
public class FilterCollection<T> : ICollection<Filter<T>>, IXmlSerializable
{
   private Boolean enabled;
   private BinaryPredicate<Boolean> combinator;
   private List<Filter<T>> filters;

   public FilterCollection() : this(null) { }
   public FilterCollection(FilterCollection<T> collection)
   {
      if (collection == null)
      {
         this.enabled = false;
         this.filters = new List<Filter<T>>();
         this.combinator = Functor.Or;
      }
      else
      {
         this.enabled = collection.Enabled;
         this.filters = collection.filters;
         this.combinator = collection.Combinator;
      }
   }


   /// <summary>
   /// Gets or sets whether the FilterCollection is enabled.
   /// </summary>
   [XmlElement("Enabled")]
   [DefaultValue(false)]
   public Boolean Enabled
   {
      get { return enabled; }
      set
      {
         enabled = value;
         this.OnFiltersEnabled();
         this.OnGeneralFiltersChanged();
      }
   }



   [XmlIgnore]
   public BinaryPredicate<Boolean> Combinator 
   {
      get { return this.combinator; }
      set
      {
         this.combinator = value;
         this.OnGeneralFiltersChanged();
      }
   }

   public Boolean InitialCombinatorValue { get; set; }


   /// <summary>
   /// Tests whether the supplied node and its children should be shown.
   /// </summary>
   public virtual Boolean ShowNode(T node)
   {
      Int32 filtersCount = this.filters.Count;
      if (!this.Enabled || filtersCount == 0)
         return true;

      Boolean show = false;
      if (filtersCount == 1)
      {
         show = this.filters[0].ShowNode(node);
      }
      else
      {
         BinaryPredicate<Boolean> combinator = this.Combinator;
         show = this.filters[0].ShowNode(node);
         for (int i = 1; i < filtersCount; i++)
         {
            show = combinator(show, this.filters[i].ShowNode(node));
         }
      }
      return show;
   }


   #region Events.

   /// <summary>
   /// A general event raised when the filter collection changes.
   /// </summary>
   public event EventHandler GeneralFiltersChanged;
   protected virtual void OnGeneralFiltersChanged()
   {
      if (this.GeneralFiltersChanged != null)
         this.GeneralFiltersChanged(this, new EventArgs());
   }

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
   //Forwards the event from any Filter<T>, so you can subscribe to the collection
   //instead of each individual filter.
   protected void OnFilterChanged(object sender, EventArgs e)
   {
      if (this.FilterChanged != null)
         this.FilterChanged(this, new FilterChangedEventArgs<T>(sender as Filter<T>));
      this.OnGeneralFiltersChanged();
   }

   #endregion


   #region ICollection members

   public void Add(Filter<T> item)
   {
      if (item == null)
         return;

      if (!this.filters.Contains(item))
         this.filters.Add(item);

      item.FilterChanged += OnFilterChanged;

      this.OnFilterAdded(item);
      this.OnGeneralFiltersChanged();
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
      this.OnGeneralFiltersChanged();
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

      item.FilterChanged -= OnFilterChanged;

      if (this.filters.Remove(item))
      {
         this.OnFilterRemoved(item);
         this.OnGeneralFiltersChanged();
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


   #region IXmlSerializable members
   
   public System.Xml.Schema.XmlSchema GetSchema()
   {
      return null;
   }

   public void ReadXml(System.Xml.XmlReader reader)
   {
      this.Enabled    = reader.GetAttribute("enabled") == (true.ToString());
      this.Combinator = Functor.PredicateFromString(reader.GetAttribute("combinator"));
      Int32 minDepth = reader.Depth + 1;
      while (reader.Read() && reader.Depth >= minDepth)
      {
         if (reader.NodeType == System.Xml.XmlNodeType.Element)
         {
            XmlSerializer serializer = new XmlSerializer(typeof(Filter<T>), Outliner.Plugins.OutlinerPlugins.GetSerializableTypes());
            Filter<T> filter = serializer.Deserialize(reader) as Filter<T>;
            this.Add(filter);
         }
      }
   }

   public void WriteXml(System.Xml.XmlWriter writer)
   {
      writer.WriteAttributeString("enabled", "", this.Enabled.ToString());
      writer.WriteAttributeString("combinator", "", Functor.PredicateToString<Boolean>(this.Combinator));
      foreach (Filter<T> filter in this.filters)
      {
         XmlSerializer serializer = new XmlSerializer(typeof(Filter<T>), Outliner.Plugins.OutlinerPlugins.GetSerializableTypes());
         serializer.Serialize(writer, filter);
      }
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
