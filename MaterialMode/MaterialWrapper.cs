using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Scene
{
   public class MaterialWrapper : IMaxNodeWrapper
   {
      public IMtl Material { get; private set; }

      public MaterialWrapper(IMtl material)
      {
         Throw.IfArgumentIsNull(material, "material");

         this.Material = material;
      }

      public override int ChildNodeCount
      {
         get { throw new NotImplementedException(); }
      }

      public override IEnumerable<object> ChildNodes
      {
         get 
         {
            IDependentIterator di = MaxInterfaces.Global.DependentIterator.Create(this.Material);
            IReferenceMaker refMaker = null;
            while ((refMaker = di.Next) != null)
            {
               if (refMaker is IINode)
                  yield return refMaker as IINode;
            }
         }
      }

      public override IEnumerable<IMaxNodeWrapper> WrappedChildNodes
      {
         get
         {
            foreach (IMaxNodeWrapper wrapper in ChildNodes.Select(n => new IINodeWrapper(n as IINode)))
            {
               yield return wrapper;
            }

            for (int i = 0; i < this.Material.NumSubMtls; i++)
            {
               yield return new MaterialWrapper(this.Material.GetSubMtl(i));
            }
         }
      }

      public override Autodesk.Max.IClass_ID ClassID
      {
         get { return this.Material.ClassID; }
      }

      public override Autodesk.Max.SClass_ID SuperClassID
      {
         get { return this.Material.SuperClassID; }
      }

      public override bool Equals(object obj)
      {
         throw new NotImplementedException();
      }

      public override int GetHashCode()
      {
         throw new NotImplementedException();
      }

      public override bool IsNodeType(MaxNodeTypes types)
      {
         return (types & MaxNodeTypes.Material) == MaxNodeTypes.Material;
      }

      public override string Name
      {
         get { return this.Material.Name; }
         set { this.Material.Name = value; }
      }

      public override bool Selected
      {
         get { return false; }
      }

      public override object WrappedNode
      {
         get { return this.Material; }
      }

      public override string ImageKey
      {
         get
         {
            return "material";
         }
      }
   }
}
