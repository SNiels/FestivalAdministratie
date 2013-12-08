using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class ContactenViewModel:PortableClassLibrary.ObservableObject,IPage
    {
        private ObservableCollection<Contactperson> _contacts;
        public ObservableCollection<Contactperson> Contacten
        {
            get {
                if (_contacts != null) return _contacts;
                try
                {
                    _contacts= Festival.SingleFestival.ContactPersons;
                    IsContactenEnabled = true;
                    foreach(var contact in _contacts)
                        contact.PropertyChanged += contact_PropertyChanged;
                    _contacts.CollectionChanged += contacts_CollectionChanged;
                    return _contacts;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Contacten konden niet uit de database gehaald worden.");
                    IsContactenEnabled = false;
                    return null;
                }
            }
            /*set {
                Festival.SingleFestival.ContactPersons= value;
            OnPropertyChanged("Contacten");
            }*/
        }

        void contacts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems!=null)
                foreach (Contactperson newitem in e.NewItems)
                {
                    try
                    {
                        newitem.PropertyChanged += contact_PropertyChanged;
                        if (newitem.ID==null&&newitem.IsValid()) newitem.Insert();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contact niet in de database steken");
                    }
                }
            if(e.OldItems!=null)
            foreach (Contactperson olditem in e.OldItems)
            {
                if (olditem.ID == null) return;
                try
                {
                    if (olditem.Delete())
                    {
                        olditem.PropertyChanged -= contact_PropertyChanged;
                        olditem.ID = null;
                    }
                    else throw new Exception("Could not remove item");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Kon contact niet verwijderen in de database");
                }
            }
                
        }

        void contact_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Contactperson person = sender as Contactperson;
            if (person.IsValid())
            {
                if (person.ID != null)
                    try
                    {
                        if (!person.Update()) throw new Exception("Could not update contact");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contact niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!person.Insert()) throw new Exception("Could not insert contact");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contact niet in de database steken");
                    }
            }
        }

        private bool _isContactenEnabled;

        public bool IsContactenEnabled
        {
            get
            {
                return _isContactenEnabled;
            }
            set
            {
                _isContactenEnabled = value;
                OnPropertyChanged("IsContactenEnabled");
            }
        }

        private ObservableCollection<ContactpersonType> _types;

        public ObservableCollection<ContactpersonType> Types
        {
            get {
                if (_types != null) return _types;
                try
                {
                    _types = Festival.SingleFestival.ContactTypes;
                    foreach (var type in _types)
                        type.PropertyChanged += type_PropertyChanged;
                    _types.CollectionChanged += types_CollectionChanged;
                    IsTypesEnabled = true;
                    return _types;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Contacttypes konden niet uit de database gehaald worden.");
                    IsTypesEnabled = false;
                    return null;
                }
            }
            //set
            //{
            //    Festival.SingleFestival.ContactTypes = value;
            //OnPropertyChanged("Types");
            //}
        }

        void types_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ContactpersonType newitem in e.NewItems)
                {
                    newitem.PropertyChanged += type_PropertyChanged;
                    if (newitem.ID==null&&newitem.IsValid()) newitem.Insert();
                }
            if (e.OldItems != null)
                foreach (ContactpersonType olditem in e.OldItems)
                {
                    if (olditem.ID == null) return;
                    try
                    {
                        if (olditem.Delete())
                        {
                            olditem.PropertyChanged -= type_PropertyChanged;
                            olditem.ID = null;
                        }
                        else throw new Exception("Could not remove contacttype");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon de functie niet verwijderen in de database, zolang er contacten zijn van functie "+olditem.Name+",kan de functie niet verwijderd worden.");
                    }
                }
        }

        void type_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ContactpersonType type = sender as ContactpersonType;
            if (type.IsValid())
            {
                if (type.ID != null)
                    try
                    {
                        if (!type.Update()) throw new Exception("Could not update contacttype");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contacttype niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!type.Insert()) throw new Exception("Could not insert contacttype");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contacttype niet in de database steken");
                    }
            }
        }

        private bool _isTypesEnabled;

        public bool IsTypesEnabled
        {
            get
            {
                return _isTypesEnabled;
            }
            set
            {
                _isTypesEnabled = value;
                OnPropertyChanged("IsTypesEnabled");
            }
        }

        public string Name
        {
            get {return "Contacten"; }
        }

        public ICommand AddContactCommand
        {
            get
            {
                return new RelayCommand(AddContact, CanAddContact);
            }
        }

        private void AddContact()
        {
            Contacten.Add(new Contactperson(true));
        }

        private bool CanAddContact()
        {
            return IsContactenEnabled;
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
