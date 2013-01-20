using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Outliner.Configuration;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.Filters;

namespace Outliner
{
public class OutlinerState
{
   public String Tree1PresetName { get; set; }
   public MaxNodeFilterCombinator Tree1Filters { get; set; }

   public String Tree2PresetName { get; set; }
   public MaxNodeFilterCombinator Tree2Filters { get; set; }

   private OutlinerPreset GetPresetByName(String name)
   {
      return ConfigurationHelpers.GetConfigurations<OutlinerPreset>(OutlinerPaths.PresetsDir)
                                 .FirstOrDefault(p => p.Text == name);
   }

   [XmlIgnore]
   public OutlinerPreset Tree1Preset 
   { 
      get { return GetPresetByName(this.Tree1PresetName); }
      set { this.Tree1PresetName = value.Text; }
   }

   [XmlIgnore]
   public OutlinerPreset Tree2Preset 
   {
      get { return GetPresetByName(this.Tree2PresetName); }
      set { this.Tree2PresetName = value.Text; }
   }

   public Boolean Panel1Collapsed { get; set; }
   public Boolean Panel2Collapsed { get; set; }

   public Orientation SplitterOrientation { get; set; }
   public Int32 SplitterDistance { get; set; }

   public OutlinerState()
   {
      this.Tree1PresetName = "";
      this.Tree2PresetName = "";

      this.Tree1Filters = new MaxNodeFilterCombinator();
      this.Tree1Filters.Enabled = false;
      this.Tree2Filters = new MaxNodeFilterCombinator();
      this.Tree2Filters.Enabled = false;

      this.Panel1Collapsed = false;
      this.Panel2Collapsed = true;
      this.SplitterOrientation = Orientation.Horizontal;
      this.SplitterDistance = 200;
   }
}
}
