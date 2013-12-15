using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using FestivalAdministratie.View;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class OnFirstStartScreenViewModel:PortableClassLibrary.ObservableObject
    {
        private Window _window;

        public Window Window
        {
            get { return _window; }
            set { _window = value; }
        }

        private Festival _festival = new Festival() { StartDate = Festival.SingleFestival.StartDate, EndDate = Festival.SingleFestival.EndDate,Name=Festival.SingleFestival.Name };

        public Festival LocalFestival
        {
            get { return _festival; }
        }

        public ICommand BevestigCommand
        {
            get
            {
                return new RelayCommand(Bevestig, CanBevestigen);
            }
        }

        private bool CanBevestigen()
        {
            return (LocalFestival.StartDate != null && LocalFestival.EndDate != null)&&LocalFestival.StartDate<=LocalFestival.EndDate&&LocalFestival.IsValid();
        }

        private void Bevestig()
        {
            //if (Festival.SingleFestival.StartDate == LocalFestival.StartDate && Festival.SingleFestival.EndDate == LocalFestival.EndDate)
              //  OpenMainWindow();
                //Annuleer();
            //save value
            if (Festival.SingleFestival.StartDate == null && Festival.SingleFestival.EndDate == null)
            {
                SafeAndCloseWindow();
                return;
            }
            bool latereStart=  Festival.SingleFestival.StartDate<LocalFestival.StartDate;
            bool vroegereEind=Festival.SingleFestival.EndDate > LocalFestival.EndDate;

            if (vroegereEind && latereStart)
                if (MessageBox.Show("U hebt een vroegere eind, en een latere start datum gekozen. U zal de datum van de optredens opnieuw moeten selecteren.", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SafeAndCloseWindow();
                    return;
                }
                else return;
            
                //dmjlmjml

            if (vroegereEind )
                if (MessageBox.Show("U hebt een vroegere eind datum gekozen. U zal de datum van de optredens opnieuw moeten selecteren.", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SafeAndCloseWindow();
                    return;
                }
                else return;


            if (latereStart)
                if (MessageBox.Show("U hebt een latere start datum gekozen. U zal de datum van de optredens opnieuw moeten selecteren.", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SafeAndCloseWindow();
                    return;
                }
                else return;
            
            SafeAndCloseWindow();
        }

        private void SafeAndCloseWindow()
        {
            Festival.SingleFestival.StartDate = LocalFestival.StartDate;
            Festival.SingleFestival.EndDate = LocalFestival.EndDate;
            Festival.SingleFestival.Name = LocalFestival.Name;
            if (Festival.SingleFestival.ID == null) Festival.SingleFestival.Insert();
            else Festival.SingleFestival.Update();
            Festival.SingleFestival.ComputeLineUps();
            OpenMainWindow();
            
            Window.Close();
        }

        private void OpenMainWindow()
        {
            if (MainWindow.Window == null)
            {
                MainWindow.Window = new MainWindow();
                MainWindow.Window.Show();
            }
            else MainWindow.Window.Activate();
        }

        public ICommand AnnuleerCommand
        {
            get
            {
                return new RelayCommand(Annuleer);
            }
        }

        private void Annuleer()
        {
            Window.Close();
            //dont save values
        }
        
    }
}
