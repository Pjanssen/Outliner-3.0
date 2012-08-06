﻿using System;
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
      public RenderableButton() 
         : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Renderable)) { }

      protected override MaxUtils.AnimatableProperty Property
      {
         get { return MaxUtils.AnimatableProperty.Renderable; }
      }

      protected override string ToolTipEnabled
      {
         get { return OutlinerResources.Tooltip_Renderable; }
      }

      protected override Image ImageEnabled
      {
         get { return OutlinerResources.button_render; }
      }
   }
}
