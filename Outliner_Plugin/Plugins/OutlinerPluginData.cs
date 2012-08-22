using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Outliner.Plugins
{
   public struct OutlinerPluginData
   {
      OutlinerPluginType pluginType;
      Type type;
      String displayName;
      Image displayImageSmall;
      Image displayImageLarge;

      public OutlinerPluginType PluginType { get { return pluginType; } }
      public Type Type { get { return type; } }
      public String DisplayName { get { return displayName; } }
      public Image DisplayImageSmall { get { return displayImageSmall; } }
      public Image DisplayImageLarge { get { return displayImageLarge; } }

      public OutlinerPluginData(OutlinerPluginType pluginType
                               , Type type, String displayName
                               , Image displayImageSmall, Image displayImageLarge)
      {
         this.pluginType = pluginType;
         this.type = type;
         this.displayName = displayName;
         this.displayImageSmall = displayImageSmall;
         this.displayImageLarge = displayImageLarge;
      }
   }
}
