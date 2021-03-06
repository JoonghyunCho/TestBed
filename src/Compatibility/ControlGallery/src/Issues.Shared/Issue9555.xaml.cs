using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Compatibility.ControlGallery.Issues
{
	public class FooEffect : RoutingEffect
	{
		public FooEffect() : base("XamControl.FooEffect")
		{
		}
	}

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 9555, "[iOS] Applying an Effect to Frame adds a shadow", PlatformAffected.iOS)]
	public partial class Issue9555 : ContentPage
	{
		public Issue9555()
		{
#if APP
			InitializeComponent();
#endif
		}
	}
}