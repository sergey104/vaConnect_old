using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using NativeWifi;
using Windows.UI.ViewManagement;
using System.Security.Cryptography.X509Certificates;
using Windows.Security.Cryptography.Certificates;
using Windows.Devices.WiFi;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using System.Collections.ObjectModel;

namespace vaConnect
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //  ApplicationView.PreferredLaunchViewSize = new Size(100, 1200);
            //   ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

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
        protected override async void  OnActivated(IActivatedEventArgs args)
        {
            Messages parameters = new Messages();
            Frame rootFrame = Window.Current.Content as Frame;
            WiFiAdapter firstAdapter;
            WiFiConnectionResult result1;
            ObservableCollection<WiFiNetworkDisplay> ResultCollection = new ObservableCollection<WiFiNetworkDisplay>();
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(WiFiConfigPage));

            }
            // Ensure the current window is active
            Window.Current.Activate();
            if (args.Kind == ActivationKind.Protocol)
            {


                ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                // Handle URI activation
                // The received URI is eventArgs.Uri.AbsoluteUri
                Uri myUri1 = new Uri(eventArgs.Uri.AbsoluteUri);
                WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(myUri1.Query);
                String token = decoder.GetFirstValueByName("token");
                String identifier = decoder.GetFirstValueByName("identifier");
                String Ssid;
                String password;
                int numVal = Int32.Parse(identifier);
                WiFiProfile z = new WiFiProfile();

                z = await OnboardingService.getInstance().getWiFiProfileAsync(token, identifier);
                Ssid = z.getUser_policies().getSsid();
                parameters.Notification = "Start connecting to " + z.getUser_policies().getEap_type() + " network.";
                rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(5));
                WiFiConfiguration wc = z.getWifiConfiguration();
                Window.Current.Activate();
                await CertificateEnrollmentManager.UserCertificateEnrollmentManager.ImportPfxDataAsync(
                            z.getUser_policies().getPublic_ca(),
                            "",
                            ExportOption.NotExportable,
                            KeyProtectionLevel.NoConsent,
                            InstallOptions.None,
                            "Test");


                var access = await WiFiAdapter.RequestAccessAsync();
                if (access != WiFiAccessStatus.Allowed)
                {
                    parameters.Clear();
                    parameters.Notification = "No WiFi Adapter. Or access is denied!";
                    parameters.Result = "System error!";
                    rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                    return;
                }
                else
                {
                    switch (numVal) {
                        case 2:
                        {   
                           ////////////////////////////////////////////////////////////////////
                       
                        //EAP-TTLS
                        var result = await Windows.Devices.Enumeration.DeviceInformation.
                        FindAllAsync(WiFiAdapter.GetDeviceSelector());
                        if (result.Count >= 1)
                        {
                            firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                            await firstAdapter.ScanAsync();
                            var report = firstAdapter.NetworkReport;
                            //   var message = string.Format("Network Report Timestamp: { 0}", report.Timestamp);
                            var message = "Find  ";
                            ResultCollection.Clear();
                            foreach (var network in report.AvailableNetworks)
                            {
                                //Format the network information
                                message += string.Format("NetworkName: {0}\n", network.Ssid);
                                ResultCollection.Add(new WiFiNetworkDisplay(network, firstAdapter));

                            }
                            var q = ResultCollection.Where(X => X.Ssid == Ssid).FirstOrDefault();
                            if(q == null)
                                    {
                                        parameters.Clear();
                                        parameters.Notification = "Cannot find network with ssid: " + Ssid;
                                        parameters.Result = "Net error!";
                                        rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                                        return;
                                    }     
                            parameters.Clear();
                            parameters.Notification = message;
                            rootFrame.Navigate(typeof(WiFiConfigPage), parameters);

                            WiFiNetworkDisplay selectedNetwork = q;


                            WiFiConnectionResult result0;
                            WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Manual;
                            if (selectedNetwork.AvailableNetwork.SecuritySettings.NetworkAuthenticationType == Windows.Networking.Connectivity.NetworkAuthenticationType.Open80211)
                            {
                                result0 = await firstAdapter.ConnectAsync(selectedNetwork.AvailableNetwork, reconnectionKind);
                            }
                            else
                            {
                                // Only the password potion of the credential need to be supplied
                                var credential = new PasswordCredential();
                                        String Password = z.getUser_policies().getPassword();
                                credential.Password = Password;

                                result0 = await firstAdapter.ConnectAsync(selectedNetwork.AvailableNetwork, reconnectionKind, credential);
                            }

                            if (result0.ConnectionStatus == WiFiConnectionStatus.Success)
                            {
                                parameters.Clear();
                                parameters.Notification = string.Format("Successfully connected to {0}.", selectedNetwork.Ssid);
                                parameters.Result = "Success!";
                                rootFrame.Navigate(typeof(WiFiConfigPage), parameters);

                                // refresh the webpage
                                // webViewGrid.Visibility = Visibility.Visible;
                                //  toggleBrowserButton.Content = "Hide Browser Control";
                                //  refreshBrowserButton.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                parameters.Clear();
                                parameters.Result = "Error!";
                                parameters.Notification = string.Format("Could not connect to {0}. Error: {1}", selectedNetwork.Ssid, result0.ConnectionStatus);
                                rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                            }

                        }
                        else
                        {
                            parameters.Clear();
                            parameters.Notification = "No WiFi Adapter. Or access is denied!";
                            parameters.Result = "System error!";
                            rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                            return;
                        }
                        break;
                        }
                        ///////////////////////////
                        case 1:
                            {
                                parameters.Clear();
                                parameters.Notification = "Not implemented yet";
                                parameters.Result = "System warning!";
                                rootFrame.Navigate(typeof(WiFiConfigPage), parameters);

                                break;
                            }
                        case 3:
                            {
                                parameters.Clear();
                                parameters.Notification = "Not implemented yet";
                                parameters.Result = "System waninbg!";
                                rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                               
                                break;
                            }
                        case 4:
                            {
                                ////////////////////////////////////////////////////////////////////

                                //EAP-PEAP
                                var result = await Windows.Devices.Enumeration.DeviceInformation.
                                FindAllAsync(WiFiAdapter.GetDeviceSelector());
                                if (result.Count >= 1)
                                {
                                    firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                                    await firstAdapter.ScanAsync();
                                    var report = firstAdapter.NetworkReport;
                                    //   var message = string.Format("Network Report Timestamp: { 0}", report.Timestamp);
                                    var message = "Find  ";
                                    ResultCollection.Clear();
                                    foreach (var network in report.AvailableNetworks)
                                    {
                                        //Format the network information
                                        message += string.Format("NetworkName: {0}\n", network.Ssid);
                                        ResultCollection.Add(new WiFiNetworkDisplay(network, firstAdapter));

                                    }
                                    var q = ResultCollection.Where(X => X.Ssid == Ssid).FirstOrDefault();
                                    if (q == null)
                                    {
                                        parameters.Clear();
                                        parameters.Notification = "Cannot find network with ssid: " + Ssid;
                                        parameters.Result = "Net error!";
                                        rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                                        return;
                                    }
                                    parameters.Clear();
                                    parameters.Notification = message;
                                    rootFrame.Navigate(typeof(WiFiConfigPage), parameters);

                                    WiFiNetworkDisplay selectedNetwork = q;


                                    WiFiConnectionResult result0;
                                    WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Manual;
                                    if (selectedNetwork.AvailableNetwork.SecuritySettings.NetworkAuthenticationType == Windows.Networking.Connectivity.NetworkAuthenticationType.Open80211)
                                    {
                                        result0 = await firstAdapter.ConnectAsync(selectedNetwork.AvailableNetwork, reconnectionKind);
                                    }
                                    else
                                    {
                                        // Only the password potion of the credential need to be supplied
                                        var credential = new PasswordCredential();
                                        String Password = z.getUser_policies().getPassword();
                                        credential.Password = Password;

                                        result0 = await firstAdapter.ConnectAsync(selectedNetwork.AvailableNetwork, reconnectionKind, credential);
                                    }

                                    if (result0.ConnectionStatus == WiFiConnectionStatus.Success)
                                    {
                                        parameters.Clear();
                                        parameters.Notification = string.Format("Successfully connected to {0}.", selectedNetwork.Ssid);
                                        parameters.Result = "Success!";
                                        rootFrame.Navigate(typeof(WiFiConfigPage), parameters);

                                        // refresh the webpage
                                        // webViewGrid.Visibility = Visibility.Visible;
                                        //  toggleBrowserButton.Content = "Hide Browser Control";
                                        //  refreshBrowserButton.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        parameters.Clear();
                                        parameters.Result = "Error!";
                                        parameters.Notification = string.Format("Could not connect to {0}. Error: {1}", selectedNetwork.Ssid, result0.ConnectionStatus);
                                        rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                                    }

                                }
                                else
                                {
                                    parameters.Clear();
                                    parameters.Notification = "No WiFi Adapter. Or access is denied!";
                                    parameters.Result = "System error!";
                                    rootFrame.Navigate(typeof(WiFiConfigPage), parameters);
                                    return;
                                }
                                break;
                            }
                            ///////////////////////////
                          
                    default:
                            {
                               break ;
                            }
                    }//switch end
                }
            }
          
                

                
        }
    }
}
