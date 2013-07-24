using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Filters
{
   /// <summary>
   /// A Filter for filtering IMaxNodes by a node property value.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_NodeProperty")]
   public class NodePropertyFilter : Filter<IMaxNode>
   {
      private BooleanNodeProperty property;

      /// <summary>
      /// Initializes a new instance of the NodePropertyFilter class.
      /// </summary>
      public NodePropertyFilter() : this(BooleanNodeProperty.None) { }

      /// <summary>
      /// Initializes a new instance of the NodePropertyFilter class using
      /// the given BooleanNodeProperty.
      /// </summary>
      /// <param name="property">The node property to filter by.</param>
      public NodePropertyFilter(BooleanNodeProperty property)
      {
         this.property = property;
      }

      /// <summary>
      /// Gets or sets the node property to filter IMaxNodes by.
      /// </summary>
      [XmlAttribute("property")]
      [DefaultValue(BooleanNodeProperty.None)]
      public BooleanNodeProperty Property 
      {
         get { return this.property; }
         set
         {
            this.property = value;
            this.OnFilterChanged();
         }
      }

      protected override bool ShowNodeInternal(IMaxNode data)
      {
         return data.GetNodeProperty(this.Property);
      }
   }
}
