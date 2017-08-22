using App1.Common;
using DataModel;
using DataSource;
using App1.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace App1
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    

    public sealed partial class GroupedItemsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedItemsPage"/> class.
        /// </summary>
        public GroupedItemsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            DateDataSource DataSource = new DateDataSource();
            var Date = await DataSource.GetDatesAsync();
            this.defaultViewModel["Groups"] = Date;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var a = sender;
            var Item = ((Assignment)e.ClickedItem);
            this.Frame.Navigate(typeof(ItemDetailPage), Item);
        }

        #region NavigationHelper registration

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="GridCS.Common.NavigationHelper.LoadState" />
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState" />.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage), null);   
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        
        /// <summary>
        /// Handles the Click event of the add_alarm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void add_alarm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string audioSrc;
                int year = datepicker_alarm.Date.Year;
                int month = datepicker_alarm.Date.Month;
                int day = datepicker_alarm.Date.Day;
                int hour = timepicker_alarm.Time.Hours;
                int min = timepicker_alarm.Time.Minutes;
                int sec = timepicker_alarm.Time.Seconds;
                audioSrc = "Default";
               
                DateTime myDate1 = new DateTime(year, month, day, hour, min, sec);
                DateTime myDate2 = DateTime.Now;
                TimeSpan myDateResult = new TimeSpan();
                myDateResult = myDate1 - myDate2;
                if (myDate2 > myDate1)
                {
                    var x = new MessageDialog("Invalid date or time");
                    await x.ShowAsync();
                }
                else
                {

                    string title = "Alarm!";
                    string message = alm_msg.Text;
                    string imgURL = "ms-appx:///Assets/Capture.PNG";

                    string toastXmlString =
                     "<toast><visual version='1'><binding template='toastImageAndText02'><text id='1'>" 
                   + title + "</text><text id='2'>"
                        + message + "</text><image id='1' src='" + imgURL + "'/></binding></visual>\n" +
                         "<commands scenario=\"alarm\">\n" +
                            "<command id=\"snooze\"/>\n" +
                            "<command id=\"dismiss\"/>\n" +
                        "</commands>\n" +
                              "<audio src='ms-winsoundevent:Notification." + audioSrc + "'/>" +
                        "</toast>";

                    Windows.Data.Xml.Dom.XmlDocument toastDOM = new Windows.Data.Xml.Dom.XmlDocument();
                    toastDOM.LoadXml(toastXmlString);
                    var toastNotifier1 = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();

                    double x1 = myDateResult.TotalSeconds;
                    TimeSpan snoozeInterval = TimeSpan.FromSeconds(60);

                    var customAlarmScheduledToast = new Windows.UI.Notifications.ScheduledToastNotification(toastDOM, DateTime.Now.AddSeconds(x1), snoozeInterval, 0);

                    toastNotifier1.AddToSchedule(customAlarmScheduledToast);
                    var x = new MessageDialog("Assignment Start Set!");
                    await x.ShowAsync();
                }

            }
            catch
            { }
            }
        }


    
    }
