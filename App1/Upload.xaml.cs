using System;
using DataModel;
using App1.Common;
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
using System.Collections;
using DataSource;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.Pickers;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Upload : Page
    {
        FileDataSource DataSource;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();


        /// <summary>
        /// Initializes a new instance of the <see cref="Upload"/> class.
        /// </summary>
        public Upload()
        {
            this.InitializeComponent();
            DataSource = new FileDataSource();

        }

        /// <summary>
        /// Handles the Open event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click_Open(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Update event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            var Files = await DataSource.GetFilesAsync();
            FileComboBox.ItemsSource = Files;
                    
        }

        /// <summary>
        /// Handles the Upload event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Button_Click_Upload(object sender, RoutedEventArgs e)
        {

            // Clear previous returned file name, if it exists, between iterations of this scenario
            OutputTextBlock.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".doc");
            openPicker.FileTypeFilter.Add(".docx");
            openPicker.FileTypeFilter.Add(".pdf");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                string filename = Convert.ToString(file.Path);
                // Application now has read/write access to the picked file
                string finalPath = Path.GetDirectoryName(filename) + "\\" + file.Name;

                File NewFile = new File() { FileId = 1, FileName = Convert.ToString(finalPath) };
                await DataSource.AddFile(NewFile);

                OutputTextBlock.Text = "File picked: " + finalPath;
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage), null);

        }
    }
}
