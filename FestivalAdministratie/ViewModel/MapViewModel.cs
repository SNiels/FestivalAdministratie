using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FestivalAdministratie.ViewModel
{
    public class MapViewModel:PortableClassLibrary.ObservableObject,IPage//this works but because the map is not implemented in the w8.1 and mvc app, I have left it out of this app as well
    {

        public ObservableCollection<Stage> Stages
        {
            get { return Festival.SingleFestival.Stages; }

        }

        public string FestivalMap
        {
            get { return Festival.SingleFestival.FestivalMap; }
            set { 
                Festival.SingleFestival.FestivalMap = value;
            OnPropertyChanged("FestivalMap");
            try
            {
                Festival.SingleFestival.Update();
            }
            catch (Exception) { }
            }
        }

        public string Name
        {
            get { return "Map"; }
        }

        #region unused
        //public ICommand ChooseMapCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(ChooseMap);
        //    }
        //}

        //private void ChooseMap()
        //{
        //    try
        //    {
        //        OpenFileDialog ofd = new OpenFileDialog();
        //        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        //        ofd.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
        //        if(ofd.ShowDialog()==true)
        //        {
        //            Image = new BitmapImage(new Uri(ofd.FileName));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
                
        //    }
        //}
        #endregion

        #region image

        public ICommand DropImageCommand
        {
            get
            {
                return new RelayCommand<DragEventArgs>(AddImage);
            }
        }

        private void AddImage(DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data.ContainsText())
            {
                if (new Regex(@"(?:([^:/?#]+):)?(?://([^/?#]*))?([^?#]*\.(?:jpg|gif|png))(?:\?([^#]*))?(?:#(.*))?", RegexOptions.None).IsMatch(data.GetText()))
                    FestivalMap = data.GetText();
            }
            //if (data.ContainsFileDropList())
            //{
            //    var files = data.GetFileDropList();
            //    //SelectedAttraction.Picture = ImageConverter.Convert(files[0]);

            //}
        }
        #endregion
    }
}
