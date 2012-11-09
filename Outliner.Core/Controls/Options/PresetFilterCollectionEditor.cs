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

      public PresetFilterCollectionEditor( PresetEditor owningEditor
                                         , FilterCombinator<IMaxNodeWrapper> rootFilter)
         : this()
      {
         this.filterEditor.OwningEditor = owningEditor;
         this.filterEditor.RootFilter = rootFilter;
      }
   }
}
