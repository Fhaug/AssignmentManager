using DataModel;
using DataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Deadline : Page
    {
        AssignmentDataSource DataSource;
        Assignment NewAssignment;
        /// <summary>
        /// Initializes a new instance of the <see cref="Deadline"/> class.
        /// </summary>
        public Deadline()
        {
            this.InitializeComponent();
            DataSource = new AssignmentDataSource();
        }
        /// <summary>
        /// Verifies the date is future.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private bool VerifyDateIsFuture(DateTimeOffset date){
            if(date > DateTimeOffset.Now)
            {
                return true;
            }
            return false;
                 
        }
        /// <summary>
        /// Handles the Click event of the Add_Assignment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Add_Assignment_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyDateIsFuture(DeadlineDate.Date) == true)
            {
                if (string.IsNullOrWhiteSpace(AssignmentTextBox.Text))
                {
                    AssignmentTextBox.Text = "Unknown assignment";
                }

                DateController.Text = string.Format("Thank you. Your Assignment {0} is set for {1}", AssignmentTextBox.Text,
                                           DeadlineDate.Date.ToString("D"));



                NewAssignment = new Assignment() { AssignmentName = this.AssignmentTextBox.Text };
                NewAssignment.Dates.Add(new Date() { DateName = this.DeadlineDate.Date.ToString("D") });
                await DataSource.AddAssignment(NewAssignment);

                // In a real app, you'd probably set a value on your data object, like this:
                //_reservation.ArrivalDate = arrivalDatePicker.Date;  

            }

            else
            {
                DateController.Text = "DeadlineDate must be later than today";
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage), null);

        }

    

  


    }
}


       