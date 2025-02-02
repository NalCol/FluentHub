using FluentHub.App.Services;
using FluentHub.App.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FluentHub.App.Dialogs
{
    public sealed partial class UserProfileEditor : ContentDialog
    {
        public UserProfileEditor(string login = null)
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<EditUserProfileViewModel>();

            ViewModel.Login = login;
        }

        public EditUserProfileViewModel ViewModel { get; }

        private async void OnContentDialogLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadUserAsync(ViewModel.Login);
        }

        private async void OnContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await ViewModel.UpdateUserAsync(ViewModel.Login);
        }
    }
}
