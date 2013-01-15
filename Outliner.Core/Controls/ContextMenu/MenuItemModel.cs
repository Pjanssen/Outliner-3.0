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
using Outliner.Configuration;

namespace Outliner.Controls.ContextMenu
{
/// <summary>
/// An Xml-serializable model for a ToolStripMenuItem.
/// </summary>
public abstract class MenuItemModel : ConfigurationFile
{
   public MenuItemModel() : this(String.Empty, String.Empty, null) { }
   public MenuItemModel(String text, String image, Type resType) 
      : base(text, image, String.Empty, null)
   {
      this.VisibleTypes = MaxNodeTypes.All;
      this.VisibleForEmptySelection = false;
      this.SubItems = new List<MenuItemModel>();
   }

   protected override string ImageBasePath
   {
      get { return OutlinerPaths.ContextMenusDir; }
   }

   [XmlElement("image24")]
   [DefaultValue("")]
   [Browsable(false)]
   public override string Image24Res
   {
      get { return ""; }
      set { }
   }

   [XmlElement("visible_types")]
   [DisplayName("Visible Types")]
   [Category("1. UI Properties")]
   [DefaultValue(MaxNodeTypes.All)]
   public virtual MaxNodeTypes VisibleTypes { get; set; }

   [XmlElement("visible_for_empty_selection")]
   [DisplayName("Visible for empty selection")]
   [Category("1. UI Properties")]
   [DefaultValue(false)]
   public virtual Boolean VisibleForEmptySelection { get; set; }

   [XmlArray("SubItems")]
   [XmlArrayItem("MenuItem")]
   [Browsable(false)]
   public virtual List<MenuItemModel> SubItems { get; set; }

   /// <summary>
   /// Magical method which tells the XmlSerializer when to serialize the SubItems list.
   /// </summary>
   public bool ShouldSerializeSubItems()
   {
      return !(this is IncludeContextMenuModel) && this.SubItems != null && this.SubItems.Count > 0;
   }

   /// <summary>
   /// Returns true if the MenuItem should be Enabled.
   /// </summary>
   protected virtual Boolean Enabled( Outliner.Controls.Tree.TreeView treeView
                                    , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      return true;
   }

   /// <summary>
   /// Returns true if the MenuItem should be in a Checked state.
   /// </summary>
   protected virtual Boolean Checked( Outliner.Controls.Tree.TreeView treeView
                                    , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      return false;
   }

   protected virtual Boolean Visible( Outliner.Controls.Tree.TreeView treeView
                                    , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      IEnumerable<IMaxNodeWrapper> context = HelperMethods.GetMaxNodes(treeView.SelectedNodes);

      if (context.Count() == 0)
         return this.VisibleForEmptySelection;
      else
         return context.Any(n => n != null && n.IsNodeType(this.VisibleTypes));
   }

   /// <summary>
   /// Executes code when the MenuItem is clicked.
   /// </summary>
   protected virtual void OnClick( Outliner.Controls.Tree.TreeView treeView
                                 , Outliner.Controls.Tree.TreeNode clickedTn) { }
   
   /// <summary>
   /// Creates a new ToolStripMenuItem from this model.
   /// </summary>
   /// <param name="clickedTn">The TreeNode that was clicked when opening the menu.</param>
   /// <param name="context">The context on which the menu item will operate (e.g. selected nodes).</param>
   public virtual ToolStripItem[] ToToolStripMenuItems( Outliner.Controls.Tree.TreeView treeView
                                                      , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");
      
      ToolStripMenuItem item = new ToolStripMenuItem();
      item.Text = this.Text;
      item.Image = this.Image16;
      Boolean visible = this.Visible(treeView, clickedTn);
      item.Visible = visible;

      if (visible)
      {
         Boolean enabled = this.Enabled(treeView, clickedTn);
         item.Enabled = enabled;
         if (enabled)
            item.Checked = this.Checked(treeView, clickedTn);

         foreach (MenuItemModel subitem in this.SubItems)
         {
            item.DropDownItems.AddRange(subitem.ToToolStripMenuItems(treeView, clickedTn));
         }

         item.Click += new EventHandler((sender, eventArgs) => this.OnClick(treeView, clickedTn));
      }

      return new ToolStripItem[1] { item };
   }
}
}
