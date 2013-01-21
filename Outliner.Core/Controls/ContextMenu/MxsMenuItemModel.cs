using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Autodesk.Max;
using ManagedServices;
using Outliner.MaxUtils;
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
      {
         String script = FormatScript(this.EnabledScript, treeView);
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(script);
      }
      else
      {
         return base.Enabled(clickedItem, treeView, clickedTn);
      }
   }


   protected override Boolean Checked( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.CheckedScript))
      {
         String script = FormatScript(this.CheckedScript, treeView);
         return MaxscriptSDK.ExecuteBooleanMaxscriptQuery(script);
      }
      else
      {
         return base.Checked(clickedItem, treeView, clickedTn);
      }
   }

   protected override void OnClick( ToolStripMenuItem clickedItem
                                  , Outliner.Controls.Tree.TreeView treeView
                                  , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfArgumentIsNull(treeView, "treeView");

      if (!String.IsNullOrEmpty(this.OnClickScript))
      {
         String script = FormatScript(this.OnClickScript, treeView);
         MaxscriptSDK.ExecuteMaxscriptCommand(script);
         Viewports.ForceRedraw();
      }
   }

   private static String FormatScript(String script, Tree.TreeView treeView)
   {
      StringBuilder scriptBuilder = new StringBuilder("fn mxsFn = ( local nodes = #( ");

      foreach (IMaxNode node in HelperMethods.GetMaxNodes(treeView.SelectedNodes))
      {
         IAnimatable anim = node.BaseObject as IAnimatable;
         if (anim != null)
         {
            UIntPtr handle = MaxInterfaces.Global.Animatable.GetHandleByAnim(anim);
            scriptBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n(getAnimByHandle {0:d}),", handle);
         }
      }
      scriptBuilder.Remove(scriptBuilder.Length - 1, 1);
      scriptBuilder.AppendLine("); ");
      scriptBuilder.AppendLine(script);
      scriptBuilder.Append("); mxsFn();");
      return scriptBuilder.ToString();
   }
}
}
