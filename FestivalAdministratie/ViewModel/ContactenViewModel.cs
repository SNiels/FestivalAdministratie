using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class ContactenViewModel:PortableClassLibrary.ObservableObject,IPage
    {

        public ObservableCollection<Contactperson> Contacten
        {
            get { return Festival.SingleFestival.ContactPersons; }
            set { Festival.SingleFestival.ContactPersons= value;
            OnPropertyChanged("Contacten");
            }
        }

        public ObservableCollection<ContactpersonType> Types
        {
            get { return Festival.SingleFestival.ContactTypes; }
            set
            {
                Festival.SingleFestival.ContactTypes = value;
            OnPropertyChanged("Types");
            }
        }

        public string Name
        {
            get {return "Contacten"; }
        }

        private bool _isDialogVisible;

        public bool IsDialogVisible
        {
            get
            {
                return _isDialogVisible;
            }
            set
            {
                _isDialogVisible = value;
                if (value == true) DialogVisibility = System.Windows.Visibility.Visible;
                else DialogVisibility = System.Windows.Visibility.Collapsed;
                OnPropertyChanged("IsDialogVisible");
            }
        }

        private void SubmitDialog()
        {
            IsDialogVisible = false;
        }

        public ICommand CancelDialogCommand
        {
            get { return new RelayCommand(CancelDialog); }
        }

        private void CancelDialog()
        {
            IsDialogVisible = false;
        }

        public ICommand OpenDialogCommand
        {
            get { return new RelayCommand(OpenDialog); }
        }

        private void OpenDialog()
        {
            IsDialogVisible = true;
        }

        public ICommand SubmitDialogResultCommand
        {
            get { return new RelayCommand(SubmitDialog); }
        }

        private System.Windows.Visibility _dialogVisibility= System.Windows.Visibility.Collapsed;

        public System.Windows.Visibility DialogVisibility
        {
            get
            {
                return _dialogVisibility;
            }
            set
            {
                _dialogVisibility = value;
                OnPropertyChanged("DialogVisibility");
            }
        }
    }
}
