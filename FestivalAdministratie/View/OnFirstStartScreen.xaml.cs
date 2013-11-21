﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FestivalAdministratie.ViewModel;

namespace FestivalAdministratie.View
{
    /// <summary>
    /// Interaction logic for OnFirstStartScreen.xaml
    /// </summary>
    public partial class OnFirstStartScreen : Window
    {
        public OnFirstStartScreen()
        {
            InitializeComponent();
            (this.DataContext as OnFirstStartScreenViewModel).Window = this;
        }
    }
}
