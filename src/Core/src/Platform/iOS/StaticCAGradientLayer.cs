using CoreAnimation;

namespace Microsoft.Maui.Platform;

class StaticCAGradientLayer : CAGradientLayer, IAutoSizedCALayer
{
	public override void AddAnimation(CAAnimation animation, string? key)
	{
		// Do nothing, we don't want animations here
	}
}