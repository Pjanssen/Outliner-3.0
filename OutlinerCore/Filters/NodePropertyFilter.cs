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
   [FilterCategory(FilterCategories.Properties)]
   public class NodePropertyFilter : Filter<IMaxNodeWrapper>
   {
      public NodePropertyFilter() : this(BooleanNodeProperty.None) { }
      public NodePropertyFilter(BooleanNodeProperty property)
      {
         this.Property = property;
      }

      [XmlAttribute("property")]
      [DefaultValue(BooleanNodeProperty.None)]
      public BooleanNodeProperty Property { get; set; }

      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         return data.GetProperty(this.Property);
      }
   }
}
