using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using Outliner.Controls;
using Autodesk.Max;

namespace Outliner
{
   //These values do not seem to be defined in Autodesk.Max.dll,
   //so they are copied from gup.h :
   public enum GupResult : uint
   {
      Keep = 0x00,
      NoKeep = 0x01,
      Abort = 0x03
   }

   public class OutlinerGUP : GUP
   {
      public override uint Start
      {
         get { return (uint)GupResult.Keep; }
      }

      public override void Stop() { }

      private MainWindow mainWindow;
      public Boolean MainWindowOpen { get; private set; }

      public void OpenMainWindow() 
      {
         if (!MainWindowOpen)
         {
            //if (this.mainWindow == null)
               this.mainWindow = new MainWindow();

            this.mainWindow.FormClosed += new System.Windows.Forms.FormClosedEventHandler(mainWindow_FormClosed);
            this.mainWindow.ShowModeless();

            this.MainWindowOpen = true;
         }
      }

      void mainWindow_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
      {
         this.mainWindow.FormClosed -= mainWindow_FormClosed;
         this.MainWindowOpen = false;
      }

      public void CloseMainWindow() 
      {
         if (this.mainWindow != null)
            this.mainWindow.Close();

         this.MainWindowOpen = false;
      }
   }
}
