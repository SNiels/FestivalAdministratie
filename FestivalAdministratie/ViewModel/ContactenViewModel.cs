using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class ContactenViewModel:PortableClassLibrary.ObservableObject,IPage
    {

        #region contacts
        private ObservableCollection<Contactperson> _contacts;
        public ObservableCollection<Contactperson> Contacten
        {
            get {
                if (_contacts != null) return _contacts;//if contacts are already loaded, return them, else get them from the Festival and watch it for updates
                try
                {
                    _contacts= Festival.SingleFestival.ContactPersons;
                    IsContactenEnabled = true;//if contacts could not be loaded, fe a connectivity problem, everything gets disabled and a messagebox is shown
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
                        if (newitem.ID==null&&newitem.IsValid()) newitem.Insert();//if the new item does not have an ID, it means it has not been inserted yet, if it's valid insert it
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
                    if (olditem.Delete())//if the delete is succesful, remove the event, the item should be garbage collected
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
                if (person.ID != null)//if id is null, contact is not inserted, so insert that if valid, else update if valid
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

        public ICommand AddContactCommand
        {
            get
            {
                return new RelayCommand(AddContact, CanAddContact);
            }
        }

        private void AddContact()
        {
            Contacten.Add(new Contactperson(true));//constructor overloading
        }

        private bool CanAddContact()
        {
            return IsContactenEnabled;
        }

        #endregion

        #region contacttypes

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
        }//same logic as for contacts

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

        #endregion

        public string Name//interface property for navigation purposes, this is shown
        {
            get {return "Contacten"; }
        }

        #region dialog

        private bool _isDialogVisible;

        public bool IsDialogVisible//set to true and the binded visibility property is set to visible, else collapsed. A bool to visibility converter would be better, because it is reusable. That's for my next project.
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
        }//simply hide the dialog, original plans were to have a submit and cancel button, but now theres just on close button.

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

        #endregion

        public ICommand FilterCommand
        {
            get
            {
                return new RelayCommand<FilterEventArgs>(Filter);
            }
        }//this is binded to the filter event of the contact viewsource

        private void Filter(FilterEventArgs e)//this is called foreach item in the viewsource
        {
            if (String.IsNullOrWhiteSpace(Query))
            {
                e.Accepted = true;
                return;
            }
            var contact = (e.Item as Contactperson);
            e.Accepted = false;
            foreach(var prop in contact.GetType().GetProperties())//iterate over all properties and check if it contains the querytext
            {
                try
                {
                    if (prop.Name!="Error"&&contact.GetType().GetProperty(prop.Name).GetValue(contact).ToString().ToLower().Contains(Query.ToLower()))//just skip error
                        e.Accepted = true;
                }
                catch (Exception) { }
            }
        }

        private string _query="";

        public string Query//query is binded to the watermark textbox
        {
            get { return _query; }
            set { _query = value; OnPropertyChanged("Query"); }
        }
        
    }
}
