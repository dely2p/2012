using ContosoCookbook.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ContosoCookbook.Data;
using Windows.UI.ApplicationSettings;
using Callisto.Controls;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;
using System.Net.Http;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;



// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace ContosoCookbook
{
    /// <summary>
    /// 기본 응용 프로그램 클래스를 보완하는 응용 프로그램별 동작을 제공합니다.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// 줄이며 따라서 main() 또는 WinMain()과 논리적으로 동일합니다.
        /// </summary>
        /// 

        private Color _background = Color.FromArgb(255, 0, 77, 96);
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 최종 사용자가 응용 프로그램을 정상적으로 시작할 때 호출됩니다. 다른 진입점은
        /// 특정 파일을 열거나, 검색 결과를 표시하는 등 응용 프로그램을 시작할 때
        /// 사용됩니다.
        /// </summary>
        /// <param name="args">시작 요청 및 프로세스에 대한 정보입니다.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                if (!String.IsNullOrEmpty(args.Arguments))
                    ((Frame)Window.Current.Content).Navigate(typeof(ItemDetailPage), args.Arguments);

                Window.Current.Activate();
                return;
            }
            await RecipeDataSource.LoadLocalDataAsync();// 요리법 자료들을 연결
            // Clear tiles and badges
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();

            // Register for push notifications
            var profile = NetworkInformation.GetInternetConnectionProfile();

            if (profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                var buffer = CryptographicBuffer.ConvertStringToBinary(channel.Uri, BinaryStringEncoding.Utf8);
                var uri = CryptographicBuffer.EnetdcodeToBase64String(buffer);
                var client = new HttpClient();

                try
                {
                    var response = await client.GetAsync(new Uri("http://ContosoRecipes8.cloudapp.net?uri=" + uri + "&type=tile"));

                    if (!response.IsSuccessStatusCode)
                    {
                        var dialog = new MessageDialog("Unable to open push notification channel");
                        dialog.ShowAsync();
                    }
                }
                catch (HttpRequestException)
                {
                    var dialog = new MessageDialog("Unable to open push notification channel");
                    dialog.ShowAsync();
                }
            }

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            // If the app was activated from a secondary tile, show the recipe
            if (!String.IsNullOrEmpty(args.Arguments))
            {
                rootFrame.Navigate(typeof(ItemDetailPage), args.Arguments);
                Window.Current.Content = rootFrame;
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // 필요한 정보를 탐색 매개 변수로 전달하여 새 페이지를
                // 구성합니다.
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 프레임을 현재 창에 배치하고 프레임이 활성 상태인지 확인합니다.
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }
        void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add an About command
            var about = new SettingsCommand("about", "About", (handler) =>
            {
                var settings = new SettingsFlyout();
                settings.Content = new AboutUserControl();
                settings.HeaderBrush = new SolidColorBrush(_background);
                settings.Background = new SolidColorBrush(_background);
                settings.HeaderText = "About";
                settings.IsOpen = true;
            });

            args.Request.ApplicationCommands.Add(about);
        }


        /// <summary>
        /// 응용 프로그램 실행이 일시 중지된 경우 호출됩니다. 응용 프로그램이 종료될지
        /// 또는 메모리 콘텐츠를 변경하지 않고 다시 시작할지 여부를 결정하지 않은 채
        /// 응용 프로그램 상태가 저장됩니다.
        /// </summary>
        /// <param name="sender">일시 중지된 요청의 소스입니다.</param>
        /// <param name="e">일시 중지된 요청에 대한 세부 정보입니다.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            ContosoCookbook.SearchResultsPage1.Activate(args.QueryText, args.PreviousExecutionState);
        }
    }
}
