using FluentHub.App.Data.Parameters;
using FluentHub.App.Services;
using FluentHub.App.ViewModels.Searches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

namespace FluentHub.App.Views.Searches
{
    public sealed partial class RepositoriesPage : Page
    {
        public RepositoriesPage()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<RepositoriesViewModel>();
        }

        public RepositoriesViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as FrameNavigationParameter;
            ViewModel.SearchTerm = param.Parameters.ElementAtOrDefault(0) as string;

            var command = ViewModel.LoadSearchRepositoriesPageCommand;
            if (command.CanExecute(null))
                command.ExecuteAsync(null);
        }
    }
}
