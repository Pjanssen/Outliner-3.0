using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Scene
{
   public class SelectionSetWrapper : IMaxNodeWrapper
   {
      private IINamedSelectionSetManager manager;
      private Int32 index;

      public SelectionSetWrapper(IINamedSelectionSetManager manager, Int32 index) 
      {
         this.index = index;
         this.manager = manager;
      }

      public override object WrappedNode
      {
         get { return null; }
      }

      public override IEnumerable<IMaxNodeWrapper> ChildNodes
      {
         get
         {
            int numChildren = this.manager.GetNamedSelSetItemCount(this.index);
            List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(numChildren);
            for (int i = 0; i < numChildren; i++)
               nodes.Add(IMaxNodeWrapper.Create(this.manager.GetNamedSelSetItem(this.index, i)));
            return nodes;
         }
      }

      public override string Name
      {
         get { return this.manager.GetNamedSelSetName(this.index); }
         set { this.manager.SetNamedSelSetName(this.index, ref value); }
      }

      public override Autodesk.Max.IClass_ID ClassID
      {
         get { return null; }
      }

      public override Autodesk.Max.SClass_ID SuperClassID
      {
         get { return this.manager.Cd.SuperClassID; }// return 0; }
      }

      public override bool Selected
      {
         get { return false; }
      }



      public const String IMGKEY_SELECTIONSET = "selectionset";
      public override string ImageKey
      {
         get { return IMGKEY_SELECTIONSET; }
      }
   }
}
