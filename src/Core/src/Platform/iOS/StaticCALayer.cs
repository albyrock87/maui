using CoreAnimation;

namespace Microsoft.Maui.Platform;

class StaticCALayer : CALayer, IAutoSizedCALayer
{
	public override void AddAnimation(CAAnimation animation, string? key)
	{
		// Do nothing, we don't want animations here
	}
}