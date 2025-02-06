using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientConvertisseurV2.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ClientConvertisseurV2.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace ClientConvertisseurV2.ViewModels
{
    public class ConvertisseurEuroViewModel : ObservableObject
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
                    OnPropertyChanged();
                }
            }
        }


        public IRelayCommand BtnSetConversion { get; }


        public ConvertisseurEuroViewModel()
        {
            GetDataOnLoadAsync();
            BtnSetConversion = new RelayCommand(ActionSetConversion);
        }

        private double resultat;
        public double Resultat
        {
            get { return resultat; }
            set
            {
                resultat = value;
                OnPropertyChanged(nameof(Resultat));  // Notify UI that Devises has changed
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

        private Devise deviseSelectionnee;
        public Devise DeviseSelectionnee
        {
            get { return deviseSelectionnee; }
            set
            {
                deviseSelectionnee = value;
                OnPropertyChanged(nameof(DeviseSelectionnee));  // Notify UI that Devises has changed
            }
        }


        private void ActionSetConversion()
        {
            Devise devise;
            if (DeviseSelectionnee == null)
            {
                MessageAsync("Erreur", "Il faut renseigner une devise");
            }
            else if (Montant == 0) 
            {
                MessageAsync("Erreur", "Il faut renseigner un montant");
            }
            else
            {
                devise = DeviseSelectionnee;
                Resultat = Montant * devise.Taux;
                Console.WriteLine(Resultat);
            }
        }

        private async void MessageAsync(string m1, string m2)
        {
            try
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = m1,
                    Content = m2,
                    CloseButtonText = "ok",
                    XamlRoot = App.MainRoot.XamlRoot  // Utilisation de App.MainRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // En cas d'erreur, on affiche dans la console pour le débogage
                Console.WriteLine($"Erreur lors de l'affichage du dialogue : {ex.Message}");
            }
        }


        private async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("http://localhost:5189/api/");
            List<Devise> result = await service.GetDevisesAsync("devises");
            Console.WriteLine(resultat);
            if (result == null)
            {
                MessageAsync("Erreur", "API non disponible");
            }
            else
            {
                Devises = new ObservableCollection<Devise>(result);
            }
        }


    }
}
