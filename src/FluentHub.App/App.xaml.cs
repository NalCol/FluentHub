// Copyright (c) FluentHub
// Licensed under the MIT License. See the LICENSE.

using FluentHub.App.Utils;
using FluentHub.App.Services;
using FluentHub.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel;
using Windows.Storage;
using CommunityToolkit.WinUI;
using FluentHub.App.ViewModels.Repositories.Codes;

namespace FluentHub.App
{
	public partial class App : Application
	{
		public static MainWindow WindowInstance { get; private set; } = null!;

		public static IntPtr WindowHandle { get; private set; }

		public new static App Current
			=> (App)Application.Current;

		public static SettingsViewModel AppSettings { get; set; }

		public static string AppVersion =
			$"{Windows.ApplicationModel.Package.Current.Id.Version.Major}." +
			$"{Windows.ApplicationModel.Package.Current.Id.Version.Minor}." +
			$"{Windows.ApplicationModel.Package.Current.Id.Version.Build}." +
			$"{Windows.ApplicationModel.Package.Current.Id.Version.Revision}";

		public App()
		{
			InitializeComponent();

			UnhandledException += OnUnhandledException;
			TaskScheduler.UnobservedTaskException += OnUnobservedException;

			var _host = ConfigureServices();
			Ioc.Default.ConfigureServices(_host.Services);
		}

		private static IHost ConfigureServices()
		{
			return Host.CreateDefaultBuilder()
				.ConfigureServices(services => services
					.AddSingleton<INavigationService, NavigationService>()
					.AddSingleton<ILogger>(new SerilogWrapperLogger(Serilog.Log.Logger))
					.AddSingleton<ToastService>()
					.AddSingleton<IMessenger>(StrongReferenceMessenger.Default)
					// ViewModels
					.AddSingleton<MainPageViewModel>()
					.AddSingleton<ViewModels.SignIn.IntroViewModel>()
					.AddTransient<ViewModels.AppSettings.AboutViewModel>()
					.AddTransient<ViewModels.AppSettings.Accounts.AccountViewModel>()
					.AddTransient<ViewModels.AppSettings.Accounts.OtherUsersViewModel>()
					.AddTransient<ViewModels.AppSettings.AppearanceViewModel>()
					.AddTransient<ViewModels.Dialogs.AccountSwitchingDialogViewModel>()
					.AddTransient<ViewModels.Dialogs.EditPinnedRepositoriesDialogViewModel>()
					.AddTransient<ViewModels.Dialogs.EditUserProfileViewModel>()
					.AddTransient<ViewModels.Viewers.DashBoardViewModel>()
					.AddTransient<ViewModels.Viewers.NotificationsViewModel>()
					.AddTransient<ViewModels.Organizations.OverviewViewModel>()
					.AddTransient<ViewModels.Organizations.RepositoriesViewModel>()
					.AddTransient<DetailsLayoutViewModel>()
					.AddTransient<TreeLayoutViewModel>()
					.AddTransient<ViewModels.Repositories.Commits.CommitsViewModel>()
					.AddTransient<ViewModels.Repositories.Commits.CommitViewModel>()
					.AddTransient<ViewModels.Repositories.Discussions.DiscussionsViewModel>()
					.AddTransient<ViewModels.Repositories.Discussions.DiscussionViewModel>()
					.AddTransient<ViewModels.Repositories.Issues.IssueViewModel>()
					.AddTransient<ViewModels.Repositories.Issues.IssuesViewModel>()
					.AddTransient<ViewModels.Repositories.Projects.ProjectsViewModel>()
					.AddTransient<ViewModels.Repositories.Projects.ProjectViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.ChecksViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.ConversationViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.CommitViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.CommitsViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.FileChangesViewModel>()
					.AddTransient<ViewModels.Repositories.PullRequests.PullRequestsViewModel>()
					.AddTransient<ViewModels.Repositories.Releases.ReleasesViewModel>()
					.AddTransient<ViewModels.Repositories.Releases.ReleaseViewModel>()
					.AddTransient<ViewModels.Searches.CodeViewModel>()
					.AddTransient<ViewModels.Searches.IssuesViewModel>()
					.AddTransient<ViewModels.Searches.RepositoriesViewModel>()
					.AddTransient<ViewModels.Searches.UsersViewModel>()
					.AddTransient<ViewModels.UserControls.FileContentBlockViewModel>()
					.AddTransient<ViewModels.UserControls.FileNavigationBlockViewModel>()
					.AddTransient<ViewModels.UserControls.IssueCommentBlockViewModel>()
					.AddTransient<ViewModels.UserControls.ReadmeContentBlockViewModel>()
					.AddTransient<ViewModels.UserControls.LatestCommitBlockViewModel>()
					.AddTransient<ViewModels.Users.ContributionsViewModel>()
					.AddTransient<ViewModels.Users.DiscussionsViewModel>()
					.AddTransient<ViewModels.Users.FollowersViewModel>()
					.AddTransient<ViewModels.Users.FollowingViewModel>()
					.AddTransient<ViewModels.Users.IssuesViewModel>()
					.AddTransient<ViewModels.Users.OrganizationsViewModel>()
					.AddTransient<ViewModels.Users.OverviewViewModel>()
					.AddTransient<ViewModels.Users.PackagesViewModel>()
					.AddTransient<ViewModels.Users.ProjectsViewModel>()
					.AddTransient<ViewModels.Users.PullRequestsViewModel>()
					.AddTransient<ViewModels.Users.RepositoriesViewModel>()
					.AddTransient<ViewModels.Users.StarredReposViewModel>()
				).Build();
		}

