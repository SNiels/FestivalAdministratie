using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FestivalLibAdmin.Class.Model;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class Ticket:ObservableValidationObject
    {

        public Ticket()
        {

        }

        public Ticket(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Amount = Convert.ToInt32(record["Amount"]);
            Type = Festival.SingleFestival.TicketTypes.Where(type => type.ID == record["TicketType"].ToString()).First();
            TicketHolderProfile = UserProfile.GetUserByID(Convert.ToInt32(record["UserId"]));
            OnPropertyChanged("TicketHolder");
            OnPropertyChanged("TicketHolderEmail");
        }

        //static Ticket()
        //{
        //    Tickets = new ObservableCollection<Ticket>();
        //}

        //private string _id;

        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private int _userId;

        //public int UserId
        //{
        //    get { return _userId; }
        //    set { _userId = value; }
        //}
        

        //private string _ticketHolder;

        //public string TicketHolder
        //{
        //    get { return _ticketHolder; }
        //    set { _ticketHolder = value; }
        //}

        //private string _ticketHolderEmail;

        //public string TicketHolderEmail
        //{
        //    get { return _ticketHolderEmail; }
        //    set { _ticketHolderEmail = value; }
        //}

        //private int _amount;

        //public int Amount
        //{
        //    get { return _amount; }
        //    set { _amount = value; }
        //}


        //private TicketType _type;

        //public TicketType Type
        //{
        //    get { return _type; }
        //    set { _type = value; }
        //}

        //private static ObservableCollection<Ticket> _tickets;
        //public static ObservableCollection<Ticket> Tickets
        //{
        //    get { return _tickets; }
        //    set { _tickets = value; }
        //}

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private UserProfile _ticketHolderProfile;
        [ScaffoldColumn(false)]
        public UserProfile TicketHolderProfile
        {
            get {
                if (_ticketHolderProfile == null) TicketHolderProfile = new UserProfile();
                return _ticketHolderProfile; }
            set { _ticketHolderProfile = value;
            OnPropertyChanged("TicketHolder");
                OnPropertyChanged("TicketHolderEmail");
            }
        }
        [Required(ErrorMessage = "Gelieve de naam in te vullen")]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van de besteller", GroupName = "Besteller",Prompt="Bv. Barack Obama")]
        [DisplayFormat(ConvertEmptyStringToNull = true )]
        [Editable(true, AllowInitialValue = false)]
        public string TicketHolder
        {
            get {
                if (TicketHolderProfile != null)
                    return TicketHolderProfile.UserName;
                return null;
            }
            set
            {
                TicketHolderProfile.UserName = value;
                OnPropertyChanged("TicketHolder");
            }
        }
        [Required(ErrorMessage="Gelieve een email adres in te geven")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress,ErrorMessage="Gelieve een geldige email adres in te geven")]
        [EmailAddress(ErrorMessage = "Gelieve een geldig email adres in te geven")]
        [Display(Name = "Email", Order = 1, Description = "Het email adres van de besteller", GroupName = "Besteller",Prompt="Bv. Barack.Obama@whitehouse.com")]
        [DisplayFormat(ConvertEmptyStringToNull = true)] 
        public string TicketHolderEmail
        {
            get
            {
                if (TicketHolderProfile != null)
                    return TicketHolderProfile.Email;
                return null;
            }
            set
            {
                TicketHolderProfile.Email = value;
                OnPropertyChanged("TicketHolderEmail");
            }
        }

        private int _amount;
        [Required(ErrorMessage="Gelieve het aantal tickets in te geven")]
        [Range(1,int.MaxValue,ErrorMessage="Het aantal tickets moet minstens 1 zijn")]
        [Display(Name = "Hoeveelheid", Order = 2, Description = "De hoeveelheid tickets", GroupName = "Bestelling",Prompt="Bv. 5")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private TicketType _type;


        [Required(ErrorMessage="Gelieve een ticket type in te geven")]
        [Display(Name = "Type", Order = 3, Description ="Het type ticket", GroupName = "Bestelling")]
        public TicketType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public static ObservableCollection<Ticket> GetTickets()
        {
            return GetTicketsByQuery("SELECT * FROM Tickets");
            
        }

        private static ObservableCollection<Ticket> GetTicketsByQuery(string query,params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Ticket> tickets = new ObservableCollection<Ticket>();
                reader = Database.GetData(query,parameters);
                while (reader.Read())
                    tickets.Add(new Ticket(reader));
                reader.Close();
                reader = null;
                return tickets;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get tickets by query", ex);
            }
        }

        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM Tickets WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn ticket", ex);
            }
        }

        public bool Update()
        {
            try
            {
                if (Type.ID == null)
                    if (!Type.Insert()) throw new Exception("Could not insert ticket, because tickettype could not be inserted");
                if (TicketHolderProfile.ID == null)
                    if (!TicketHolderProfile.Insert()) throw new Exception("Could not insert ticket, because userprofile could not be inserted");
                /*int amountOfModifiedRows = Database.ModifyData("UPDATE Tickets SET Amount=@Amount,TicketType=@TicketType,UserId=@UserId WHERE ID=@ID AND ((SELECT SUM(Amount) FROM Tickets WHERE TicketType=@TicketType)+@Amount) <= (SELECT AmountOfTickets From TicketTypes WHERE ID=@TicketType)",
                    Database.CreateParameter("@Amount", Amount),
                    Database.CreateParameter("@TicketType", Type.ID),
                    Database.CreateParameter("@UserId", TicketHolderProfile.ID),
                    Database.CreateParameter("@ID",ID)
                    );*/
                int amountOfModifiedRows = Database.ModifyData("UPDATE Tickets SET Amount=@Amount,TicketType=@TicketType,UserId=@UserId WHERE ID=@ID",
                    Database.CreateParameter("@Amount", Amount),
                    Database.CreateParameter("@TicketType", Type.ID),
                    Database.CreateParameter("@UserId", TicketHolderProfile.ID),
                    Database.CreateParameter("@ID", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the contact, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                if (Type.ID == null)
                    if (!Type.Insert()) throw new Exception("Could not insert ticket, because tickettype could not be inserted");
                if (TicketHolderProfile.ID == null)
                    if (!TicketHolderProfile.Insert()) throw new Exception("Could not insert ticket, because userprofile could not be inserted");
                //reader = Database.GetData("INSERT INTO Tickets (Amount, TicketType, UserId) VALUES(@Amount, @TicketType, @UserId) WHERE ((SELECT SUM(Amount) FROM Tickets WHERE TicketType=@TicketType)+@Amount) <= (SELECT AmountOfTickets From TicketTypes WHERE ID=@TicketType); SELECT SCOPE_IDENTITY() as 'ID'",
                //    Database.CreateParameter("@Amount", Amount),
                //    Database.CreateParameter("@TicketType",Type.ID),
                //    Database.CreateParameter("@UserId",TicketHolderProfile.ID)
                //    );
                reader = Database.GetData("INSERT INTO Tickets (Amount, TicketType, UserId) VALUES(@Amount, @TicketType, @UserId);SELECT SCOPE_IDENTITY() as 'ID'",
                    Database.CreateParameter("@Amount", Amount),
                    Database.CreateParameter("@TicketType", Type.ID),
                    Database.CreateParameter("@UserId", TicketHolderProfile.ID)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted ticket, it is possible the insert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not add ticket.", ex);
            }
        }

        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == "Amount" && Type != null && Type.ID != null && Type.AvailableTickets < Amount)
                {
                    try { 
                    Validator.ValidateProperty(Amount, new ValidationContext(this) { MemberName = propertyName });
                    return "Het gewenste aantal tickets is groter dan het aantal beschikbare tickets";
                    }catch(Exception ex)
                    {
                            return ex.Message;
                    }
                }
                return base[propertyName];
            }
        }

        public override bool IsValid()
        {
            if (Type != null && Type.ID != null && Type.AvailableTickets < Amount)
            {
                    return false;
            }
            return base.IsValid();
        }

        public static IEnumerable<Ticket> GetTicketsByUserID(string userID)
        {
            return GetTicketsByQuery("SELECT * FROM Tickets WHERE UserId=@UserId", Database.CreateParameter("@UserId", userID));
        }

        public void CreatePdf(string path, string outputPath)
        {
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork+=(sender, eventArgs) =>
            //{            
                WordprocessingDocument newdoc = null;
                try
                {
                    string filename = outputPath + "\\" + TicketHolder + ID+ ".docx";

                    File.Copy(path, filename, true);
                    newdoc = WordprocessingDocument.Open(filename, true);
                    IDictionary<String, BookmarkStart> bookmarks = new Dictionary<String, BookmarkStart>();
                    foreach (BookmarkStart bms in newdoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                    {
                        bookmarks[bms.Name] = bms;
                    }
                    bookmarks["TicketType"].Parent.InsertAfter<Run>(new Run(new Text(Type.Name)), bookmarks["TicketType"]);
                    bookmarks["Naam"].Parent.InsertAfter<Run>(new Run(new Text(TicketHolder)), bookmarks["Naam"]);
                    bookmarks["Email"].Parent.InsertAfter<Run>(new Run(new Text(TicketHolderEmail)), bookmarks["Email"]);
                
                    bookmarks["Aantal"].Parent.InsertAfter<Run>(new Run(new Text(String.Empty + Amount)), bookmarks["Aantal"]);
                    Run run = new Run(new Text(ID));
                    RunProperties prop = new RunProperties();
                    RunFonts font = new RunFonts() { Ascii = "Free 3 of 9 Extended", HighAnsi = "Free 3 of 9 Extended"};
                    FontSize size = new FontSize() { Val = "96" };
                    prop.Append(font);
                    prop.Append(size);
                    run.PrependChild<RunProperties>(prop);
                    bookmarks["Barcode"].Parent.InsertAfter<Run>(run, bookmarks["Barcode"]);
                    newdoc.Close();
                }
                catch (Exception) {if(newdoc!=null)newdoc.Close(); } 
        //    };
        }
    }
}
