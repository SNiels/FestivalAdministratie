using DemoApp.Common;

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
using DemoApp.Data;
using Windows.ApplicationModel.Search;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Networking.Connectivity;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Core;
using PortableClassLibrary.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace DemoApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private CoreDispatcher _dispatcher = null;
        private Color _background = Color.FromArgb(255, 0, 77, 96);

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        

        public CoreDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            _dispatcher = args.Window.Dispatcher;
            base.OnWindowCreated(args);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
                // Load recipe data
                try
                {
                    await OptredenDataSource.LoadRemoteDataAsync();
                }
                catch (Exception) { }

                //if the app was closed by the user the last time it ran, and if "Remember
                //where i was" is enabled, restore the navigation state
                if(args.PreviousExecutionState==ApplicationExecutionState.ClosedByUser)
                {
                    if(ApplicationData.Current.RoamingSettings.Values.ContainsKey("Remember"))
                    {
                        bool remember = (bool)ApplicationData.Current.RoamingSettings.Values["Remember"];
                        if(remember)
                            await SuspensionManager.RestoreAsync();
                    }
                }
                //register handler for SuggestionsRequested events from the search pane
                SearchPane.GetForCurrentView().SuggestionsRequested += App_SuggestionsRequested;

                //register handler for CommandsRequested events from the settings pane
                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;

                

                var file = await Package.Current.InstalledLocation.GetFileAsync(@"Data\license.xml");
                await Windows.ApplicationModel.Store.CurrentAppSimulator.ReloadSimulatorAsync(file);

                if(!string.IsNullOrWhiteSpace(args.Arguments))
                {
                    rootFrame.Navigate(typeof(ItemDetailPage), args.Arguments);
                    Window.Current.Content = rootFrame;
                    Window.Current.Activate();
                    return;
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if(args.PreviousExecutionState==ApplicationExecutionState.Running)
            {
                if (!String.IsNullOrWhiteSpace(args.Arguments))
                    ((Frame)Window.Current.Content).Navigate(typeof(ItemDetailPage),
                        args.Arguments);
                Window.Current.Activate();
                return;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            //Add an About command
            var about = new SettingsCommand("about", "About", (handler) =>
                {
                    var settings = new SettingsFlyout();
                    settings.Content = new AboutUserControl();
                    settings.HeaderBackground = new SolidColorBrush(_background);
                    settings.Title = "About";
                    settings.Show();
                });
            args.Request.ApplicationCommands.Add(about);

            var preferences = new SettingsCommand("preferences", "Preferences", (handler) =>
            {
                var settings = new SettingsFlyout();
                settings.Content = new PreferencesUserControl();
                settings.HeaderBackground = new SolidColorBrush(_background);
                settings.Title = "Preferences";
                settings.Show();
            });

            args.Request.ApplicationCommands.Add(preferences);
        }
        void App_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            string query = args.QueryText.ToLower();
            string[] terms = { "salt", "pepper", "water", "egg", "vinegar", "flour", "rice", "sugar", "oil","bacon","lots of bacon" };
            foreach (string term in terms)
                if (term.Contains(query)) args.Request.SearchSuggestionCollection.AppendQuerySuggestion(term);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="e">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs e)
        {
            //reinitialize the app if a new instance was launched for search
            if(e.PreviousExecutionState==ApplicationExecutionState.NotRunning||
                e.PreviousExecutionState==ApplicationExecutionState.ClosedByUser||
                e.PreviousExecutionState==ApplicationExecutionState.Terminated)
            {
                //Load recipe data
                await OptredenDataSource.LoadLocalDataAsync();

                //Register handler for SuggestionsRequested
                SearchPane.GetForCurrentView().SuggestionsRequested+=App_SuggestionsRequested;
                //register handler for CommandsRequested events from the settings pane
                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;

            }
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running

            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                DemoApp.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await DemoApp.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (DemoApp.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

            frame.Navigate(typeof(SearchResultsPage), e.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}
