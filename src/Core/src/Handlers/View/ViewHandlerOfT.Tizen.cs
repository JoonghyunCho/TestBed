using System;
using ElmSharp;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using ERect = ElmSharp.Rect;
using ESize = ElmSharp.Size;
using Point = Microsoft.Maui.Graphics.Point;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		bool _disposedValue;

		EvasObject? INativeViewHandler.NativeView => this.GetWrappedNativeView();
		EvasObject? INativeViewHandler.ContainerView => ContainerView;

		public new WrapperView? ContainerView
		{
			get => (WrapperView?)base.ContainerView;
			protected set => base.ContainerView = value;
		}

		public void SetParent(INativeViewHandler parent) => Parent = parent;

		public CoreUIAppContext? Context => MauiContext?.Context;

		public INativeViewHandler? Parent { get; private set; }

		public EvasObject? NativeParent => Context?.BaseLayout;

		~ViewHandler()
		{
			Dispose(disposing: false);
		}

		public override void NativeArrange(Rectangle frame)
		{
			if (NativeParent == null)
				return;

			var nativeView = this.GetWrappedNativeView();

			if (nativeView == null)
				return;

			if (frame.Width < 0 || frame.Height < 0)
			{
				// This is just some initial Forms value nonsense, nothing is actually laying out yet
				return;
			}

			nativeView.UpdateBounds(new Rectangle(ComputeAbsolutePoint(frame), new Size(frame.Width, frame.Height)).ToPixel());
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var nativeView = NativeView;

			if (nativeView == null || VirtualView == null || NativeParent == null)
			{
				return VirtualView == null ? Size.Zero : new Size(VirtualView.Width, VirtualView.Height);
			}

			int availableWidth = widthConstraint.ToScaledPixel();
			int availableHeight = heightConstraint.ToScaledPixel();

			if (availableWidth < 0)
				availableWidth = int.MaxValue;
			if (availableHeight < 0)
				availableHeight = int.MaxValue;

			var explicitWidth = VirtualView.Width;
			var explicitHeight = VirtualView.Height;
			var hasExplicitWidth = explicitWidth >= 0;
			var hasExplicitHeight = explicitHeight >= 0;

			Size measured;
			if (nativeView is IMeasurable nativeViewMeasurable)
			{
				measured = nativeViewMeasurable.Measure(availableWidth, availableHeight).ToDP();
			}
			else
			{
				measured = Measure(availableWidth, availableHeight);
			}

			return new Size(hasExplicitWidth ? explicitWidth : measured.Width,
				hasExplicitHeight ? explicitHeight : measured.Height);
		}

		public virtual ERect GetNativeContentGeometry()
		{
			var nativeView = this.GetWrappedNativeView();

			if (nativeView == null)
			{
				return new ERect();
			}
			return nativeView.Geometry;
		}

		protected virtual Size Measure(double availableWidth, double availableHeight)
		{
			var nativeView = this.GetWrappedNativeView();

			if (nativeView == null)
			{
				return new Size(0, 0);
			}
			return new ESize(nativeView.MinimumWidth, nativeView.MinimumHeight).ToDP();
		}

		protected virtual double ComputeAbsoluteX(Rectangle frame)
		{
			if (Parent != null)
			{
				return frame.X + Parent.GetNativeContentGeometry().X.ToScaledDP();
			}
			else
			{
				return frame.X;
			}
		}

		protected virtual double ComputeAbsoluteY(Rectangle frame)
		{
			if (Parent != null)
			{
				return frame.Y + Parent.GetNativeContentGeometry().Y.ToScaledDP();
			}
			else
			{
				return frame.Y;
			}
		}

		protected virtual Point ComputeAbsolutePoint(Rectangle frame)
		{
			return new Point(ComputeAbsoluteX(frame), ComputeAbsoluteY(frame));
		}

		protected override void SetupContainer()
		{
			var parent = Parent?.NativeView as IContainable<EvasObject>;
			parent?.Children.Remove(NativeView!);

			ContainerView ??= new WrapperView(NativeParent!);
			ContainerView.Show();
			ContainerView.Content = NativeView;

			parent?.Children?.Add(ContainerView);
		}

		protected override void RemoveContainer()
		{
			var parent = Parent?.NativeView as IContainable<EvasObject>;
			parent?.Children.Remove(ContainerView!);

			ContainerView!.Content = null;
			ContainerView?.Unrealize();
			ContainerView = null;

			parent?.Children.Add(NativeView!);
		}

		protected override void OnNativeViewDeleted()
		{
			Dispose();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					(this as IElementHandler)?.DisconnectHandler();
					base.NativeView?.Unrealize();
					ContainerView?.Unrealize();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
