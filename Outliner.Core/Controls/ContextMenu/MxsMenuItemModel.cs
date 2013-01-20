using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ManagedServices;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
[OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
[LocalizedDisplayName(typeof(ContextMenuResources), "Str_MxsMenuItemModel")]
public class MxsMenuItemModel : MenuItemModel
{
   public MxsMenuItemModel() 
      : this(String.Empty, String.Empty, null, String.Empty) { }

   public MxsMenuItemModel(String text, String image, Type resType, String script) 
      : base(text, image, resType)
   {
      this.OnClickScript = script;
      this.EnabledScript = String.Empty;
      this.CheckedScript = String.Empty;
   }

   [XmlElement("onclickScript")]
   [DefaultValue("")]
   [DisplayName("OnClick Script")]
   [Category("2. Maxscript Properties")]
   [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
   public String OnClickScript { get; set; }

   [XmlElement("enabledScript")]
   [DefaultValue("")]
   [DisplayName("Enabled Script")]
   [Category("2. Maxscript Properties")]
   [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
   public String EnabledScript { get; set; }

   [XmlElement("checkedScript")]
   [DefaultValue("")]
   [DisplayName("Checked Script")]
   [Category("2. Maxscript Properties")]
   [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
   public String CheckedScript { get; set; }


   protected override Boolean Enabled( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.EnabledScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.EnabledScript);
      else
         return base.Enabled(clickedItem, treeView, clickedTn);
   }


   protected override Boolean Checked( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.CheckedScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.CheckedScript);
      else
         return base.Checked(clickedItem, treeView, clickedTn);
   }

   protected override void OnClick( ToolStripMenuItem clickedItem
                                  , Outliner.Controls.Tree.TreeView treeView
                                  , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.OnClickScript))
      {
         MaxscriptSDK.ExecuteMaxscriptCommand(this.OnClickScript);
      }
   }
}
}
