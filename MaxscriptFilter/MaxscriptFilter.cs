using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ManagedServices;
using Outliner.MaxUtils;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Filters
{
[OutlinerPlugin(OutlinerPluginType.Filter)]
[LocalizedDisplayName(typeof(Resources), "Filter_Maxscript")]
public class MaxscriptFilter : Filter<IMaxNodeWrapper>
{
   public MaxscriptFilter()
   {
      _script = "";
   }

   private const String execFilterTemplate = "( local node = getAnimByHandle {0}; {1} )";

   private String _script;
   private String _filterFn;

   [XmlElement("script")]
   public String Script
   {
      get { return _script; }
      set
      {
         _script = value;
         _filterFn = String.Format(CultureInfo.InvariantCulture, execFilterTemplate, "{0:d}", value);
      }
   }

   protected override Boolean ShowNodeInternal(IMaxNodeWrapper data)
   {
      if (String.IsNullOrEmpty(_script))
         return true;

      IINodeWrapper iinodeWrapper = data as IINodeWrapper;
      if (data == null)
         return false;

      UIntPtr handle = MaxInterfaces.Global.Animatable.GetHandleByAnim(iinodeWrapper.IINode);
      String script = String.Format(CultureInfo.InvariantCulture, _filterFn, handle);
      return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(script);
   }
}
}
