using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using PookyApp.Models;

namespace PookyApp
{
    public partial class Options : PhoneApplicationPage
    {

        public static String paramName = "inputUrl";
        public Options()
        {
            InitializeComponent();

            if (IsolatedStorageSettings.ApplicationSettings.Contains(paramName))
            {
                inputUrl.Text = IsolatedStorageSettings.ApplicationSettings[paramName] as string;
            }
            else
            {
                inputUrl.Text = Data.defaultURL;
            }


        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Gestures.xaml", UriKind.Relative));
        }

        /**
         * Save user URL
         **/
        private void buttonSaveOptions_Click(object sender, RoutedEventArgs e)
        {
            String uriName = inputUrl.Text;
            System.Uri uriResult;

            // check input url
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
            if (!result)
            {
                MessageBox.Show("Tato adresa není platná");
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (!settings.Contains(paramName))
            {
                settings.Add(paramName, inputUrl.Text);
            }
            else
            {
                settings[paramName] = inputUrl.Text;
            }
            settings.Save();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            // reset url and return to main apge
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings.Remove(paramName);

            inputUrl.Text = Data.defaultURL;
            
        }
    }
}