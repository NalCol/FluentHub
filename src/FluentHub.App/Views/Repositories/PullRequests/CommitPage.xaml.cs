// Copyright (c) FluentHub
// Licensed under the MIT License. See the LICENSE.

using FluentHub.App.Services;
using FluentHub.App.ViewModels.Repositories.PullRequests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using FluentHub.App.Data.Parameters;

namespace FluentHub.App.Views.Repositories.PullRequests
{
    public sealed partial class CommitPage : LocatablePage
    {
        public CommitViewModel ViewModel { get; }

        private readonly INavigationService _navigation;

        public CommitPage()
            : base(NavigationPageKind.Repository, NavigationPageKey.PullRequests)
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<CommitViewModel>();
            _navigation = Ioc.Default.GetRequiredService<INavigationService>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as FrameNavigationParameter;
            ViewModel.Login = param.UserLogin;
            ViewModel.Name = param.RepositoryName;
            ViewModel.Number = param.Number;
            ViewModel.CommitItem = param.Parameters.ElementAtOrDefault(0) as Commit;

            var command = ViewModel.LoadRepositoryPullRequestCommitPageCommand;
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }
}
