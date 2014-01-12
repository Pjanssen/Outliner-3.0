using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Plugins;

using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_BoxModeButton")]
public class BoxModeButton : NodePropertyButton
{
   public BoxModeButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.BoxMode)) { }



   protected override NodeProperty Property
   {

      get { return NodeProperty.BoxMode; }
   }

   protected override string ToolTipEnabled
   {
      get { return Resources.Tooltip_BoxMode; }
   }

   //TODO: Test if this override is needed?
   //protected override System.Drawing.Image ImageEnabled
   //{
   //   get { return OutlinerResources.button_boxmode; }
   //}

   public override TreeNodeLayoutItem Copy()
   {
      BoxModeButton newItem = new BoxModeButton();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.InvertBehavior = this.InvertBehavior;
      newItem.imageDisabled = this.imageDisabled;
      newItem.imageDisabled_Filtered = this.imageDisabled_Filtered;
      newItem.imageEnabled = this.imageEnabled;
      newItem.imageEnabled_Filtered = this.imageEnabled_Filtered;

      return newItem;
   }
}
}
