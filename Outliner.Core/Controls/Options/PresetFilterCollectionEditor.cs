using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Options
{
   public partial class PresetFilterCollectionEditor : UserControl
   {
      public PresetFilterCollectionEditor()
      {
         InitializeComponent();
      }

      public PresetFilterCollectionEditor( FilterCombinator<IMaxNodeWrapper> rootFilter
                                         , Action updateAction)
         : this()
      {
         this.filterEditor.RootFilter = rootFilter;
         this.filterEditor.UpdateAction = updateAction;
      }
   }
}
