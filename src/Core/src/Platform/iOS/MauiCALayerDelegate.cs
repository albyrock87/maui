using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Platform;

class MauiCALayerDelegate : NSObject
{
	public static void EnsureAttached(CALayer layer)
	{
		var layerDelegate = layer.Delegate;
		var view = new UIView();
		view.ob
		if (layerDelegate is not MauiCALayerDelegate)
		{
			layer.Delegate = new MauiCALayerDelegate(layerDelegate);
		}
	}

	readonly WeakReference<ICALayerDelegate> _delegateRef;

	private MauiCALayerDelegate(ICALayerDelegate originalDelegate)
	{
		_delegateRef = new WeakReference<ICALayerDelegate>(originalDelegate);
	}

	public override bool RespondsToSelector(Selector? sel)
	{
		if (sel is not null && _delegateRef.TryGetTarget(out var originalDelegate) && originalDelegate is NSObject nsObject)
		{
			if (sel.Name == "layoutSublayersOfLayer:")
			{
				
			}

			return nsObject.RespondsToSelector(sel);
		}

		return false;
	}
}