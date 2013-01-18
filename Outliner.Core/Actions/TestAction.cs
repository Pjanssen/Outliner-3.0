using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Commands;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using Outliner.Scene;
using UiViewModels.Actions;

namespace Outliner.Actions
{
   public class TestAction : CuiActionCommandAdapter
   {
      public override string ActionText
      {
         get { return "Test"; }
      }

      public override string Category
      {
         get { return OutlinerResources.Action_Category; }
      }

      public override string InternalActionText
      {
         get { return ActionText; }
      }

      public override string InternalCategory
      {
         get { return OutlinerActions.InternalCategory; }
      }

      public override void Execute(object parameter)
      {
         IHold theHold = MaxInterfaces.Global.TheHold;
         theHold.Begin();

         MaxScene.AllObjects.ForEach(n => n.Selected = true);
         
         theHold.Accept("Test!");

         IInterface ip = MaxInterfaces.Global.COREInterface;
         ip.RedrawViews(ip.Time, RedrawFlags.Normal, null);
      }

   }
}
