using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Modes.SelectionSet;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class CreateSelectionSetCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private int? selSetNum;

      public CreateSelectionSetCommand() : this(Enumerable.Empty<IMaxNode>()) { }
      public CreateSelectionSetCommand(IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");

         this.nodes = nodes.ToList();
      }

      public override string Description
      {
         get
         {
            if (this.nodes.Count() == 0)
               return Resources.Command_CreateNewSelSet;
            else
               return Resources.Command_AddToNewSelSet;
         }
      }

      public override void Do()
      {
         IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
         INameMaker nameMaker = MaxInterfaces.COREInterface.NewNameMaker(false);
         for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
         {
            String selSetName = selSetMan.GetNamedSelSetName(i);
            nameMaker.AddName(ref selSetName);
         }
         String newName = "New Set ";
         nameMaker.MakeUniqueName(ref newName);

         ITab<IINode> nodeTab = HelperMethods.ToIINodeTab(this.nodes);
         if (MaxInterfaces.SelectionSetManager.AddNewNamedSelSet(nodeTab, ref newName))
         {
            this.selSetNum = MaxInterfaces.SelectionSetManager.NumNamedSelSets - 1;
         }
      }

      public SelectionSetWrapper CreatedSelectionSet
      {
         get
         {
            if (selSetNum != null)
               return new SelectionSetWrapper(this.selSetNum.Value);
            else
               return null;
         }
      }
   }
}
