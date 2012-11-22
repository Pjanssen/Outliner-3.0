using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;
using System.Drawing;
using Outliner.Plugins;
using Outliner.Controls.Tree.Layout;
using System.Xml.Serialization;
using Outliner.MaxUtils;

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
public class RenderableButton : NodePropertyButton
{
   public RenderableButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Renderable)) { }

   protected override NodeProperty Property
   {
      get { return NodeProperty.Renderable; }
   }

   protected override string ToolTipEnabled
   {
      get { return Resources.Tooltip_Renderable; }
   }

   //TODO: test if this override is necessary.
   //protected override Image ImageEnabled
   //{
   //   get { return OutlinerResources.button_render; }
   //}

   public override TreeNodeLayoutItem Copy()
   {
      RenderableButton newItem = new RenderableButton();

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
