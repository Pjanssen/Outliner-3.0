using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Scene
{
   public class TextureMapWrapper : MaxNodeWrapper
   {
      public ITexmap TextureMap { get; private set; }

      public TextureMapWrapper(ITexmap textureMap)
      {
         this.TextureMap = textureMap;
      }

      public override object BaseObject
      {
         get { return this.TextureMap; }
      }


      #region Equality
      
      public override bool Equals(object obj)
      {
         throw new NotImplementedException();
      }

      public override int GetHashCode()
      {
         throw new NotImplementedException();
      }

      #endregion


      #region ChildNodes

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            for (int i = 0; i < this.TextureMap.NumSubTexmaps; i++)
            {
               ITexmap texmap = this.TextureMap.GetSubTexmap(i);
               if (texmap != null)
                  yield return texmap;
            }
         }
      }

      #endregion


      #region Type

      protected override MaxNodeType MaxNodeType
      {
         get { return Scene.MaxNodeType.TextureMap; }
      }

      public override IClass_ID ClassID
      {
         get { return this.TextureMap.ClassID; }
      }

      public override SClass_ID SuperClassID
      {
         get { return this.TextureMap.SuperClassID; }
      }

      #endregion


      #region Name

      public override bool CanEditName
      {
         get { return true; }
      }

      public override string Name
      {
         get { return this.TextureMap.Name; }
         set { this.TextureMap.Name = value; }
      }

      public override string DisplayName
      {
         get
         {
            return this.Name + " (" + this.TextureMap.ClassName + ")";
         }
      }

      #endregion


      #region ImageKey

      public override string ImageKey
      {
         get
         {
            if (this.TextureMap.ClassName == "Bitmap")
               return "bitmap";

            if (this.TextureMap.ClassName.StartsWith("Gradient"))
               return "gradient";

            return "checker";
         }
      }

      #endregion


      public override string ToString()
      {
         return "TextureMapWrapper (" + this.Name + ")";
      }
   }
}
