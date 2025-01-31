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




        private void ActionSetConversion()
        {
            Devise devise;
            if (DeviseComboBox.SelectedItem == null)
            {
                //MessageAsync("Erreur", "Il faut renseigner une devise");
            }
            else if (Resultat == null )
            {
                //MessageAsync("Erreur", "Il faut renseigner un montant");
            }
            else
            {
                devise = (Devise)DeviseComboBox.SelectedItem;
                Resultat = Convert.ToDouble(Montant) * devise.Taux;
            }
        }

        private async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("http://localhost:5199/api/");
            List<Devise> result = await service.GetDevisesAsync("devises");
            if (result == null)
            {
            }
            else
            {
                Devises = new ObservableCollection<Devise>(result);
            }
        }


    }
}
