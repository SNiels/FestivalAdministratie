using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            private ObservableCollection<Optreden> _list;

            public ObservableCollection<Optreden> List
            {
                get { 
                    if(_list!=null)return _list;
                    try
                    {
                        _list = Festival.SingleFestival.Optredens;
                        if (_list.Count > 0) SelectedItem = _list.First();
                        IsOptredensEnabled = true;
                        foreach (Optreden optreden in _list)
                            optreden.PropertyChanged += Optreden_PropertyChanged;
                        _list.CollectionChanged += Optreden_CollectionChanged;
                        return _list;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Optredens konden niet uit de database gehaald worden.");
                        IsOptredensEnabled = false;
                        return null;
                    }
                }
            }

            private bool _isOptredensEnabled;

            public bool IsOptredensEnabled
            {
                get { return _isOptredensEnabled; }
                set { _isOptredensEnabled = value;
                OnPropertyChanged("IsOptredensEnabled");
                }
            }
            
            private void Optreden_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (Optreden newitem in e.NewItems)
                    {
                        try
                        {
                            newitem.PropertyChanged += Optreden_PropertyChanged;
                            if (newitem.ID == null && newitem.IsValid()) newitem.Insert();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Kon optreden niet in de database steken");
                        }
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Optreden olditem in e.OldItems)
                    {
                        if (olditem.ID == null) return;
                        try
                        {
                            if (olditem.Delete())
                            {
                                olditem.PropertyChanged -= Optreden_PropertyChanged;
                                olditem.ID = null;
                            }
                            else throw new Exception("Could not remove performance");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Kon optreden niet verwijderen uit de database, gelieve eerst de optredens van de stage te verwijderen.");
                        }
                    }
                }
            }

            private void Optreden_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "LeftPositionPercent"||e.PropertyName=="WidthPercent") return;
                if (e.PropertyName == "LineUp"||e.PropertyName=="Stage") OnPropertyChanged("IsAnItemLineUpStageSelected");
                Optreden optreden = sender as Optreden;
                if (optreden.IsValid())
                {
                    if (optreden.ID != null)
                        try
                        {
                            if (!optreden.Update()) throw new Exception("Could not update performance");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Kon optreden niet updaten naar de database");
                        }
                    else
                        try
                        {
                            if (!optreden.Insert()) throw new Exception("Could not insert performance");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Kon optreden niet in de database steken");
                        }
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
                    OnPropertyChanged("IsAnItemSelectedAndLineUpSelected");
                }
            }



            public bool IsAnItemLineUpStageSelected
            {
                get { return IsAnItemSelected&&SelectedItem.LineUp!=null&&SelectedItem.Stage!=null; }
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
                var optreden = new Optreden();
 	            List.Add(optreden);
                SelectedItem = optreden;
            }

            private bool CanAddNewCommand()
            {
                return IsOptredensEnabled;//voorlopig
            }

            //private ObservableCollection<Band> _bands;

            public ObservableCollection<Band> Bands
            {
                get { 
                    return Festival.SingleFestival.Bands;
                }
            }

            //private ObservableCollection<Stage> _stages;

            public ObservableCollection<Stage> Stages
            {
                get { 
                    //return _stages;
                    return Festival.SingleFestival.Stages;
                }
            }

            //private ObservableCollection<LineUp> _dagen;

            public ObservableCollection<LineUp> Dagen
            {
                get { 
                    return Festival.SingleFestival.LineUps;
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

