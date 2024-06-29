using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Maui.Handlers
{
	internal static class LayoutExtensions
	{
		public static IEnumerable<IView> EnumeratePlatformChildren(this ILayout layout)
		{
			foreach (var view in layout.OrderByZIndex())
			{
				yield return view;
				
				if (view is IHeadlessLayout { IsHeadless: true } headlessLayout)
				{
					foreach (var child in headlessLayout.EnumeratePlatformChildren())
					{
						yield return child;
					}
				}
			}
		}
		
		public static IOrderedEnumerable<IView> OrderByZIndex(this ILayout layout)
		{
			return layout.OrderBy(v => v.ZIndex);
		}

		public static int GetLayoutHandlerIndex(this ILayout layout, IView view)
		{
			var count = layout.Count;
			switch (count)
			{
				case 0:
					return -1;
				case 1:
					return view == layout[0] ? 0 : -1;
				default:
					var found = false;
					var zIndex = view.ZIndex;
					var lowerViews = 0;

					for (int i = 0; i < count; i++)
					{
						var child = layout[i];
						var childZIndex = child.ZIndex;

						if (child == view)
						{
							found = true;
						}

						if (childZIndex < zIndex || !found && childZIndex == zIndex)
						{
							lowerViews = child is IHeadlessLayout { IsHeadless: true } headlessChild
								? lowerViews + headlessChild.GetHeadlessDescendantCount()
								: lowerViews + 1;
						}
					}

					return found ? lowerViews : -1;
			}
		}

		private static int GetHeadlessDescendantCount(this IHeadlessLayout headlessLayout)
		{
			var count = 0;
			foreach (var child in headlessLayout)
			{
				if (child is IHeadlessLayout { IsHeadless: true } headlessChild)
				{
					count += headlessChild.GetHeadlessDescendantCount();
				}
				else
				{
					count++;
				}
			}
			return count;
		}
	}
}
