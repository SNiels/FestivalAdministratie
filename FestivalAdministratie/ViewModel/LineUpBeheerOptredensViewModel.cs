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
    class LineUpBeheerOptredensViewModel:PortableClassLibrary.ObservableObject,BeheerIPage
    {
            private static LineUpBeheerOptredensViewModel _viewModel;
            public static LineUpBeheerOptredensViewModel ViewModel
            {
                get
                {
                    if (_viewModel == null)
                        _viewModel = new LineUpBeheerOptredensViewModel();
                    return _viewModel;
                }
            }

            public LineUpBeheerOptredensViewModel()
            {
                #region
                //List = Optreden.Optredens;
                //Bands = Band.Bands;
                //Stages = Stage.Stages;
                //Dagen = LineUp.LineUps;
                //OnPropertyChanged("Optredens");
                //OnPropertyChanged("Stages");
                //OnPropertyChanged("LineUps");
                //OnPropertyChanged("Bands");
                //Festival.SingleFestival.PropertyChanged += SingleFestival_PropertyChanged;
                #endregion
            }

            //void SingleFestival_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            //{
            //    if (e.PropertyName == "LineUps") Dagen = ((Festival)sender).LineUps;
            //}


            public string Name
            {
                get { return "Optredens"; }
            }

            public string NameEnkel
            {
                get { return "Optreden"; }
            }

            //private ObservableCollection<Optreden> _list;

            public ObservableCollection<Optreden> List
            {
                get { 
                    //return _list;
                    return Festival.SingleFestival.Optredens;
                }
                set
                {
                    //_list = value;
                    Festival.SingleFestival.Optredens = value;
                    OnPropertyChanged("List");
                    if (value != null && value.Count() > 0) SelectedItem = value.First();
                }
            }

            private Optreden _selectedItem;

            public Optreden SelectedItem
            {
                get { return _selectedItem; }
                set
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    OnPropertyChanged("IsAnItemSelected");
                }
            }

            public ICommand AddItemCommand
            {
                get
                {
                    return new RelayCommand(AddNewOptreden, CanAddNewCommand);
                }
            }

            private void AddNewOptreden()
            {
 	            List.Add(new Optreden());
                
            }

            private bool CanAddNewCommand()
            {
                return true;//voorlopig
            }

            //private ObservableCollection<Band> _bands;

            public ObservableCollection<Band> Bands
            {
                get { 
                    //return _bands;
                    return Festival.SingleFestival.Bands;
                }
                set {
                    Festival.SingleFestival.Bands = value;
                    //_bands = value;
                    OnPropertyChanged("Bands");
                }
            }

            //private ObservableCollection<Stage> _stages;

            public ObservableCollection<Stage> Stages
            {
                get { 
                    //return _stages;
                    return Festival.SingleFestival.Stages;
                }
                set
                {
                    Festival.SingleFestival.Stages = value;
                    //_stages = value;
                    OnPropertyChanged("Stages");
                }
            }

            //private ObservableCollection<LineUp> _dagen;

            public ObservableCollection<LineUp> Dagen
            {
                get { 
                    //return _dagen;
                    return Festival.SingleFestival.LineUps;
                }
                set
                {
                    Festival.SingleFestival.LineUps = value;
                    //_dagen = value;
                    OnPropertyChanged("Dagen");
                }
            }

            public bool IsAnItemSelected
            {
                get
                {
                    return SelectedItem != null;
                }
            }
        }
    }

