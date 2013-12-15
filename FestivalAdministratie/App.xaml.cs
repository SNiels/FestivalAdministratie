using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FestivalLibAdmin.Model;
using Helper;

namespace FestivalAdministratie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Database.ConnectionFailed += Database_ConnectionFailed;
                //Database.TestConnection();
                Festival.SingleFestival = Festival.GetFestival();
                if(Festival.SingleFestival==null)
                {
                    Festival.SingleFestival = new Festival();
                    this.StartupUri = new Uri("View/OnFirstStartScreen.xaml",UriKind.Relative);
                }else 
                StartupUri = new Uri("View/MainWindow.xaml",UriKind.Relative);
                base.OnStartup(e);
                Festival.SingleFestival.ComputeLineUps();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Database_ConnectionFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Kon geen connectie maken met de database. Mogelijk heeft u geen netwerk verbinding tot de database.");
        }
    }
}
