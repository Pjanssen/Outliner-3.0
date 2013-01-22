using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
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
public class MaxscriptFilter : Filter<IMaxNode>
{
   public MaxscriptFilter()
   {
      script = "";
   }

   private const String execFilterTemplate = "( fn mxsFn = ( local node = getAnimByHandle {0};\r\n {1} ); mxsFn(); )";

   private String script;
   private String filterFn;

   [XmlElement("script")]
   [DisplayName("Script")]
   [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
   public String Script
   {
      get { return script; }
      set
      {
         script = value;
         filterFn = String.Format(CultureInfo.InvariantCulture, execFilterTemplate, "{0:d}", value);
      }
   }

   protected override Boolean ShowNodeInternal(IMaxNode data)
   {
      if (String.IsNullOrEmpty(this.script))
         return true;

      INodeWrapper iinodeWrapper = data as INodeWrapper;
      if (iinodeWrapper == null)
         return false;

      UIntPtr handle = MaxInterfaces.Global.Animatable.GetHandleByAnim(iinodeWrapper.INode);
      String script = String.Format(CultureInfo.InvariantCulture, filterFn, handle);
      return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(script);
   }
}
}
