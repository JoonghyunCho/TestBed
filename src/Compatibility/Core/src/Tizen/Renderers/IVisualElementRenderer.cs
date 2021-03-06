using System;
using ElmSharp;
using Microsoft.Maui.Controls.Platform;
using ERect = ElmSharp.Rect;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Tizen
{
	/// <summary>
	/// Base interface for VisualElement renderer.
	/// </summary>
	public interface IVisualElementRenderer : IRegisterable, IDisposable
	{
		/// <summary>
		/// Gets the VisualElement associated with this renderer.
		/// </summary>
		/// <value>The VisualElement.</value>
		VisualElement Element
		{
			get;
		}

		/// <summary>
		/// Gets the native view associated with this renderer.
		/// </summary>
		/// <value>The native view.</value>
		EvasObject NativeView
		{
			get;
		}

		event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		/// <summary>
		/// Sets the VisualElement associated with this renderer.
		/// </summary>
		/// <param name="element">New element.</param>
		void SetElement(VisualElement element);

		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);

		void UpdateLayout();

		ERect GetNativeContentGeometry();
	}
}
