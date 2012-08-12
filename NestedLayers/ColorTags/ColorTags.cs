using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using MaxUtils;

namespace Outliner.LayerTools
{
public static class ColorTags
{
   private const uint CID_A = 0x68D53413;
   private const uint CID_B = 0x42B40170;
   private static IClass_ID classID;

   public const byte NoTag = 0;
   public const byte UseWireColor = Byte.MaxValue;
   public const SystemNotificationCode TagChanged = (SystemNotificationCode)0x00000200;
   
   private static readonly List<Tuple<uint, String, Color>> colors = new List<Tuple<uint, string, Color>>()
   {
        new Tuple<uint, String, Color>(0x588B5E2D, "Tag 1", Color.FromArgb(230, 96, 93))
      , new Tuple<uint, String, Color>(0x3663775D, "Tag 2", Color.FromArgb(241, 169, 74))
      , new Tuple<uint, String, Color>(0x011713D3, "Tag 3", Color.FromArgb(239, 220, 80))
      , new Tuple<uint, String, Color>(0x4C600228, "Tag 4", Color.FromArgb(179, 217, 79))
      , new Tuple<uint, String, Color>(0x51D06AFF, "Tag 5", Color.FromArgb(92, 162, 250))
      , new Tuple<uint, String, Color>(0x385D0131, "Tag 6", Color.FromArgb(193, 140, 215))
      , new Tuple<uint, String, Color>(0x4F014CFB, "Tag 7", Color.FromArgb(169, 169, 169))
   };

   internal static void Start(IGlobal global)
   {
      ColorTags.classID = global.Class_ID.Create(CID_A, CID_B);

      IIColorManager colorMan = MaxInterfaces.Global.ColorManager;

      foreach (Tuple<uint, String, Color> color in ColorTags.colors)
      {
         colorMan.RegisterColor(color.Item1, color.Item2, "Outliner", color.Item3);
      }
   }


   /// <summary>
   /// Returns true if the supplied node has a tag set on it.
   /// </summary>
   public static Boolean HasTag(IAnimatable node)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      byte tag = ColorTags.GetTag(node);
      return tag != ColorTags.NoTag;
   }

   /// <summary>
   /// Gets the tag index for the supplied node.
   /// </summary>
   public static byte GetTag(IAnimatable node)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      IAppDataChunk chunk = node.GetAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);
      if (chunk == null || chunk.Data == null || chunk.Data.Length == 0)
         return ColorTags.NoTag;
      else
         return chunk.Data[0];
   }

   /// <summary>
   /// Gets the tag (or wire-) color of the supplied node.
   /// </summary>
   public static Color GetColor(IAnimatable node) 
   {
      if (node == null)
         throw new ArgumentNullException("node");

      byte tag = ColorTags.GetTag(node);

      if (tag == ColorTags.NoTag)
         return Color.Empty;
      else if (tag == ColorTags.UseWireColor)
         return ColorTags.getWireColor(node);
      else
         return ColorTags.getTagColor(tag);
   }

   /// <summary>
   /// Sets the tag index on the supplied node.
   /// </summary>
   /// <param name="node">The node to tag.</param>
   /// <param name="tagIndex">The 1-based tag index.</param>
   public static void SetTag(IAnimatable node, byte tag)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      node.RemoveAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);

      if (tag != ColorTags.NoTag)
      {
         byte[] data = new byte[1] { tag };
         node.AddAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0, data);
      }

      MaxInterfaces.Global.BroadcastNotification(ColorTags.TagChanged, node);
   }

   /// <summary>
   /// Clears the color tag from the supplier node.
   /// </summary>
   public static void RemoveTag(IAnimatable node)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      node.RemoveAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);

      MaxInterfaces.Global.BroadcastNotification(ColorTags.TagChanged, node);
   }


   private static Color getTagColor(byte index)
   {
      if (index < 1)
         throw new ArgumentOutOfRangeException("index");

      if (index > ColorTags.colors.Count)
         return Color.Empty;

      IIColorManager colorMan = MaxInterfaces.Global.ColorManager;
      Color color = colorMan.GetColor((GuiColors)ColorTags.colors[index - 1].Item1);
      return ColorHelpers.FromMaxColor(color);
   }

   private static Color getWireColor(IAnimatable node)
   {
      Color color = Color.Empty;

      if (node is IINode)
         color = ((IINode)node).WireColor;
      else if (node is IILayer)
         color = ((IILayer)node).WireColor;

      return ColorHelpers.FromMaxColor(color);
   }
}
}
