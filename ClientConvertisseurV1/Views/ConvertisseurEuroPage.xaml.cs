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
using System.ComponentModel;
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
    public sealed partial class ConvertisseurEuroPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Devise> _devises;

        public ObservableCollection<Devise> Devises
        {
            get => _devises;
            set
            {
                if (_devises != value)
                {
                    _devises = value;
                    OnPropertyChanged(nameof(Devises));  // Notify UI that Devises has changed
                }
            }
        }
        private double montant;
        public double Montant
        {
            get { return montant; }
            set
            {
                montant = value;
                OnPropertyChanged(nameof(Montant));  // Notify UI that Devises has changed
            }
        }

        public ConvertisseurEuroPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            GetDataOnLoadAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will be called when any property changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("http://localhost:5199/api/");
            List<Devise> result = await service.GetDevisesAsync("devises");
            if (result == null)
            {
                MessageAsync("Erreur", "API non disponible");
            }
            else
            {
                Devises = new ObservableCollection<Devise>(result);
            }
        }

        private async void MessageAsync(string m1, string m2)
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = m1,
                Content = m2,
                CloseButtonText = "ok"
            };
            noWifiDialog.XamlRoot = this.Content.XamlRoot;
            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private void ConvertirButton_Click(object sender, RoutedEventArgs e)
        {
            Devise devise;
            if (DeviseComboBox.SelectedItem== null)
            {
                MessageAsync("Erreur", "Il faut renseigner une devise");
            }
            else if (MontantTextBox.Text == null || MontantTextBox.Text == "")
            {
                MessageAsync("Erreur", "Il faut renseigner un montant");
            }
            else
            {
                devise = (Devise)DeviseComboBox.SelectedItem;
                Montant = Convert.ToDouble(MontantTextBox.Text) * devise.Taux;
            }

        }
    }

}
