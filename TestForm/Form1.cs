using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace TestForm
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
      }

      private const int CS_DROPSHADOW = 0x00020000;

      // Override the CreateParams property
      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams cp = base.CreateParams;
            cp.ClassStyle |= CS_DROPSHADOW;
            return cp;
         }
      }
   }

   [DataContract]
   public class SerializeTest
   {
      [DataMember]
      private String name;

      public SerializeTest()
      {
         this.name = "Test";
      }
   }
}
