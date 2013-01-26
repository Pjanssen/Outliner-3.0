using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode.Scene
{
   public class MaterialObjectsWrapper : MaxNodeWrapper
   {
      public IMtl Material { get; private set; }

      public MaterialObjectsWrapper(IMtl material)
      {
         this.Material = material;
      }

      public override object BaseObject
      {
         get { return this.Material; }
      }

      public override bool Equals(object obj)
      {
         throw new NotImplementedException();
      }

      public override int GetHashCode()
      {
         throw new NotImplementedException();
      }

      protected override MaxNodeType MaxNodeType
      {
         get { return (MaxNodeType)0; }
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            return base.ChildBaseObjects;
         }
      }
   }
}
