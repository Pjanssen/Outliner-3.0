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

   [XmlElement("OnClickScript")]
   [DefaultValue("")]
   public String OnClickScript { get; set; }

   [XmlElement("EnabledScript")]
   [DefaultValue("")]
   public String EnabledScript { get; set; }

   [XmlElement("CheckedScript")]
   [DefaultValue("")]
   public String CheckedScript { get; set; }


   protected override Boolean Enabled(Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.EnabledScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.EnabledScript);
      else
         return base.Enabled(treeView, clickedTn);
   }


   protected override Boolean Checked(Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.CheckedScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.CheckedScript);
      else
         return base.Checked(treeView, clickedTn);
   }

   protected override void OnClick( Outliner.Controls.Tree.TreeView treeView
                                  , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.OnClickScript))
      {
         MaxscriptSDK.ExecuteMaxscriptCommand(this.OnClickScript);
      }
   }
}
}
