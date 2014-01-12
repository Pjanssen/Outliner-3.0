using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree.Layout;
using System.Xml.Serialization;
using System.IO;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner;

namespace TestForm
{
   public partial class Form2 : System.Windows.Forms.Form
   {
      private TreeNode tnA;
      private TreeNode tnB;
      private TreeNode tnC;

      public Form2()
      {
         InitializeComponent();

         this.treeView1.BeginUpdate();

         tnA = new TreeNode("testA");
         //tnA.ShowNode = false;
         this.treeView1.Nodes.Add(tnA);

         tnB = new TreeNode("childA");
         //this.tnB.ShowNode = false;
         tnA.Nodes.Add(tnB);
         
         tnC = new TreeNode("childB");
         tnB.Nodes.Add(tnC);

         this.treeView1.EndUpdate();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         this.treeView1.BeginUpdate();
         this.tnA.ShowNode = !this.tnA.ShowNode;
         this.tnB.ShowNode = !this.tnB.ShowNode;
         this.tnC.ShowNode = !this.tnC.ShowNode;
         this.treeView1.EndUpdate();
      }
   }
}
