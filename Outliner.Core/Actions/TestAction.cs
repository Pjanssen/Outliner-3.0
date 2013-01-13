using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Commands;
using Outliner.Controls.Tree;
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
         CreateNewLayerCommand cmd = new CreateNewLayerCommand();
         cmd.Execute(false);
      }

   }
}
