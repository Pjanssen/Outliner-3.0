using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   /// <summary>
   /// IMaxNode provides a generic interface to any 3dsMax scene node.
   /// </summary>
   public interface IMaxNode
   {
      /// <summary>
      /// Gets the base object of this IMaxNode.
      /// </summary>
      Object BaseObject { get; }

      /// <summary>
      /// Gets or sets the parent node of this IMaxNode.
      /// </summary>
      IMaxNode Parent { get; set; }

      /// <summary>
      /// Gets or sets the selected state of this IMaxNode.
      /// </summary>
      Boolean IsSelected { get; set; }

      /// <summary>
      /// Tests if the base object is still a valid scene node and hasn't been deleted.
      /// </summary>
      Boolean IsValid { get; }


      #region Delete
      
      /// <summary>
      /// Tests if this IMaxNode can be deleted from the scene.
      /// </summary>
      Boolean CanDelete { get; }

      /// <summary>
      /// Deletes this IMaxNode from the scene.
      /// </summary>
      void Delete();

      #endregion


      #region Childnodes

      /// <summary>
      /// Returns the number of child nodes.
      /// </summary>
      Int32 ChildNodeCount { get; }

      /// <summary>
      /// Returns the BaseObject children of this IMaxNode.
      /// </summary>
      IEnumerable<Object> ChildBaseObjects { get; }

      /// <summary>
      /// Returns the children of this IMaxNode.
      /// </summary>
      IEnumerable<IMaxNode> ChildNodes { get; }

      /// <summary>
      /// Tests whether the given node can be added as a child.
      /// </summary>
      /// <param name="node">The node to be added.</param>
      Boolean CanAddChildNode(IMaxNode node);

      /// <summary>
      /// Tests whether any of the given nodes can be added as a child.
      /// </summary>
      /// <param name="nodes">The nodes to be added.</param>
      Boolean CanAddChildNodes(IEnumerable<IMaxNode> nodes);

      /// <summary>
      /// Adds the given node as a child node.
      /// </summary>
      /// <param name="node">The node to add.</param>
      void AddChildNode(IMaxNode node);

      /// <summary>
      /// Adds all the given nodes which can be added as a child.
      /// </summary>
      /// <param name="nodes">The nodes to be added.</param>
      void AddChildNodes(IEnumerable<IMaxNode> nodes);

      /// <summary>
      /// Tests whether the given (child-)node can be removed.
      /// </summary>
      /// <param name="node">The node to be removed.</param>
      Boolean CanRemoveChildNode(IMaxNode node);

      /// <summary>
      /// Tests whether any of the given (child-)nodes can be removed.
      /// </summary>
      /// <param name="nodes">The nodes to be removed.</param>
      Boolean CanRemoveChildNodes(IEnumerable<IMaxNode> nodes);

      /// <summary>
      /// Removes the given (child-)node.
      /// </summary>
      /// <param name="node">The node to be removed.</param>
      void RemoveChildNode(IMaxNode node);

      /// <summary>
      /// Removes all the given nodes which can be removed.
      /// </summary>
      /// <param name="nodes">The nodes to be removed.</param>
      void RemoveChildNodes(IEnumerable<IMaxNode> nodes);

      #endregion


      #region Node Type

      /// <summary>
      /// Gets the SuperClass ID of this IMaxNode.
      /// </summary>
      SClass_ID SuperClassID { get; }

      /// <summary>
      /// Gets the Class ID of this IMaxNode.
      /// </summary>
      IClass_ID ClassID { get; }

      /// <summary>
      /// Tests if this IMaxNode is of the given MaxNodeType.
      /// </summary>
      /// <param name="types">The MaxNodeType flags to test.</param>
      Boolean IsNodeType(MaxNodeType types);

      #endregion


      #region Name

      /// <summary>
      /// The name of this IMaxNode.
      /// </summary>
      String Name { get; set; }

      /// <summary>
      /// Tests if the name of this IMaxNode can be edited.
      /// </summary>
      Boolean CanEditName { get; }

      /// <summary>
      /// The name of this IMaxNode formatted for displaying in the UI.
      /// </summary>
      String DisplayName { get; }

      #endregion


      #region Node Properties

      /// <summary>
      /// Gets the value of the given NodeProperty.
      /// </summary>
      Object GetNodeProperty(NodeProperty property);

      /// <summary>
      /// Gets the value of the given BooleanNodeProperty.
      /// </summary>
      Boolean GetNodeProperty(BooleanNodeProperty property);

      /// <summary>
      /// Sets the value of the given NodeProperty.
      /// </summary>
      void SetNodeProperty(NodeProperty property, Object value);

      /// <summary>
      /// Sets the value of the given BooleanNodeProperty.
      /// </summary>
      void SetNodeProperty(BooleanNodeProperty property, Boolean value);

      /// <summary>
      /// Returns true if the supplied property is inherited from another object.
      /// </summary>
      Boolean IsNodePropertyInherited(NodeProperty property);

      /// <summary>
      /// Returns true if the supplied property is inherited from another object.
      /// </summary>
      Boolean IsNodePropertyInherited(BooleanNodeProperty property);

      /// <summary>
      /// The color of the IMaxNode.
      /// </summary>
      Color WireColor { get; set; }

      #endregion

      /// <summary>
      /// Gets the name of the Image to be displayed in the UI for this IMaxNode.
      /// </summary>
      String ImageKey { get; }
   }
}
