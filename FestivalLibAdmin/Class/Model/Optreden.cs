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
    public class Optreden:ObservableValidationObject
    {
        //static Optreden()
        //{
        //    Optredens = new ObservableCollection<Optreden>();
        //    #region
        //    //Optredens.Add(
        //    //    new Optreden()
        //    //    {
        //    //        Band = new Band() { Name = "ar" },
        //    //        From = DateTime.Now.AddHours(-1),
        //    //        Until = DateTime.Now.AddHours(1),
        //    //        Stage = Stage.Stages.First(),
        //    //        LineUp = LineUp.LineUps.First()
                    
        //    //    });
        //    //Optredens.Add(
        //    //    new Optreden()
        //    //    {
        //    //        Band = new Band() { Name = "dlfjsdlmf" },
        //    //        From = DateTime.Now.AddHours(1),
        //    //        Until = DateTime.Now.AddHours(2),
        //    //        Stage=Stage.Stages.First(),
        //    //        LineUp=LineUp.LineUps.First()
        //    //    });
        //    //Optredens.CollectionChanged += Optredens_CollectionChanged;
        //    #endregion
        //}
        //#region
        ////static void Optredens_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        ////{
        ////    foreach (Stage stage in Stage.Stages)
        ////        stage.OnOptredensChanged();
        ////}

        ////public static void NewOptreden()
        ////{
        ////    Optreden optreden = new Optreden();
        ////    Optredens.Add(optreden);
        ////    //if (LineUp.LineUps.Count > 0)
        ////    //{
        ////    //    optreden.LineUp = LineUp.LineUps.First();
        ////    //    optreden.From = LineUp.LineUps.First().MinHour;
        ////    //    optreden.Until = LineUp.LineUps.First().MaxHour;
        ////    //}
        ////    //if(Band.Bands.Count>0)
        ////    //optreden.Band = Band.Bands.First();
        ////    //if (Stage.Stages.Count > 0)
        ////    //    optreden.Stage = Stage.Stages.First();
        ////}
        //#endregion
        ////public Optreden()
        ////{
        ////    #region
        ////    //if (LineUp.LineUps.Count > 0)
        ////    //{
        ////    //    LineUp = LineUp.LineUps.First();
        ////    //    From = LineUp.LineUps.First().MinHour;
        ////    //    Until = LineUp.LineUps.First().MaxHour;
        ////    //}

        ////    //if(Band.Bands.Count>0)
        ////    //Band = Band.Bands.First();
        ////    //if (Stage.Stages.Count > 0)
        ////    //    Stage = Stage.Stages.First();
        ////    #endregion
        ////}

        //private double CalculateLeftPercent()
        //{
        //    TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
        //    return (From-LineUp.MinHour).TotalMinutes /aantalUren.TotalMinutes;
        //}

        //private double CalculateWidthPercent()
        //{
        //    TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
        //    return (this.Until - From).TotalMinutes/aantalUren.TotalMinutes;
        //}

        //private string _id;

        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private DateTime _from;

        //public DateTime From
        //{
        //    get { return _from; }
        //    set { _from = value;
        //    OnPropertyChanged("From");
        //    OnPropertyChanged("LeftPositionPercent");
        //    OnPropertyChanged("WidthPercent");
        //    if (LineUp != null)
        //    LineUp.OnHoursChanged();
        //    }
        //}

        //private DateTime _until;

        //public DateTime Until
        //{
        //    get { return _until; }
        //    set { _until = value; OnPropertyChanged("Until");
        //    OnPropertyChanged("LeftPositionPercent");
        //    OnPropertyChanged("WidthPercent");
        //        if(LineUp!=null)
        //    LineUp.OnHoursChanged();
        //    }
        //}

        //private LineUp _lineUp;

        //public LineUp LineUp
        //{
        //    get { return _lineUp; }
        //    set { _lineUp = value;
        //    OnPropertyChanged("LineUp");
        //        if(value!=null)
        //    LineUp.PropertyChanged += LineUp_PropertyChanged;
        //        if (Stage != null)
        //            Stage.OnOptredensChanged();
        //    }
        //}

        //void LineUp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Hours") return;
        //        OnPropertyChanged("LeftPositionPercent");
        //        OnPropertyChanged("WidthPercent");
        //}
        

        //private Stage _stage;

        //public Stage Stage
        //{
        //    get { return _stage; }
        //    set {
        //        Stage oldStage = _stage;
        //        _stage = value;
        //    OnPropertyChanged("Stage");
        //    OnPropertyChanged("FriendlyName");
        //    if (value != null)
        //        value.OnOptredensChanged();
        //    if (oldStage != null)
        //        oldStage.OnOptredensChanged();
        //    }

        //}

        //private Band _band;

        //public Band Band
        //{
        //    get { return _band; }
        //    set { _band = value;
        //    OnPropertyChanged("Band");
        //    OnPropertyChanged("FriendlyName");
        //    }
        //}

        ////private double _positionLeft;

        //public double LeftPositionPercent
        //{
        //    get {
        //        //try
        //        //{
        //            return CalculateLeftPercent();
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    Console.WriteLine(e.Message);
        //        //    return 0;
        //        //}
        //    }
        //    //set
        //    //{
        //    //    _positionLeft = value;
        //    //    OnPropertyChanged("LeftPositionPercent");
        //    //}
        //}

        ////private double _widthPercent;

        //public double WidthPercent
        //{
        //    get {
        //        //try
        //        //{
        //            return CalculateWidthPercent();
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    Console.WriteLine(e.Message);
        //        //    return 0;
        //        //}
                
        //    }
        //    //set
        //    //{
        //    //    _widthPercent = value;
        //    //    OnPropertyChanged("WidthPercent");
        //    //}
        //}

        //private static ObservableCollection<Optreden> _optredens;

        //public static ObservableCollection<Optreden> Optredens
        //{
        //    get { return _optredens; }
        //    set { _optredens = value; }
        //}
        //public string FriendlyName
        //{
        //    get { return ToString(); }
        //}

        //public override string ToString()
        //{
        //    if (Band == null && Stage == null) return "Nieuw optreden";
        //    return Band + " - " + Stage;
        //}

        public Optreden(IDataRecord record)
        {
            ID = record["ID"].ToString();
            From = Convert.ToDateTime(record["From"]);
            Until = Convert.ToDateTime(record["Until"]);
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

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _from;
        [DataType(DataType.DateTime,ErrorMessage="Gelieve een geldig tijdstip in te geven")]
        [Required(ErrorMessage="Gelieve de start van het optreden aan te geven")]
        [Display(Name = "Start optreden", Order = 0, Description = "Het start tijdstip van het optreden", GroupName = "Optreden",Prompt="Gelieve het einde van het optreden te kiezen")]
        public DateTime From
        {
            get {
                if (LineUp != null && _from != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _from.Hour, _from.Minute, 0);
                return _from; }
            set
            {
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
        [DataType(DataType.DateTime, ErrorMessage = "Gelieve een geldig tijdstip in te geven")]
        [Required(ErrorMessage = "Gelieve het einde van het optreden aan te geven")]
        [Display(Name = "Einde optreden", Order = 1, Description = "Het eind tijdstip van het optreden", GroupName = "Optreden",Prompt="Gelieve het begin van het optreden te kiezen")]
        public DateTime Until
        {
            get {
                if (LineUp != null && _until != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _until.Hour, _until.Minute, 0);
                return _until; }
            set
            {
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

        private Band _band;

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

        //private double _positionLeft;
        [ScaffoldColumn(false)]
        public double LeftPositionPercent
        {
            get
            {
                //try
                //{
                return CalculateLeftPercent();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    return 0;
                //}
            }
            //set
            //{
            //    _positionLeft = value;
            //    OnPropertyChanged("LeftPositionPercent");
            //}
        }

        //private double _widthPercent;
        [ScaffoldColumn(false)]
        public double WidthPercent
        {
            get
            {
                //try
                //{
                return CalculateWidthPercent();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    return 0;
                //}

            }
            //set
            //{
            //    _widthPercent = value;
            //    OnPropertyChanged("WidthPercent");
            //}
        }

        //private static ObservableCollection<Optreden> _optredens;

        //public static ObservableCollection<Optreden> Optredens
        //{
        //    get { return _optredens; }
        //    set { _optredens = value; }
        //}
        public string FriendlyName
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            if (Band == null && Stage == null) return "Nieuw optreden";
            return Band + " - " + Stage;
        }

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
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Optreden> optredens = new ObservableCollection<Optreden>();
                reader = Database.GetData("SELECT * FROM Optredens ORDER BY [From] ASC");
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
                throw new Exception("Could not get optredens", ex);
            }
        }

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
