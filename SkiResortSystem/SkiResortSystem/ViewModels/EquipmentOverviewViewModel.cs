﻿using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public class EquipmentOverviewViewModel : ObservableObject
    {

        public ObservableCollection<Utrustningsbokning> Utrustningsbokningar { get; set; }
        public Bokning Bokning { get; set; }
        public EquipmentOverviewViewModel()
        {
            
        }
        public EquipmentOverviewViewModel(List<Utrustningsbokning> utrustningsbokningar, Bokning bokning)
        {
            Utrustningsbokningar = new ObservableCollection<Utrustningsbokning>(utrustningsbokningar);
            Bokning = bokning;
        }
       

        private Utrustningsbokning selectedUtrustningsbokning;
        public Utrustningsbokning SelectedUtrustningsbokning
        {
            get { return selectedUtrustningsbokning; }
            set
            {
                selectedUtrustningsbokning = value;
                OnPropertyChanged();
            }
        } 
        /// <summary>
        /// sparar utrustningsbokning
        /// </summary>

        private ICommand saveEquipment = null;
        public ICommand SaveEquipment =>
            saveEquipment ??= saveEquipment = new RelayCommand<ICloseable>((closeable) =>
            {
                EquipmentController controller = new EquipmentController();
                controller.SaveEquipmentBooking(Utrustningsbokningar.ToList());
                CloseCommand.Execute(closeable);
            });
        /// <summary>
        /// tar bort utrustningsbokning
        /// </summary>
        private ICommand removeEquipment = null;
        public ICommand RemoveEquipment =>
            removeEquipment ??= removeEquipment = new RelayCommand<ICloseable>((closeable) =>
            {
                if (SelectedUtrustningsbokning != null)
                { 
                    EquipmentController controller = new EquipmentController();
                    if (controller.FindUtrustningsbokningar(SelectedUtrustningsbokning) != null) controller.RemoveEquipmentBooking(SelectedUtrustningsbokning);
                    Utrustningsbokningar.Remove(SelectedUtrustningsbokning);
                    OnPropertyChanged(nameof(Utrustningsbokningar));
                }
            });
        /// <summary>
        /// stänger fönster
        /// </summary>
        private ICommand closeCommand = null;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((closeable) =>
            {
                closeable.Close();
            });
    }
}
