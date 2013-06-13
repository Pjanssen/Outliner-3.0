using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outliner.Scene
{
   public interface IXRefRecord
   {
      Boolean Enabled { get; set; }
      Boolean AutoUpdate { get; set; }

      void Update();

      String Filename { get; }
   }
}
