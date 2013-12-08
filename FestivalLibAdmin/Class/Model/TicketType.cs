using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class TicketType:ObservableValidationObject
    {
        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }

        private double _price;

        public virtual double Price
        {
            get { return _price; }
            set { _price = value;
            OnPropertyChanged("Price");
            }
        }

        public virtual int AvailableTickets
        {
            get { return AmountOfTickets-TicketsSold; }
        }

        private int _ticketsSold=-1;

        public int TicketsSold
        {
            get {
                if (_ticketsSold < 0) TicketsSold = GetAmountOfSoldTickets();
                return _ticketsSold; }
            set {
                
                _ticketsSold = value;
            OnPropertyChanged("TicketsSold");
            OnPropertyChanged("AvailableTickets");
            }
        }

        private int GetAmountOfSoldTickets()
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("SELECT SUM([Amount]) as TicketsSold FROM [Festival].[dbo].[Tickets]");
                int amount = -1;
                if (reader.Read())
                    amount = Convert.ToInt32(reader["TicketsSold"]);
                reader.Close();
                reader = null;
                return amount;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get amount of sold tickets", ex);
            }
        }
        

        private int _amountOfTickets;

        public TicketType(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
            Price = Convert.ToInt32(record["Price"]);
            AmountOfTickets = Convert.ToInt32(record["AmountOfTickets"]);
        }

        public virtual int AmountOfTickets
        {
            get { return _amountOfTickets; }
            set { _amountOfTickets = value;
            OnPropertyChanged("AmountOfTickets");
            OnPropertyChanged("AvailableTickets");
            }
        }

        public static ObservableCollection<TicketType> GetTypes()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<TicketType> types = new ObservableCollection<TicketType>();
                reader = Database.GetData("SELECT * FROM TicketTypes");
                while (reader.Read())
                    types.Add(new TicketType(reader));
                reader.Close();
                reader = null;
                return types;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get tickettypes", ex);
            }
        }
    }
}
