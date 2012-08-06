﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Tree.Layout
{
   public class BoxModeButton : AnimatablePropertyButton
   {
      public BoxModeButton() 
         : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.BoxMode)) { }

      protected override MaxUtils.AnimatableProperty Property
      {
         get { return MaxUtils.AnimatableProperty.BoxMode; }
      }

      protected override string ToolTipEnabled
      {
         get { return OutlinerResources.Tooltip_BoxMode; }
      }

      protected override System.Drawing.Image ImageEnabled
      {
         get { return OutlinerResources.button_boxmode; }
      }
   }
}
