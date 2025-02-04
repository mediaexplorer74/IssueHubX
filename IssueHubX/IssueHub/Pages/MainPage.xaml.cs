﻿using System;
using System.Linq;
using System.Reactive.Linq;
using IssueHub.Actions;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
//using Xamarin.Forms.PlatformConfiguration.UWPSpecific;

using IssueHub.Helpers; // !

using Xamarin.Forms.Xaml;
using R = IssueHub.Properties.Resources;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage(IStore<AppState> store)
        {
            InitializeComponent();

            // RnD
            //listView.On<UWP>().SetSeparatorStyle(SeparatorStyle.FullWidth);

            ViewModel = new MainPageViewModel(store);
        }

        MainPageViewModel ViewModel
        {
            get => BindingContext as MainPageViewModel;
            set => BindingContext = value;
        }

        IStore<AppState> Store => ViewModel.Store;

        IDisposable disposable;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!Store.State.Login.LoggedIn)
            {
                Store.Dispatch(new AutoLogIn());
            }

            if (!Store.State.Login.LoggedIn)
            {
                // RnD
                //ShowLoginPage();
                
                return;
            }

            // RnD
            if (disposable == null)
            {
                //disposable = Store.Select(x => x.Login.LoggedIn)
                //    .DistinctUntilChanged()
                //    .Where(x => !x)
                //    .Subscribe(x => ShowLoginPage());
            }

            if (!ViewModel.Initialized.Value && ViewModel.InitializeCommand.CanExecute())
            {
                ViewModel.InitializeCommand.Execute();
            }
        }

        async void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            if (e.SelectedItem is RepositoryViewModel repository)
            {
                Store.Dispatch(new SelectRepository(repository.ToRepository()));
                await Navigation.PushAsync(
                    new IssuesPage(Store));
            }
            else if (e.SelectedItem is FavoriteViewModel favorite)
            {
                Store.Dispatch(new SelectRepository(favorite.ToRepository()));
                await Navigation.PushAsync(
                    new IssuesPage(Store));
            }
            else if (e.SelectedItem is SmartListViewModel smartList)
            {
                if (smartList.FullName == R.Created)
                {
                    Store.Dispatch(new SelectSmartList());
                    await Navigation.PushAsync(
                        new CreatedIssuesPage(Store));
                }
                else if (smartList.FullName == R.Assigned)
                {
                    Store.Dispatch(new SelectSmartList());
                    await Navigation.PushAsync(
                        new AssignedIssuesPage(Store));
                }
            }

            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        async void HandleSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalWithNavigationPageAsync(
                new SettingsPage(Store));
        }

        async void HandleSearchClicked(object sender, EventArgs e)
        {
            Store.Dispatch(new BeginSearch());
            await Navigation.PushModalWithNavigationPageAsync(
                new SearchPage(Store));
        }

        async void HandleRemoveFavoriteClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var vm = (FavoriteViewModel)menuItem.BindingContext;
            var repository = vm.ToRepository();
            await Store.DispatchAsync(
                FavoriteActions.RemoveFromFavorites(
                    owner: repository.Owner.Login,
                    name: repository.Name));
        }

        void ShowLoginPage()
        {
            OAuth2Authenticator authenticator = new OAuth2Authenticator
            (
                clientId: Helpers.Secrets.GitHubClientId,
                clientSecret: Helpers.Secrets.GitHubClientSecret,
                scope: Constants.GitHubScopes,
                authorizeUrl: new Uri(Constants.GitHubAuthorizeUrl),
                redirectUrl: new Uri(Helpers.Secrets.GitHubAuthorizationCallbackUrl),
                accessTokenUrl: new Uri(Constants.GitHubAccessTokenUrl),
                isUsingNativeUI: true
            );
           
            // AllowCancel  true  Android WebAuthenticatorNativeBrowserActivity.OnResume
            //  Authenticator  OnCancelled Completed 
            // e.IsAuthenticated  false
            authenticator.AllowCancel = false;
            authenticator.ClearCookiesBeforeLogin = true;
            authenticator.Completed += Authenticator_Completed;
            authenticator.Error += Authenticator_Error;
            AuthenticationState.Authenticator = authenticator;

            var presenter = new OAuthLoginPresenter();
            presenter.Login(authenticator);

        }//ShowLoginPage


        // Authenticator_Completed
        async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }

            if (e.IsAuthenticated &&
                e.Account.Properties.TryGetValue("access_token", out var accessToken))
            {
                await Store.DispatchAsync(
                    LoginActions.ReceiveAccessToken(accessToken));
            }
        }

        void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }
        }
    }
}