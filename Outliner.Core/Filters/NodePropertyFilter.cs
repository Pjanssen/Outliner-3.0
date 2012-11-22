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
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "FilterNodeProp")]
   [FilterCategory(FilterCategory.Hidden)]
   public class NodePropertyFilter : Filter<IMaxNodeWrapper>
   {
      private BooleanNodeProperty property;

      public NodePropertyFilter() : this(BooleanNodeProperty.None) { }
      public NodePropertyFilter(BooleanNodeProperty property)
      {
         this.property = property;
      }

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

      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         return data.GetProperty(this.Property);
      }
   }
}
