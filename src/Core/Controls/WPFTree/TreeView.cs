using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PJanssen.Outliner.Controls.WPFTree
{
   public class TreeView : Canvas
   {
      private DependencyProperty ThingsProperty = CreateThingsProperty();

      private static DependencyProperty CreateThingsProperty()
      {
         return DependencyProperty.Register( "Things"
                                           , typeof(Dictionary<string, object>)
                                           , typeof(TreeView)
                                           , new FrameworkPropertyMetadata(new Dictionary<string, object>()));
      }
      
      protected override void OnRender(System.Windows.Media.DrawingContext dc)
      {
         dc.DrawRectangle(Brushes.White, null, new Rect(this.RenderSize));
         dc.DrawText(new FormattedText("text", CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Segoe"), 12, Brushes.Black), new Point(0, 0));
         base.OnRender(dc);
      }

      public Dictionary<string, object> Things
      {
         get
         {
            return (Dictionary<string, object>)GetValue(ThingsProperty);
         }
         set
         {
            SetValue(ThingsProperty, value);
         }
      }
   }
}
