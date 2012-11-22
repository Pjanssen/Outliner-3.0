using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using Outliner.Plugins;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
/// <summary>
/// An Xml-serializable model for a ToolStripMenuItem.
/// </summary>
[XmlInclude(typeof(ActionMenuItemModel))]
[XmlInclude(typeof(IncludeContextMenuModel))]
[XmlInclude(typeof(MxsMenuItemModel))]
[XmlInclude(typeof(NodePropertyMenuItemModel))]
[XmlInclude(typeof(SeparatorMenuItemModel))]
public abstract class MenuItemModel
{
   public MenuItemModel() : this(String.Empty, String.Empty, null) { }
   public MenuItemModel(String text, String image, Type resType)
   {
      this.TextRes = text;
      this.ImageRes = image;
      if (resType != null)
         this.ResourceTypeName = resType.Name;
      this.VisibleTypes = MaxNodeTypes.All;
      this.SubItems = new List<MenuItemModel>();
   }

   [XmlAttribute("text")]
   [DisplayName("Text")]
   [TypeConverter(typeof(StringResourceConverter))]
   public String TextRes { get; set; }

   [XmlAttribute("image")]
   [DefaultValue("")]
   [DisplayName("Image")]
   [TypeConverter(typeof(ImageResourceConverter))]
   public String ImageRes { get; set; }

   [XmlAttribute("resource_type")]
   [DefaultValue("")]
   [Browsable(false)]
   public String ResourceTypeName { get; set; }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeTypes.All)]
   public MaxNodeTypes VisibleTypes { get; set; }

   private Type resourceType;
   [Browsable(true)]
   [TypeConverter(typeof(ResourceTypeConverter))]
   public Type ResourceType
   {
      get
      {
         if (this.resourceType == null && !String.IsNullOrEmpty(this.ResourceTypeName))
         {
            foreach (Assembly pluginAssembly in OutlinerPlugins.PluginAssemblies)
            {
               this.resourceType = pluginAssembly.GetType(this.ResourceTypeName);
               if (this.resourceType != null)
                  break;
            }
         }
         return this.resourceType;
      }
      set
      {
         this.resourceType = value;
         this.ResourceTypeName = value.Name;
      }
   }

   [XmlIgnore]
   [Browsable(false)]
   public String Text
   {
      get
      {
         if (this.ResourceType != null && this.TextRes != null)
            return ResourceHelpers.LookupString(this.ResourceType, this.TextRes);
         else
            return this.TextRes;
      }
   }

   [XmlIgnore]
   [Browsable(false)]
   public Image Image
   {
      get
      {
         if (String.IsNullOrEmpty(this.ImageRes))
            return null;

         if (this.ResourceType != null)
            return ResourceHelpers.LookupImage(this.ResourceType, this.ImageRes);
         else
         {
            String path = this.ImageRes;
            if (!Path.IsPathRooted(path))
               path = Path.Combine(OutlinerPaths.ContextMenuDir, path);
               
            return Image.FromFile(path);
         }
      }
   }


   [XmlArray("SubItems")]
   [XmlArrayItem("MenuItem")]
   [Browsable(false)]
   public virtual List<MenuItemModel> SubItems { get; set; }

   /// <summary>
   /// Magical method which tells the XmlSerializer when to serialize the SubItems list.
   /// </summary>
   public bool ShouldSerializeSubItems()
   {
      return this.SubItems != null && this.SubItems.Count > 0;
   }

   /// <summary>
   /// Returns true if the MenuItem should be Enabled.
   /// </summary>
   protected virtual Boolean Enabled( Outliner.Controls.Tree.TreeNode clickedTn
                                    , IEnumerable<IMaxNodeWrapper> context)
   {
      return true;
   }

   /// <summary>
   /// Returns true if the MenuItem should be in a Checked state.
   /// </summary>
   protected virtual Boolean Checked( Outliner.Controls.Tree.TreeNode clickedTn
                                    , IEnumerable<IMaxNodeWrapper> context)
   {
      return false;
   }

   /// <summary>
   /// Executes code when the MenuItem is clicked.
   /// </summary>
   public abstract void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                               , IEnumerable<IMaxNodeWrapper> context);


   /// <summary>
   /// Creates a new ToolStripMenuItem from this model.
   /// </summary>
   /// <param name="clickedTn">The TreeNode that was clicked when opening the menu.</param>
   /// <param name="context">The context on which the menu item will operate (e.g. selected nodes).</param>
   public virtual ToolStripItem ToToolStripMenuItem( Outliner.Controls.Tree.TreeNode clickedTn
                                                   , IEnumerable<IMaxNodeWrapper> context)
   {
      ToolStripMenuItem item = new ToolStripMenuItem();
      item.Text = this.Text;
      item.Image = this.Image;
      Boolean visible = context.Any(n => n != null && n.IsNodeType(this.VisibleTypes));
      item.Visible = visible;

      if (visible)
      {
         Boolean enabled = this.Enabled(clickedTn, context);
         item.Enabled = enabled;
         if (enabled)
            item.Checked = this.Checked(clickedTn, context);

         foreach (MenuItemModel subitem in this.SubItems)
         {
            item.DropDownItems.Add(subitem.ToToolStripMenuItem(clickedTn, context));
         }

         item.Click += new EventHandler((sender, eventArgs) => this.OnClick(clickedTn, context));
      }

      return item;
   }
}
}
