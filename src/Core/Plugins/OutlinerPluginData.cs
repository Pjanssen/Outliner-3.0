using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Plugins
{
/// <summary>
/// A class containing metadata for an Outliner plugin.
/// </summary>
public class OutlinerPluginData
{
   /// <summary>
   /// Gets the plugintype of this plugin.
   /// </summary>
   public OutlinerPluginType PluginType { get; private set; }

   /// <summary>
   /// Gets the type of this plugin.
   /// </summary>
   public Type Type { get; private set; }

   /// <summary>
   /// Gets the displayname of this plugin.
   /// </summary>
   public String DisplayName { get; private set; }

   /// <summary>
   /// Gets the small display-image of this plugin.
   /// </summary>
   public Image DisplayImageSmall { get; private set; }

   /// <summary>
   /// Gets the large display-image of this plugin.
   /// </summary>
   public Image DisplayImageLarge { get; private set; }

   /// <summary>
   /// Initializes a new instance of the OutlinerPluginData class.
   /// </summary>
   /// <param name="type">The type to create the metadata for.</param>
   public OutlinerPluginData(Type type)
   {
      Throw.IfNull(type, "type");
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
