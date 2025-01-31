using ClientConvertisseurV1.Models;
using ClientConvertisseurV1.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClientConvertisseurV1.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConvertisseurEuroPage : Page
    {
        ObservableCollection<Devise> Devises = new ObservableCollection<Devise>();
        public ConvertisseurEuroPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            GetDataOnLoadAsync();
        }

        private async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("http://localhost:5189/api");
            List<Devise> result = await service.GetDevisesAsync("Devises");
            if(result == null)
            {
                MessageAsync("Erreur","API non disponible","ok");
            }
            else
            {
                Devises = new ObservableCollection<Devise>(result);   
            }
        }

        private async void MessageAsync(string m1,string m2,string m3)
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = m1,
                Content = m2,
                CloseButtonText = m3
            };
            noWifiDialog.XamlRoot = this.Content.XamlRoot;
            ContentDialogResult result = await noWifiDialog.ShowAsync();
            Console.WriteLine(result);
        }
    }
}
