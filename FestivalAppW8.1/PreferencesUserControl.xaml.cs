using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DemoApp
{
    public sealed partial class PreferencesUserControl : UserControl
    {
        public PreferencesUserControl()
        {
            this.InitializeComponent();
            //initialize the toggleswitch values from roaming settings
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Remember"))
                Remember.IsOn = (bool)ApplicationData.Current.RoamingSettings.Values["Remember"];
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("NSA"))
                NSA.IsOn = (bool)ApplicationData.Current.RoamingSettings.Values["NSA"];
        }

        private void OnToggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = (sender as ToggleSwitch);
            switch (toggle.Name)
            {
                case "Remember":
                    ApplicationData.Current.RoamingSettings.Values["Remember"] = toggle.IsOn;
                    break;
                case "NSA":
                    ApplicationData.Current.RoamingSettings.Values["NSA"] = toggle.IsOn;
                    break;
                default:
                    ApplicationData.Current.RoamingSettings.Values[toggle.Name] = toggle.IsOn;
                    break;
            }
        }
    }
}
