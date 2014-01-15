using PJanssen.Outliner.UpdateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PJanssen.Outliner.UpdateClient
{
   /// <summary>
   /// Interaction logic for UpdateDialog.xaml
   /// </summary>
   public partial class UpdateDialog : Window
   {
      private UpdateData updateData;

      public UpdateDialog()
      {
         InitializeComponent();
      }

      public UpdateData UpdateData
      {
         get 
         {
            return this.updateData;
         }
         set 
         {
            this.updateData = value;
            this.DataContext = value;
         }
      }

      private void RemindBtn_Click(object sender, RoutedEventArgs e)
      {
         this.Close();
      }

      private void SkipBtn_Click(object sender, RoutedEventArgs e)
      {
         UpdateSettings.SkippedVersion = this.UpdateData.NewVersion;
         this.Close();
      }
   }
}
