using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using ManagedServices;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
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
   public String OnClickScript { get; set; }

   [XmlElement("enabledScript")]
   [DefaultValue("")]
   [DisplayName("Enabled Script")]
   [Category("2. Maxscript Properties")]
   public String EnabledScript { get; set; }

   [XmlElement("checkedScript")]
   [DefaultValue("")]
   [DisplayName("Checked Script")]
   [Category("2. Maxscript Properties")]
   public String CheckedScript { get; set; }


   protected override Boolean Enabled(Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.EnabledScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.EnabledScript);
      else
         return base.Enabled(treeView, clickedTn);
   }


   protected override Boolean Checked(Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.CheckedScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.CheckedScript);
      else
         return base.Checked(treeView, clickedTn);
   }

   protected override void OnClick( Outliner.Controls.Tree.TreeView treeView
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
