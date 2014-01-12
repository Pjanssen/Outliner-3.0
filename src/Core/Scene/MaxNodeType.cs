using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PJanssen.Outliner.Scene
{

/// <summary>
/// Defines types of 3dsMax nodes, which can be used to filter node sets.
/// </summary>
[Flags]
public enum MaxNodeType
{
   /// <summary>
   /// Matches no types.
   /// </summary>
   None         = 0x00,

   /// <summary>
   /// Specifies that the node is an object (INode).
   /// </summary>
   Object       = 0x01, 

   /// <summary>
   /// Specifies that the node is a layer.
   /// </summary>
   Layer        = 0x02,

   /// <summary>
   /// Specifies that the node is a material.
   /// </summary>
   Material     = 0x04,

   /// <summary>
   /// Specifies that the node is a selection-set.
   /// </summary>
   SelectionSet = 0x08,

   /// <summary>
   /// Specifies that the node is a texture map.
   /// </summary>
   TextureMap   = 0x10,

   /// <summary>
   /// Specifies that the node is an xref scene record.
   /// </summary>
   XRefSceneRecord = 0x20,

   /// <summary>
   /// Specifies that the node is an xref object record.
   /// </summary>
   XRefObjectRecord = 0x40,

   XRefObject = 0x80,

   XRefMaterial = 0x100,

   XRefController = 0x200,

   XRefRecord = XRefSceneRecord | XRefObjectRecord,

   XRefs = XRefSceneRecord | XRefObjectRecord | XRefObject | XRefMaterial | XRefController,

   /// <summary>
   /// Matches all types.
   /// </summary>
   All = Object | Layer | Material | SelectionSet | TextureMap | XRefs
}

}
