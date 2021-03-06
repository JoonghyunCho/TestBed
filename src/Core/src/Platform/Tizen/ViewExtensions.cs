using ElmSharp;
using ElmSharp.Accessible;
using Microsoft.Maui.Graphics;
using Tizen.UIExtensions.ElmSharp;
using static Microsoft.Maui.Primitives.Dimension;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		public static void UpdateIsEnabled(this EvasObject nativeView, IView view)
		{
			if (!(nativeView is Widget widget))
				return;

			widget.IsEnabled = view.IsEnabled;
		}

		public static void UpdateVisibility(this EvasObject nativeView, IView view)
		{
			if (view.Visibility.ToNativeVisibility())
			{
				nativeView.Show();
			}
			else
			{
				nativeView.Hide();
			}
		}

		public static bool ToNativeVisibility(this Visibility visibility)
		{
			return visibility switch
			{
				Visibility.Hidden => false,
				Visibility.Collapsed => false,
				_ => true,
			};
		}

		public static void UpdateBackground(this EvasObject nativeView, IView view)
		{
			if (nativeView is IBackgroundCanvas canvas)
			{
				canvas.BackgroundCanvas.Drawable = view.Background?.ToDrawable() ?? null;
			}
			else
			{
				if (view.Background is SolidPaint paint)
				{
					nativeView.UpdateBackgroundColor(paint.Color.ToNative());
				}
			}
		}

		public static void UpdateOpacity(this EvasObject nativeView, IView view)
		{
			if (nativeView is Widget widget)
			{
				widget.Opacity = (int)(view.Opacity * 255.0);
			}
		}

		public static void UpdateClip(this EvasObject nativeView, IView view)
		{
			if (nativeView is WrapperView wrapper)
				wrapper.Clip = view.Clip;
		}

		public static void UpdateAutomationId(this EvasObject nativeView, IView view)
		{
			{
				//TODO: EvasObject.AutomationId is supported from tizen60.
				//nativeView.AutomationId = view.AutomationId;
			}
		}

		public static void UpdateSemantics(this EvasObject nativeView, IView view)
		{
			var semantics = view.Semantics;
			var accessibleObject = nativeView as IAccessibleObject;

			if (semantics == null || accessibleObject == null)
				return;

			accessibleObject.Name = semantics.Description;
			accessibleObject.Description = semantics.Hint;
		}

		public static void InvalidateMeasure(this EvasObject nativeView, IView view)
		{
			nativeView.MarkChanged();
		}

		public static void UpdateWidth(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateHeight(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateMinimumWidth(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateMinimumHeight(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateMaximumWidth(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateMaximumHeight(this EvasObject nativeView, IView view)
		{
			UpdateSize(nativeView, view);
		}

		public static void UpdateSize(EvasObject nativeView, IView view)
		{
			if (!IsExplicitSet(view.Width) || !IsExplicitSet(view.Height))
			{
				// Ignore the initial setting of the value; the initial layout will take care of it
				return;
			}

			// Updating the frame (assuming it's an actual change) will kick off a layout update
			// Handling of the default (-1) width/height will be taken care of by GetDesiredSize
			nativeView.Resize(view.Width.ToScaledPixel(), view.Height.ToScaledPixel());
		}
	}
}
