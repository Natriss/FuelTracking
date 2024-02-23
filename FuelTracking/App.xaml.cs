using FuelTracking.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FuelTracking
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application
	{
		private static Window _window;
		public static Window Window
		{
			get
			{
				if (_window == null)
				{
					_window = new Window();
				}
				return _window;
			}
		}


		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			Window.ExtendsContentIntoTitleBar = true;
			Window.SystemBackdrop = new MicaBackdrop();
			Window.Content = new NavigationPage();
			Window.Activate();
		}
	}
}
