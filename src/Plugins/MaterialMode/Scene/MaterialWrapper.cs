using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   /// <summary>
   /// Defines a MaxNodeWrapper for an IMtl object.
   /// </summary>
   public class MaterialWrapper : MaxNodeWrapper
   {
      public IMtl Material { get; private set; }

      /// <summary>
      /// Initializes a new instance of the MaterialWrapper class.
      /// </summary>
      /// <param name="material">The IMtl object to wrap.</param>
      public MaterialWrapper(IMtl material)
      {
         Throw.IfNull(material, "material");
         
         this.Material = material;
      }

      public override object BaseObject
      {
         get { return this.Material; }
      }


      #region Equality

      public override Boolean Equals(Object obj)
      {
         MaterialWrapper wrapper = obj as MaterialWrapper;
         return wrapper != null && this.Material.Handle == wrapper.Material.Handle;
      }

      public override int GetHashCode()
      {
         return this.Material.GetHashCode();
      }

      #endregion


      #region Delete

      override public Boolean CanDelete
      {
         get { return true; }
      }

      override public void Delete() 
      {
         throw new NotImplementedException();
      }

      #endregion


      #region Childnodes

      public override int ChildNodeCount
      {
         get
         {
            return this.ChildBaseObjects.Count();
         }
      }

      public IEnumerable<IINode> ChildINodes
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

      public IEnumerable<IMtl> ChildMaterials
      {
         get
         {
            for (int i = 0; i < this.Material.NumSubMtls; i++)
            {
               yield return this.Material.GetSubMtl(i);
            }
         }
      }

      public IEnumerable<ITexmap> ChildTextureMaps
      {
         get
         {
            for (int i = 0; i < this.Material.NumSubTexmaps; i++)
            {
               ITexmap texMap = this.Material.GetSubTexmap(i);
               if (texMap != null)
                  yield return texMap;
            }
         }
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            //Assigned nodes.
            foreach (IINode inode in ChildINodes)
            {
               yield return inode;
            }

            //Submaterials
            for (int i = 0; i < this.Material.NumSubMtls; i++)
            {
               yield return this.Material.GetSubMtl(i);
            }

            //SubTexmaps
            foreach (ITexmap texmap in ChildTextureMaps)
            {
               yield return texmap;
            }
         }
      }

      public override bool CanAddChildNode(IMaxNode node)
      {
         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper == null)
            return false;

         IINode inode = inodeWrapper.INode;

         return inode.Mtl == null || !inode.Mtl.Handle.Equals(this.Material.Handle);
      }

      public override void AddChildNode(IMaxNode node)
      {
         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper == null)
            return;

         inodeWrapper.INode.Mtl = this.Material;
      }

      public override bool CanRemoveChildNode(IMaxNode node)
      {
         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper == null)
            return false;

         IINode inode = inodeWrapper.INode;
         return inode.Mtl != null && inode.Mtl.Handle.Equals(this.Material.Handle);
      }

      public override void RemoveChildNode(IMaxNode node)
      {
         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper == null)
            return;

         inodeWrapper.INode.Mtl = null;
      }

      #endregion


      #region Node Type

      override public SClass_ID SuperClassID
      {
         get { return this.Material.SuperClassID; }
      }

      override public IClass_ID ClassID
      {
         get { return this.Material.ClassID; }
      }

      override protected MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.Material; }
      }

      #endregion


      #region Name

      override public String Name
      {
         get { return this.Material.Name; }
         set { this.Material.Name = value; }
      }

      override public Boolean CanEditName
      {
         get { return true; }
      }

      #endregion


      #region Node Properties

      #endregion


      #region ImageKey

      override public String ImageKey
      {
         get { return "material"; }
      }

      #endregion


      public override String ToString()
      {
         return "MaterialWrapper (" + this.Name + ")";
      }
   }
}
