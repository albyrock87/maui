using System;
using CoreGraphics;
using Microsoft.Maui.Graphics;
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.LayoutView;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.LayoutViewGroup;
#elif WINDOWS
using PlatformView = Microsoft.Maui.Platform.LayoutPanel;
#elif TIZEN
using PlatformView = Microsoft.Maui.Platform.LayoutViewGroup;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers.Layout;

class HeadlessLayoutHandler : ElementHandler, IViewHandler
{
	public static CommandMapper<ILayout, ILayoutHandler> CommandMapper = new()
	{
		// [nameof(ILayoutHandler.Add)] = MapAdd,
		// [nameof(ILayoutHandler.Remove)] = MapRemove,
		// [nameof(ILayoutHandler.Clear)] = MapClear,
		// [nameof(ILayoutHandler.Insert)] = MapInsert,
		// [nameof(ILayoutHandler.Update)] = MapUpdate,
		// [nameof(ILayoutHandler.UpdateZIndex)] = MapUpdateZIndex,
		// [nameof(IView.InvalidateMeasure)] = MapInvalidateMeasure,
		// [nameof(IView.Frame)] = MapFrame,
		// [nameof(IView.ZIndex)] = MapZIndex,
	};

	public static PropertyMapper<ILayout, ILayoutHandler> ViewMapper = new()
	{
		// [nameof(IView.FlowDirection)] = MapFlowDirection,
		// [nameof(IView.Width)] = MapWidth,
		// [nameof(IView.Height)] = MapHeight,
		// [nameof(IView.MinimumHeight)] = MapMinimumHeight,
		// [nameof(IView.MaximumHeight)] = MapMaximumHeight,
		// [nameof(IView.MinimumWidth)] = MapMinimumWidth,
		// [nameof(IView.MaximumWidth)] = MapMaximumWidth,
	};
	
	public HeadlessLayoutHandler(PlatformView containerView, ILayout compressedLayout)
		: base(ViewMapper, CommandMapper)
	{
		ConcretePlatformView = containerView;
		VirtualView = compressedLayout;
	}

	public PlatformView ConcretePlatformView { get; }
	public new ILayout VirtualView { get; }

	public bool HasContainer { get; set; }
	public object? ContainerView => null;
	
	IView IViewHandler.VirtualView => VirtualView;

	public Size GetDesiredSize(double widthConstraint, double heightConstraint)
	{
		var virtualView = VirtualView;

		widthConstraint = Math.Min(widthConstraint, virtualView.MaximumWidth);
		heightConstraint = Math.Min(heightConstraint, virtualView.MaximumHeight);

		var crossPlatformMeasure = virtualView.CrossPlatformMeasure(widthConstraint, heightConstraint);

		return crossPlatformMeasure;
	}

	public void PlatformArrange(Rect frame)
	{
		var virtualView = VirtualView;
		virtualView.CrossPlatformArrange(frame);
	}
	
	private protected override object OnCreatePlatformElement() => ConcretePlatformView;

	private protected override void OnConnectHandler(object platformView)
	{
	}

	private protected override void OnDisconnectHandler(object platformView)
	{
	}
}