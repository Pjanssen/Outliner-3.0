using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Scene
{
   public class IILayerWrapper : IMaxNodeWrapper
   {
      private IILayer layer;
      public IILayerWrapper(IILayer layer)
      {
         this.layer = layer;
      }

      public override object UnderlyingNode
      {
         get { return this.layer; }
      }

      public override string Name
      {
         get { return layer.Name; }
         set { layer.SetName(ref value); }
      }

      public override int NumChildren
      {
         get { return layer.NumChildren; }
      }

      public override IEnumerable<IMaxNodeWrapper> ChildNodes
      {
         get
         {
            List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>();
            //for (int i = 0; i < this.node.NumberOfChildren; i++)
            //   nodes.Add(IMaxNodeWrapper.Create(this.node.GetChildNode(i)));
            return nodes;
         }
      }

      public override bool CanAddChildNode(IMaxNodeWrapper node)
      {
         if (node is IINodeWrapper)
         {
            IINode n = (IINode)node.UnderlyingNode;
            IILayer l = (IILayer)n.GetReference((int)ReferenceNumbers.NodeLayerRef);
            return this.layer.Handle != l.Handle;
         }
         else
            return false;
      }

      public override IClass_ID ClassID
      {
         get { return layer.ClassID; }
      }

      public override SClass_ID SuperClassID
      {
         get { return layer.SuperClassID; }
      }

      public const String IMGKEY_LAYER        = "layer";
      public const String IMGKEY_LAYER_ACTIVE = "layer_active";
      public override string ImageKey
      {
         get
         {
            return IMGKEY_LAYER;
         }
      }
   }
}
