using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.UserFiles
{
   [Flags]
   public enum FilterCategory
   {
      Hidden     = 0x00,
      Classes    = 0x01,
      Properties = 0x02,
      Custom     = 0x04,
      All        = 0xFF
   }
}
