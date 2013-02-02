using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Outliner.MaxUtils;
using System.ComponentModel;

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

   public OutlinerPluginData(Type type)
   {
      Throw.IfArgumentIsNull(type, "type");
      this.Type = type;

      OutlinerPluginAttribute pluginAttr = type.GetAttribute<OutlinerPluginAttribute>();
      Throw.IfNull(pluginAttr, "OutlinerPlugin attribute not found on type.");
      this.PluginType = pluginAttr.PluginType;

      DisplayNameAttribute dispAtrr = type.GetAttribute<DisplayNameAttribute>();
      if (dispAtrr != null)
         this.DisplayName = dispAtrr.DisplayName;
      else
         this.DisplayName = type.Name;

      LocalizedDisplayImageAttribute imgAttr = type.GetAttribute<LocalizedDisplayImageAttribute>();
      if (imgAttr != null)
      {
         this.DisplayImageSmall = imgAttr.DisplayImageSmall;
         this.DisplayImageLarge = imgAttr.DisplayImageLarge;
      }
      else
      {
         this.DisplayImageSmall = null;
         this.DisplayImageLarge = null;
      }
   }
}
}
