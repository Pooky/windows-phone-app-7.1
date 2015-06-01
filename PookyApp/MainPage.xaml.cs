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
using PookyApp.Models;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows.Threading;


namespace PookyApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        Items items;
        ListItem actualItem;
        Data data;
        // Drag events
        Orientation DragDirection;
        Object DragOriginObject;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.data = new Data(this);

            this.items = new Items();
            listBox1.DataContext = items;

            data.getDataFromUrl();
           
            this.actualItem = null;

        }
        /**
         * Method to make phone call
         * 
         **/
        private void PhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton button = (HyperlinkButton)sender;

            ListItem item = (ListItem)button.DataContext;

            // make call
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = item.PhoneNumber;
            phoneCallTask.DisplayName = item.Name;
            

            phoneCallTask.Show();


        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.Relative));
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            refreshList();
        }
        private void OptionsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Options.xaml", UriKind.Relative));
        }

        private void OnItemDoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            //ApplicationTitle.Text = "DoubleTapp";

            /*TextBlock box = (TextBlock)sender;
            Grid grid = (Grid)VisualTreeHelper.GetParent(box);
            Border border = (Border)VisualTreeHelper.GetParent(grid);*/

            Border border = (Border)sender;
           
            ListItem item = (ListItem)border.DataContext;
            if (item.isDone)
            {
                border.Background = null;
                item.isDone = false;
            }
            else
            {
                border.Background = new SolidColorBrush(Colors.Green);
                item.isDone = true;
            }
        }

        private void OnItemDragStarted(object sender, DragStartedGestureEventArgs e)
        {
           // ListItem item = (ListItem)(sender as Border).DataContext;
          //  this.actualItem = item;

            this.DragDirection = e.Direction;
            this.DragOriginObject = e.OriginalSource;
        }

        /**
         * Gesture completed
         * 
         **/
        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            
            if (e.Direction.Equals(this.DragDirection) && e.OriginalSource.Equals(this.DragOriginObject)){

                if (e.Direction.ToString() == "Horizontal" && e.HorizontalChange < -10 && e.VerticalChange > -40)
                {

                    // swipe left
                    this.SwipeLeft(sender, e);

                }
                
                //@ TODO, implement swipe Right

            }

        }

        private void SwipeLeft(object sender, DragCompletedGestureEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Really delete this item?", "Confirm", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                // ON SWIPE LEFT REMOVE ITEM
                ListItem item = (ListItem)(sender as Border).DataContext;
                items.Remove(item);
            }
        }

        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBlock block = (TextBlock)sender;
            block.Foreground = new SolidColorBrush(Colors.Black);
            block.FontWeight = FontWeights.Bold;
        }
        /**
         * Item list update
         **/
        public void updateItems(List<ListItem> list)
        {
            items = new Items(list);
            listBox1.DataContext = items;

            //MessageBox.Show("Your list was updated");
            textBoxStatus.Text = "Done!"; 

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += hideLoadingStatus;
            timer.Start();
            
        
        }

        public void hideLoadingStatus(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= hideLoadingStatus;
          
            textBoxStatus.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void refreshList()
        {
            // loading box
            textBoxStatus.Visibility = System.Windows.Visibility.Visible;
            ActualDate.Text = string.Format("{0:d.M.yyyy}", DateTime.Now); // actual date time
            
            // Check intenter connection
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();

            if (!isConnected)
            {
                MessageBox.Show("Sorry, your internet connection is not available."
                + "Please connect your phone and then try again"
                , "Internet connection", MessageBoxButton.OK);
                return;
            }

            String url = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Options.paramName))
            {
                url = IsolatedStorageSettings.ApplicationSettings[Options.paramName] as string;
            }
            else
            {
                url = Data.defaultURL;
            }
            data.getDataFromUrl(url); // load data    

        }
    }
} 