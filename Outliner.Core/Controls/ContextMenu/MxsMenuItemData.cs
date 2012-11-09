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
public class MxsMenuItemData : MenuItemData
{
   public MxsMenuItemData() : this(String.Empty, String.Empty, null, String.Empty) { }

   public MxsMenuItemData(String text, String image, Type resType, String script) 
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

   public override void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                               , IEnumerable<IMaxNodeWrapper> context)
   {
      if (!String.IsNullOrEmpty(this.OnClickScript))
      {
         MaxscriptSDK.ExecuteMaxscriptCommand(this.OnClickScript);
      }
   }

   protected override Boolean Enabled( Outliner.Controls.Tree.TreeNode clickedTn
                                     , IEnumerable<IMaxNodeWrapper> context)
   {
      if (!String.IsNullOrEmpty(this.EnabledScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.EnabledScript);
      else
         return base.Enabled(clickedTn, context);
   }

   protected override bool Checked( Outliner.Controls.Tree.TreeNode clickedTn
                                  , IEnumerable<IMaxNodeWrapper> context)
   {
      if (!String.IsNullOrEmpty(this.CheckedScript))
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(this.CheckedScript);
      else
         return base.Checked(clickedTn, context);
   }
}
}
