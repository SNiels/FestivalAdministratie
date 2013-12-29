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
using Newtonsoft.Json;

namespace FestivalLibAdmin.Model
{
    public class Optreden:ObservableValidationObject
    {
        #region ctors
        public Optreden(IDataRecord record)
        {
            ID = record["ID"].ToString();
            _from = Convert.ToDateTime(record["From"]);
            _until = Convert.ToDateTime(record["Until"]);
            try
            {
                LineUp = Festival.SingleFestival.LineUps.Where(lineup => lineup.Dag == new DateTime(From.Year, From.Month, From.Day)).First();
            }
            catch (Exception) { }
            Stage = Festival.SingleFestival.Stages.Where(stage => stage.ID == record["Stage"].ToString()).First();
            Band = Festival.SingleFestival.Bands.Where(band => band.ID == record["Band"].ToString()).First();
        }

        public Optreden()
        {

        }

        #endregion

        private double CalculateLeftPercent()
        {
            TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
            return (From - LineUp.MinHour).TotalMinutes / aantalUren.TotalMinutes;
        }
        private double CalculateWidthPercent()
        {
            TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
            return (this.Until - From).TotalMinutes / aantalUren.TotalMinutes;
        }

        #region props

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _from;
        [DataType(DataType.Time,ErrorMessage="Gelieve een geldig tijdstip in te geven")]
        [Required(ErrorMessage="Gelieve de start van het optreden aan te geven")]
        [Display(Name = "Start optreden", Order = 0, Description = "Het start tijdstip van het optreden", GroupName = "Optreden",Prompt="Gelieve het begin van het optreden te kiezen")]
        public DateTime From
        {
            get {
                    if (LineUp != null && LineUp.Dag!=null&& _from != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _from.Hour, _from.Minute, 0);
                    return _from;
            }
            set
            {
                if (_from != null && _until != null && value >= _until)
                    if (value.Hour == 23 && value.Minute == 59)
                        return;
                    else
                        _until= value.AddMinutes(1);
                _from = value;
                //if (LineUp != null)
                //_from = new DateTime(LineUp.Dag.Year,LineUp.Dag.Month,LineUp.Dag.Day,value.Hour,value.Minute,0);
                OnPropertyChanged("From");
                OnPropertyChanged("LeftPositionPercent");
                OnPropertyChanged("WidthPercent");
                if (LineUp != null)
                    LineUp.OnHoursChanged();
            }
        }

        private DateTime _until;
        [DataType(DataType.Time, ErrorMessage = "Gelieve een geldig tijdstip in te geven")]
        [Required(ErrorMessage = "Gelieve het einde van het optreden aan te geven")]
        [Display(Name = "Einde optreden", Order = 1, Description = "Het eind tijdstip van het optreden", GroupName = "Optreden",Prompt="Gelieve het einde van het optreden te kiezen")]
        public DateTime Until
        {
            get {
                if (LineUp != null&&LineUp.Dag!=null && _until != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _until.Hour, _until.Minute, 0);
                return _until; }
            set
            {
                if (_from != null && _until != null && value <= _from)
                    if (value.Hour == 0 && value.Minute == 0)
                        return;
                    else
                        _from = value.AddMinutes(-1);
                //_until = new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, value.Hour, value.Minute, 0);
                _until = value;
                OnPropertyChanged("Until");
                OnPropertyChanged("LeftPositionPercent");
                OnPropertyChanged("WidthPercent");
                if (LineUp != null)
                    LineUp.OnHoursChanged();
            }
        }

        private LineUp _lineUp;
        [Display(Name = "Line-up", Order = 2, Description = "De dag van het optreden", GroupName = "Optreden",Prompt="Gelieve een dag te kiezen")]
        [Required(ErrorMessage = "Gelieve een LineUp te kiezen")]
        [JsonIgnore]
        public LineUp LineUp
        {
            get { return _lineUp; }
            set
            {
                _lineUp = value;
                OnPropertyChanged("LineUp");
                if (value != null)
                    LineUp.PropertyChanged += LineUp_PropertyChanged;
                if (Stage != null)
                    Stage.OnOptredensChanged();
            }
        }

