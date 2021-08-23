using Microsoft.Maui;

namespace Samples.Tizen
{
	class Program : MauiApplication<Startup>
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			Microsoft.Maui.Essentials.Platform.Init(CoreUIAppContext.GetInstance(this).MainWindow);
		}

		static void Main(string[] args)
		{
			var app = new Program();
			app.Run(args);
		}
	}
}
