using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using Windows.Phone.Media.Devices;


namespace PhoneApp3
{
    public partial class Page1 : PhoneApplicationPage
    {
        

        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

        }
        private void recordingList_SelectionChanged(object sender, EventArgs e)
        { 
        }
    }
}