        void LineUp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Hours") return;
            OnPropertyChanged("LeftPositionPercent");
            OnPropertyChanged("WidthPercent");
        }

        private Stage _stage;
        [Display(Name = "Stage", Order = 3, Description = "De stage van het optreden", GroupName = "Optreden", Prompt = "Gelieve een stage te kiezen")]
        [Required(ErrorMessage="Gelieve een stage te kiezen")]
        [JsonIgnore]
        public Stage Stage
        {
            get { return _stage; }
            set
            {
                Stage oldStage = _stage;
                _stage = value;
                OnPropertyChanged("Stage");
                OnPropertyChanged("FriendlyName");
                if (value != null)
                    value.OnOptredensChanged();
                if (oldStage != null)
                    oldStage.OnOptredensChanged();
            }
        }

        public string StageID
        {
            get
            {
                if (Stage != null) return Stage.ID;
                return null;
            }
        }

        private Band _band;
        [JsonIgnore]
        [Required(ErrorMessage = "Gelieve een band te kiezen")]
        [Display(Name = "Band", Order = 4, Description = "De optredende band", GroupName = "Optreden",Prompt="Gelieve een band te kiezen")]
        public Band Band
        {
            get { return _band; }
            set
            {
                _band = value;
                OnPropertyChanged("Band");
                OnPropertyChanged("FriendlyName");
            }
        }

        public string BandID { 
            get {
            if (Band != null) return Band.ID;
            return null;
            }
        }

        [ScaffoldColumn(false)]
        public double LeftPositionPercent
        {
            get
            {
                return CalculateLeftPercent();
            }
        }

        [ScaffoldColumn(false)]
        public double WidthPercent
        {
            get
            {
                return CalculateWidthPercent();
            }
        }
        [JsonIgnore]
        public string FriendlyName
        {
            get { return ToString(); }
        }

        #endregion

        public override string ToString()
        {
            if (Band == null && Stage == null) return "Nieuw optreden";
            return Band + " - " + Stage;
        }

        #region dal

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE Optredens SET [From]=@From,Until=@Until,Stage=@StageID,Band=@BandID WHERE ID=@ID",
                    Database.CreateParameter("@From",From),
                    Database.CreateParameter("@Until", Until),
                    Database.CreateParameter("@StageID", Stage.ID),
                    Database.CreateParameter("@BandID",Band.ID),
                    Database.CreateParameter("@ID", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the performance, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string sql = "INSERT INTO Optredens ([From],Until,Stage,Band) VALUES(@From,@Until,@StageID,@BandID); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@From", From),
                    Database.CreateParameter("@Until", Until),
                    Database.CreateParameter("@StageID", Stage.ID),
                    Database.CreateParameter("@BandID", Band.ID)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted performance, it is possible the insert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not insert performance", ex);
            }
        }

        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM Optredens WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn performance", ex);
            }
        }

        public static ObservableCollection<Optreden> GetOptredens()
        {
            return GetOptredensByQuery("SELECT * FROM Optredens ORDER BY [From] ASC");
            
        }

        private static ObservableCollection<Optreden> GetOptredensByQuery(string query,params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Optreden> optredens = new ObservableCollection<Optreden>();
                reader = Database.GetData(query,parameters);
                while (reader.Read())
                    optredens.Add(new Optreden(reader));
                reader.Close();
                reader = null;
                return optredens;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get optredens by query", ex);
            }
        }

        public static ObservableCollection<Optreden> GetOptredensByGenreID(string genreID)
        {
            return GetOptredensByQuery("SELECT Optredens.ID as 'ID', [From], Until, Stage, Band FROM Optredens INNER JOIN Band_Genre ON Band_Genre.BandID=Optredens.Band WHERE Band_Genre.GenreID=@GenreID", Database.CreateParameter("@GenreID", genreID));
        }

        #endregion

        [JsonIgnore]
        public override string this[string propertyName]
        {
            get
            {
                if ((propertyName == "From" || propertyName == "Until") && (From>Until||From==Until)) return "Gelieve de start vroeger dan het einde van het optreden te zetten";
                return base[propertyName];
            }
        }

        public override bool IsValid()
        {
            if (From > Until||From==Until) return false;
            return base.IsValid();
        }

        
    }
}
