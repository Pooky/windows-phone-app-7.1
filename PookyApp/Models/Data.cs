using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO.IsolatedStorage;



namespace PookyApp.Models
{
    public class Data
    {
        public static String defaultURL = "http://testing.drevotrieska.com/data.json";
        MainPage ui;

        public Data(MainPage ui){

            this.ui = ui;
        
        }
        

        public List<ListItem> getMockData()
        {

            string jsonString = @"{
                  'items':[
                    {
                      'ID':'15',
                      'Name':'Jan Novak',
                      'PhoneNumber':'+420 721 860 751'
                    },
                    {
                      'ID':'16',
                      'Name':'Jan Novak2',
                      'PhoneNumber':'+420 725 959 151'
                    }
                  ]
                }";

            List<ListItem> list = new List<ListItem>();

            JObject jsonItems = JObject.Parse(jsonString);
            foreach (JObject jsonItem in jsonItems["items"])
            {
                ListItem item = JsonConvert.DeserializeObject<ListItem>(jsonItem.ToString());
                list.Add(item);
            }


            Console.WriteLine("Aj");
            return list;
        }

        public void loadData()
        {

            String jsonString = null;
            string path = @"C:\Users\Martin\Documents\Visual Studio 2010\Projects\PookyApp\PookyApp\Resources\data.json";
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                using (var writeFile = myIsolatedStorage.OpenFile(path, FileMode.Open))
                using (StreamReader streamReader = new StreamReader(writeFile))
                {
                   jsonString = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Error: {0}", e.Message), e);
            }

            if (jsonString != null)
            {
                JObject jsonItems = JObject.Parse(jsonString);
                foreach (JObject jsonItem in jsonItems["items"])
                {
                    ListItem item = JsonConvert.DeserializeObject<ListItem>(jsonItem.ToString());

                }
            }

        }
        public void getDataFromUrl()
        {
            getDataFromUrl(defaultURL);
        }

        public void getDataFromUrl(String url)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;

            webClient.DownloadStringAsync(new Uri(url));
        }

        /**
         * On JSON Download
         **/
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    List<ListItem> list = new List<ListItem>();

                    JObject jsonItems = JObject.Parse(e.Result);
                    foreach (JObject jsonItem in jsonItems["items"])
                    {
                        ListItem item = JsonConvert.DeserializeObject<ListItem>(jsonItem.ToString());
                        list.Add(item);
                    }
                    ui.updateItems(list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
