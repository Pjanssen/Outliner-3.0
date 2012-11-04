using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.MaxUtils;
using Outliner.Plugins;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Controls.Options;

namespace Outliner.Filters
{
public class FilterCombinator<T> : Filter<T> //, IXmlSerializable
{
   private BinaryPredicate<Boolean> predicate;

   public FilterCombinator() 
      : this(Functor.Or) { }

   public FilterCombinator(BinaryPredicate<Boolean> predicate)
      : this(predicate, new List<Filter<T>>()) { }

   public FilterCombinator(BinaryPredicate<Boolean> predicate, List<Filter<T>> filters)
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
   public String PredicateString 
   { 
      get { return Functor.PredicateToString<Boolean>(this.Predicate); }
      set { this.Predicate = Functor.PredicateFromString(value); }
   }

   [Browsable(false)]
   public List<Filter<T>> Filters { get; set; }

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

   /// <summary>
   /// Retrieves the first found filter in the collection of the supplied type.
   /// </summary>
   public Filter<T> Get(Type filterType)
   {
      return this.Filters.Find(f => f.GetType().Equals(filterType));
   }

   #region ICollection members

   //public void Add(Filter<T> item)
   //{
   //   this.Filters.Add(item);
   //}

   //public void Clear()
   //{
   //   this.Filters.Clear();
   //}

   //public bool Contains(Filter<T> item)
   //{
   //   return this.Filters.Contains(item);
   //}

   //public Boolean Contains(Type filterType)
   //{
   //   return this.Get(filterType) != null;
   //}

   //public void CopyTo(Filter<T>[] array, int arrayIndex)
   //{
   //   this.Filters.CopyTo(array, arrayIndex);
   //}

   //[Browsable(false)]
   //public int Count
   //{
   //   get { return this.Filters.Count; }
   //}

   //[Browsable(false)]
   //public bool IsReadOnly
   //{
   //   get { return false; }
   //}

   //public bool Remove(Filter<T> item)
   //{
   //   return this.Filters.Remove(item);
   //}

   //public IEnumerator<Filter<T>> GetEnumerator()
   //{
   //   return this.Filters.GetEnumerator();
   //}

   //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   //{
   //   return this.Filters.GetEnumerator();
   //}

   #endregion


   //#region IXmlSerializable members

   //public System.Xml.Schema.XmlSchema GetSchema()
   //{
   //   return null;
   //}

   //public void ReadXml(System.Xml.XmlReader reader)
   //{
   //   //this.Enabled = reader.GetAttribute("enabled") == (true.ToString());
   //   this.Predicate = Functor.PredicateFromString(reader.GetAttribute("combinator"));
   //   Int32 minDepth = reader.Depth + 1;
   //   while (reader.Read() && reader.Depth >= minDepth)
   //   {
   //      if (reader.NodeType == System.Xml.XmlNodeType.Element)
   //      {
   //         XmlSerializer serializer = new XmlSerializer(typeof(Filter<T>), Outliner.Plugins.OutlinerPlugins.GetSerializableTypes());
   //         Filter<T> filter = serializer.Deserialize(reader) as Filter<T>;
   //         this.Filters.Add(filter);
   //      }
   //   }
   //}

   //public void WriteXml(System.Xml.XmlWriter writer)
   //{
   //   //writer.WriteAttributeString("enabled", "", this.Enabled.ToString());
   //   writer.WriteAttributeString("predicate", "", Functor.PredicateToString<Boolean>(this.Predicate));
   //   foreach (Filter<T> filter in this.Filters)
   //   {
   //      IXmlSerializable serializable = filter as IXmlSerializable;
   //      if (serializable != null)
   //         serializable.WriteXml(writer);
   //      else
   //      {
   //         XmlSerializer serializer = new XmlSerializer(typeof(Filter<T>), Outliner.Plugins.OutlinerPlugins.GetSerializableTypes());
   //         serializer.Serialize(writer, filter);
   //      }
   //   }
   //}

   //#endregion
}
}
