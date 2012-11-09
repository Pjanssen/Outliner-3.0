using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Filters;
using Outliner.Scene;
using MaxCustomControls;

namespace Outliner.Controls.Options
{
   public partial class AdvancedFilterEditor : MaxForm
   {
      public AdvancedFilterEditor()
      {
         InitializeComponent();
      }

      public AdvancedFilterEditor(FilterCombinator<IMaxNodeWrapper> rootFilter)
         : this()
      {
         this.filterCollectionEditor1.RootFilter = rootFilter;
      }

      private void closeBtn_Click(object sender, EventArgs e)
      {
         this.Close();
      }
   }
}
