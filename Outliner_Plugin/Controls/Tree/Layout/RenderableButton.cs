using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;
using System.Drawing;

namespace Outliner.Controls.Tree.Layout
{
   public class RenderableButton : AnimatablePropertyButton
   {
      public RenderableButton() { }

      protected override MaxUtils.AnimatableProperty Property
      {
         get { return MaxUtils.AnimatableProperty.Renderable; }
      }

      protected override Type SetPropertyCommandType
      {
         get { return typeof(SetRenderableCommand); }
      }

      protected override string ToolTipEnabled
      {
         get { return OutlinerResources.Tooltip_Renderable; }
      }

      protected override Bitmap ImageEnabled
      {
         get { return OutlinerResources.button_render; }
      }
   }
}
