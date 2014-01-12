using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Scene;
using UiViewModels.Actions;

namespace PJanssen.Outliner.Actions
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
         MaxscriptListenerLogger logger = new MaxscriptListenerLogger("Outliner");
         logger.Debug("test");
         logger.Info("some information goes here");
         logger.Warning("this is your last warning!");
         logger.Error("Oh noeesss!");
      }

   }
}
