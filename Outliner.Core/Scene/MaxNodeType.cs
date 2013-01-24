using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Outliner.Scene
{

[Flags]
public enum MaxNodeType
{
   None         = 0x00,

   //Object refers to IINodes, but this name could be a bit confusing to the user.
   Object       = 0x01, 
   Layer        = 0x02,
   Material     = 0x04,
   SelectionSet = 0x08,
   TextureMap   = 0x10,

   All          = Object | Layer | Material | SelectionSet | TextureMap
}

}
