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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace PookyApp.Models
{
    public class ListItem
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ID { get; set; }
        public Boolean isDone { get; set; }


        public ListItem(String ID, String Name, String PhoneNumber)
        {
            this.ID = ID;
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
            this.isDone = false;
        }

    }

    public class Items : ObservableCollection<ListItem>
    {
        public Items()
        {
            /*Add(new ListItem("1", "Jan Novák", "+420 721 860 751"));
            Add(new ListItem("2", "Franta Tomáš", "+420 728 595 229"));
            Add(new ListItem("3", "Pan Horáček", "+420 158 959 362"));
            for (int i = 0; i < 15; i++)
            {
                Add(new ListItem("4", "Tomáš Vrána" + i.ToString(), "+420 849 595 659"));
            }*/
        }

        public Items(List<ListItem> items)
        {
            foreach (ListItem item in items)
            {
                Add(item);
            }

        }



    }
}
