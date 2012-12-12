using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Configuration;

namespace Outliner.Controls.Options
{
   public partial class SorterConfigurationEditor : OutlinerUserControl
   {
      public SorterConfigurationEditor()
      {
         InitializeComponent();
      }

      public SorterConfigurationEditor(SorterConfiguration config) : this() 
      {
         
      }
   }
}
