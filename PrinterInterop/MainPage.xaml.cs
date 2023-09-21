using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PrinterInterop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFile imageFile = null;
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (imageFile != null)
                ApplicationData.Current.LocalSettings.Values["FileToPrint"] = imageFile.Path;

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            imageFile = await picker.PickSingleFileAsync();

            if (imageFile != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(await imageFile.OpenAsync(FileAccessMode.Read));
                img.Source = bitmap;
            }
        }
    }
}
