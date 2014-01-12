using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Scene
{
   /// <summary>
   /// Defines a method for creating an IMaxNode of a specific type.
   /// </summary>
   public interface IMaxNodeFactory
   {
      /// <summary>
      /// Creates a specific instance of IMaxNode for the given base-node.
      /// </summary>
      /// <param name="baseNode">The 3dsmax base node to create an IMaxNode for.</param>
      IMaxNode CreateMaxNode(Object baseNode);
   }
}
