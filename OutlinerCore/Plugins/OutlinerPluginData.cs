using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Outliner.Plugins
{
public class OutlinerPluginData
{
   public OutlinerPluginType PluginType { get; private set; }
   public Type Type { get; private set; }
   public String DisplayName { get; private set; }
   public Image DisplayImageSmall { get; private set; }
   public Image DisplayImageLarge { get; private set; }

   public OutlinerPluginData( OutlinerPluginType pluginType
                            , Type type, String displayName
                            , Image displayImageSmall, Image displayImageLarge)
   {
      this.PluginType = pluginType;
      this.Type = type;
      this.DisplayName = displayName;
      this.DisplayImageSmall = displayImageSmall;
      this.DisplayImageLarge = displayImageLarge;
   }
}
}
