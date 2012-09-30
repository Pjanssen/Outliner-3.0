using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Modes;
using Outliner.NodeSorters;
using Outliner.Scene;
using Outliner.Filters;
using Outliner.Controls.Tree.Layout;
using System.Drawing;
using Outliner.Controls.Tree;
using Autodesk.Max;
using MaxUtils;
using System.IO;
using System.Xml.Serialization;
using Outliner.Plugins;
using System.ComponentModel;

namespace Outliner.Presets
{
public class OutlinerPreset
{
   public OutlinerPreset()
   {
      this.Name = String.Empty;
      this.TreeModeTypeName = String.Empty;
      this.NodeSorter = new AlphabeticalSorter();
      this.TreeNodeLayout = TreeNodeLayout.SimpleLayout;
      this.Filters = new FilterCollection<Scene.IMaxNodeWrapper>();
   }

   [XmlIgnore]
   internal Boolean IsDefaultPreset { get; set; }

   [XmlElement("name")]
   public virtual String Name { get; set; }

   [XmlElement("image_resource_type")]
   [DefaultValue("")]
   public virtual String ImageResourceType { get; set; }

   [XmlElement("image_16")]
   [DefaultValue("")]
   public virtual String Image16Name { get; set; }
   
   [XmlElement("image_24")]
   [DefaultValue("")]
   public virtual String Image24Name { get; set; }
   
   [XmlIgnore]
   public Image Image16
   {
      get
      {
         Image img = this.ImageFromFile(this.Image16Name);
         if (img == null)
            img = this.ImageFromResource(this.ImageResourceType, this.Image16Name);
         if (img == null)
            img = OutlinerResources.default_preset_16;

         return img;
      }
   }

   [XmlIgnore]
   public Image Image24
   {
      get
      {
         Image img = this.ImageFromFile(this.Image24Name);
         if (img == null)
            img = this.ImageFromResource(this.ImageResourceType, this.Image24Name);
         if (img == null)
            img = OutlinerResources.default_preset_24;

         return img;
      }
   }

   [XmlElement("treemode")]
   public virtual String TreeModeTypeName { get; set; }

   [XmlElement("nodesorter")]
   public virtual NodeSorter NodeSorter { get; set; }

   [XmlElement("treenodelayout")]
   public virtual TreeNodeLayout TreeNodeLayout { get; set; }

   [XmlElement("filters")]
   public virtual FilterCollection<IMaxNodeWrapper> Filters { get; set; }

   public TreeMode CreateTreeMode(TreeView tree)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(tree, "tree");

      Type treeModeType = OutlinerPlugins.GetPluginType( OutlinerPluginType.TreeMode
                                                       , this.TreeModeTypeName);

      if (treeModeType == null)
         treeModeType = typeof(NullTreeMode);

      return Activator.CreateInstance(treeModeType, new object[] { tree }) as TreeMode;
   }

   private Image ImageFromFile(String path)
   {
      Image img = null;
      if (!String.IsNullOrEmpty(path))
      {
         if (!Path.IsPathRooted(path))
            path = Path.Combine(OutlinerPaths.Presets, path);

         if (!Path.HasExtension(path))
            path = Path.ChangeExtension(path, "png");

         if (File.Exists(path))
            img = Image.FromFile(path);
      }

      return img;
   }

   private Image ImageFromResource(string resourceTypeName, string resourceKey)
   {
      if (resourceTypeName == null || resourceKey == null)
         return null;

      Type resourceType = null;
      foreach (System.Reflection.Assembly pluginAssembly in OutlinerPlugins.PluginAssemblies)
      {
         resourceType = pluginAssembly.GetType(resourceTypeName);
         if (resourceType != null)
            break;
      }
      if (resourceType != null)
         return ResourceHelpers.LookupImage(resourceType, resourceKey);
      else
         return null;
   }
}
}
