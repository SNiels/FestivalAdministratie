using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoApp.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using System.Text;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Media.Capture;
using Windows.UI.StartScreen;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using PortableClassLibrary.Model;
//using Callisto.Controls;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace DemoApp
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage : DemoApp.Common.LayoutAwarePage
    {

        private StorageFile _photo;
        private StorageFile _video;

        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = await OptredenDataSource.GetItem((String)navigationParameter);
            if (item != null)
            {
                this.DefaultViewModel["Group"] = item.Group;
                this.DefaultViewModel["Items"] = item.Group.Items;
                this.flipView.SelectedItem = item;
            }
            DataTransferManager.GetForCurrentView().DataRequested += ItemDetailPage_DataRequested;
        }

        void ItemDetailPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var item = (OptredenDataItem)this.flipView.SelectedItem;
            request.Data.Properties.Title = item.Title;
            if(_photo != null)
            {
                request.Data.Properties.Description = "Band photo";
                var reference = RandomAccessStreamReference.CreateFromFile(_photo);
                request.Data.Properties.Thumbnail = reference;
                request.Data.SetBitmap(reference);
                _photo = null;
            }
            else if (_video != null)
            {
                request.Data.Properties.Description = "Band video";
                List<StorageFile> items = new List<StorageFile>();
                items.Add(_video);
                request.Data.SetStorageItems(items);
                _video = null;
            }
            else
            {
                request.Data.Properties.Description = "Band playing at Satisfaction";

                // Share recipe text
                var recipe = "\r\nDescription\r\n";
                recipe += String.Join("\r\n", item.Band.Description);
                request.Data.SetText(recipe);

                // Share recipe image
                var reference = RandomAccessStreamReference.CreateFromUri(new Uri(item.ImagePath.AbsoluteUri));
                request.Data.Properties.Thumbnail = reference;
                request.Data.SetBitmap(reference);

            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (OptredenDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;

            DataTransferManager.GetForCurrentView().DataRequested -= ItemDetailPage_DataRequested;
        }

        //private void OnBragButtonClicked(object sender, RoutedEventArgs e)
        //{
        //    //create a menu containing two items
        //    /*Menu menu = new Menu();
        //    MenuItem item1 = new MenuItem { Text = "Photo" };
        //    item1.Tapped+=OnCapturePhoto;
        //    menu.Items.Add(item1);
        //    MenuItem item2 = new MenuItem { Text = "Video" };
        //    item2.Tapped += OnCaptureVideo;
        //    menu.Items.Add(item2);

        //    //Show the menu in a Flyout anchored to the Brag button
        //    Callisto.Controls.Flyout flyout = new Callisto.Controls.Flyout();*/

        //    /*MenuFlyout menu = new MenuFlyout();
        //    MenuFlyoutItem item1 = new MenuFlyoutItem { Text = "Photo" };
        //    item1.Tapped+=OnCapturePhoto;
        //    menu.Items.Add(item1);
        //    MenuFlyoutItem item2 = new MenuFlyoutItem { Text = "Video" };
        //    item2.Tapped += OnCaptureVideo;
        //    menu.Items.Add(item2);

        //    //Show the menu in a Flyout anchored to the Brag button
        //    Callisto.Controls.Flyout flyout = new Callisto.Controls.Flyout();

        //    flyout.Placement = PlacementMode.Top;
        //    flyout.HorizontalAlignment = HorizontalAlignment.Left;
        //    flyout.HorizontalContentAlignment = HorizontalAlignment.Left;
        //    flyout.PlacementTarget = BragButton;
        //    flyout.Content = menu;
        //    flyout.IsOpen = true;¨*/
        //}

        private async void OnCaptureVideo(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();
            camera.VideoSettings.Format = CameraCaptureUIVideoFormat.Mp4;
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Video);
            if(file!=null)
            {
                _video = file;
                DataTransferManager.ShowShareUI();
            }
        }

        private async void OnCapturePhoto(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI camera = new CameraCaptureUI();
            StorageFile file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                _photo = file;
                DataTransferManager.ShowShareUI();
            }
            
        }

        private async void OnPinRecipeButtonClicked(object sender, RoutedEventArgs e)
        {
            var item = (OptredenDataItem)this.flipView.SelectedItem;
            var tile = new SecondaryTile(
                item.UniqueId,
                item.ShortTitle,
                item.Title,
                item.UniqueId,
                TileOptions.ShowNameOnLogo, new Uri(@"ms-appx:///Assets/Square310x310Logo.scale-100.png"));

            await tile.RequestCreateAsync();
        }

        private async void OnReminderButtonClicked(object sender, RoutedEventArgs e)
        {
            var item = (OptredenDataItem)this.flipView.SelectedItem;
            var notifier = ToastNotificationManager.CreateToastNotifier();

            //Make sure notifications are enabled
            if(notifier.Setting!=NotificationSetting.Enabled)
            {
                var dialog = new MessageDialog("Notifications are currently disabled");
                await dialog.ShowAsync();
                return;
            }

            // Get a toast template and insert a text node containing a message
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var element = template.GetElementsByTagName("text")[0];
            element.AppendChild(template.CreateTextNode(item.Band.Name+" speelt binnen 5 minuten"));

            //Schedule the toast to appear 30 seonds from now

            //var date = DateTimeOffset.Parse(item.From.ToString()).AddMinutes(-5);//dit is hoe het zou moeten zijn
            var date = DateTimeOffset.Parse(DateTime.Now.ToString()).AddSeconds(15);//.AddMinutes(5);//dit is om de werking te demonstreren
            try
            {
                var stn = new ScheduledToastNotification(template, date);

                notifier.AddToSchedule(stn);
            }
            catch (Exception)
            {
                new MessageDialog("Optreden is al passé maat").ShowAsync();
            }

        }

        private void Optreden_Click(object sender, RoutedEventArgs e)
        {
            var optreden = (sender as FrameworkElement).DataContext;
            this.Frame.Navigate(typeof(ItemDetailPage), ((Optreden)optreden).ID);
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = (button.DataContext as Optreden).ID != (flipView.SelectedItem as OptredenDataItem).UniqueId;
        }

        private void Genre_Click(object sender, RoutedEventArgs e)
        {
            var genre = (sender as FrameworkElement).DataContext;
            this.Frame.Navigate(typeof(SearchResultsPage), ((Genre)genre).Name);
        }
    }
}
