using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using System.Linq;
using Windows.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using System.Globalization;

namespace CortanaCanteenChecker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        // could be decoupled via a ViewModelLocator but in this case we have only one ViewModel
        public static MainPageViewModel MainPageViewModel { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            this.EnsureInstancedMainVM();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            try
            {
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"CanteenCheckerCommands.xml");

                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);

                await UpdatePhraseList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }

        private async Task UpdatePhraseList()
        {
            try
            {
                var countryCode = "de-de";

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("CanteenCheckerCommandSet_" + countryCode, out var commandDefinition))
                {
                    var canteenService = new CanteenService();

                    var canteens = await canteenService.GetCanteens(null, null);

                    var canteenNames = canteens.Select(c => c.Name).Distinct().ToList();
                    var mealNames = canteens.Select(c => c.Meal).Distinct().ToList();

                    await commandDefinition.SetPhraseListAsync("canteen", canteenNames);
                    await commandDefinition.SetPhraseListAsync("meal", mealNames);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Set PhraseList faild: " + ex.ToString());
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            this.EnsureInstancedMainVM();

            // If the app was launched via a Voice Command, this corresponds to the "show trip to <location>" command. 
            // Protocol activation occurs when a tile is clicked within Cortana (via the background task)

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;

                var speechRecognitionResult = commandArgs.Result;

                var voiceCommandName = speechRecognitionResult.RulePath[0];
                var textSpoken = speechRecognitionResult.Text;

                //var commandMode = SemanticInterpretation("", speechRecognitionResult);

                switch (voiceCommandName)
                {
                    case "showTodaysMenueForeground":
                        var canteen = this.SemanticInterpretation("canteen", speechRecognitionResult);

                        App.MainPageViewModel.NameFilter = canteen;

                        break;
                    case "showMealForeground":
                        var meal = this.SemanticInterpretation("meal", speechRecognitionResult);

                        App.MainPageViewModel.DishFilter = meal;

                        break;
                    default:
                        Debug.WriteLine("default case");
                        break;
                }

                
            }

            // Re"peat the same basic initialization as OnLaunched() above, taking into account whether
            // or not the app is already active.
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(MainPage));

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void EnsureInstancedMainVM()
        {
            if(App.MainPageViewModel == null)
            {
                App.MainPageViewModel = new MainPageViewModel(new CanteenService());
            }
        }
    }
}