		private static void EnsureSettingsAndConfigurationAreBootstrapped()
		{
			AppSettings ??= new SettingsViewModel();
		}

		private void EnsureWindowIsInitialized()
		{
			WindowInstance = new MainWindow();
			WindowInstance.Activated += Window_Activated;
			//Window.Closed += Window_Closed;
			WindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(WindowInstance);
		}

		#region Event Methods
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

			// Initialize MainWindow here
			EnsureWindowIsInitialized();

			// Initialize static services
			EnsureSettingsAndConfigurationAreBootstrapped();

			// Initialize Window
			WindowInstance.InitializeApplication(activatedEventArgs.Data);
		}

		public void OnActivated(AppActivationArguments activatedEventArgs)
		{
			// Called from Program class

			// Initialize Window
			WindowInstance.DispatcherQueue.EnqueueAsync(() => WindowInstance.InitializeApplication(activatedEventArgs.Data));
		}

		private void Window_Activated(object sender, WindowActivatedEventArgs args)
		{
			if (args.WindowActivationState == WindowActivationState.CodeActivated ||
				args.WindowActivationState == WindowActivationState.PointerActivated)
			{
				ApplicationData.Current.LocalSettings.Values["INSTANCE_ACTIVE"] = -System.Diagnostics.Process.GetCurrentProcess().Id;
			}
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
			=> throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}

		private async void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
			=> await AppUnhandledException(e.Exception);

		private async void OnUnobservedException(object sender, UnobservedTaskExceptionEventArgs e)
			=> await AppUnhandledException(e.Exception);

		private async Task AppUnhandledException(Exception ex)
		{
			Ioc.Default.GetService<ILogger>()?.Fatal("Unhandled exception", ex);

			try
			{
				await new Microsoft.UI.Xaml.Controls.ContentDialog
				{
					Title = "Unhandled exception",
					Content = ex.Message,
					CloseButtonText = "Close"
				}
				.ShowAsync();
			}
			catch (Exception ex2)
			{
				Ioc.Default.GetService<ILogger>()?.Error("Failed to display unhandled exception", ex2);
			}
		}
		#endregion

		public static void CloseApp()
			=> WindowInstance.Close();

		public static AppWindow GetAppWindow(Window w)
		{
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(w);
			WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

			return AppWindow.GetFromWindowId(windowId);
		}
	}
}
