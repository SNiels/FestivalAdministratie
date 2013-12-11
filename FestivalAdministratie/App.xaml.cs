using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            base.OnStartup(e);
            try
            {
                Database.ConnectionFailed += Database_ConnectionFailed;
                Database.TestConnection();
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
