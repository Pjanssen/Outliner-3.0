using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Outliner.Plugins;
using Outliner.Presets;
using Outliner.Scene;

namespace Outliner
{
public class OutlinerState
{
   public OutlinerPreset Tree1Preset { get; set; }
   public OutlinerPreset Tree2Preset { get; set; }

   public Boolean Panel1Collapsed { get; set; }
   public Boolean Panel2Collapsed { get; set; }

   public Orientation SplitterOrientation { get; set; }
   public Int32 SplitterDistance { get; set; }

   public OutlinerState()
   {
      this.Tree1Preset = new OutlinerPreset();
      this.Tree2Preset = new OutlinerPreset();

      this.Panel1Collapsed = false;
      this.Panel2Collapsed = true;
      this.SplitterOrientation = Orientation.Horizontal;
      this.SplitterDistance = 200;
   }
}
}
