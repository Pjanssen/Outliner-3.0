using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_SuperClass")]
   public class SuperClassFilter : Filter<IMaxNode>
   {
      public SuperClassFilter() : this(SClass_ID.Utility) { }
      public SuperClassFilter(SClass_ID superClass)
      {
         this.SuperClass = superClass;
      }

      [XmlAttribute("superclass")]
      [Category("2. Filter Properties")]
      [TypeConverter(typeof(SuperClassConverter))]
      public SClass_ID SuperClass { get; set; }

      protected override bool ShowNodeInternal(IMaxNode data)
      {
         if (data == null)
            return false;

         return data.SuperClassID.Equals(this.SuperClass);
      }
   }
}
