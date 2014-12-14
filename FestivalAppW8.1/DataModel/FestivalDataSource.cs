using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using Windows.Data.Json;
using Windows.ApplicationModel;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Storage;
using DemoApp.Common;
using PortableClassLibrary.Model;
using DemoApp;
using Windows.UI.Popups;
using System.Diagnostics;


//I used the default datamodel that W8 apps use to make use of the default templates that are provide,
//if I had more time, I would do it more cleanly
#region
// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace DemoApp.Data
{
    /// <summary>
    /// Base class for <see cref="OptredenDataItem"/> and <see cref="LineUpDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class FestivalDataCommon : BindableBase
    {
        internal static Uri _baseUri = new Uri("ms-appx:///");

        public FestivalDataCommon(String uniqueId, String title, String shortTitle, String imagePath)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._shortTitle = shortTitle;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _shortTitle = string.Empty;
        public string ShortTitle
        {
            get { return this._shortTitle; }
            set { this.SetProperty(ref this._shortTitle, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;

        public Uri ImagePath
        {
            get
            {
                return new Uri(FestivalDataCommon._baseUri, this._imagePath);
            }
        }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(FestivalDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public string GetImageUri()
        {
            return _imagePath;
        }
    }

    /// <summary>
    /// Optreden item data model.
    /// </summary>
    public class OptredenDataItem : FestivalDataCommon
    {
        public OptredenDataItem()
            : base(String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        public OptredenDataItem(String uniqueId, DateTime from, DateTime until, Band band, Stage stage,LineUpDataGroup group)
            : base(uniqueId, band.Name, band.Name, band.Picture)
        {
            this.UniqueId = uniqueId;
            this._from = from;
            this._until = until;
            this._band = band;
            this._stage = stage;
            this._group = group;
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return Band.Name; }
        }

        private string _shortTitle = string.Empty;
        public string ShortTitle
        {
            get { return Band.Name; }
        }

        private DateTime _from;
        public DateTime From
        {
            get { return this._from; }
            set { this.SetProperty(ref this._from, value); }
        }

        public string Day
        {
            get
            {
                return From.BeDayOfWeek().ToString();
            }
        }

        private DateTime _until;
        public DateTime Until
        {
            get { return this._until; }
            set { this.SetProperty(ref this._until, value); }
        }

        private Band _band;
        public Band Band
        {
            get { return this._band; }
            set { this.SetProperty(ref this._band, value); }
        }

        private Stage _stage;
        public Stage Stage
        {
            get { return this._stage; }
            set { this.SetProperty(ref this._stage, value); }
        }

        private LineUpDataGroup _group;
        public LineUpDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        private ImageSource _tileImage;
        private string _tileImagePath;

        public Uri TileImagePath
        {
            get
            {
                return new Uri(Band.Picture);
            }
        }

        public ImageSource TileImage
        {
            get
            {
                return new BitmapImage(TileImagePath);
            }
        }

        public void SetTileImage(String path)
        {
            this._tileImage = null;
            this._tileImagePath = path;
            this.OnPropertyChanged("TileImage");
        }
    }

    /// <summary>
    /// LineUp group data model.
    /// </summary>
    public class LineUpDataGroup : FestivalDataCommon
    {

        public LineUpDataGroup()
            : base(String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        public LineUpDataGroup(LineUp lineup)
            : base(lineup.Dag.BeDayOfWeek().ToString(), lineup.Dag.BeDayOfWeek().ToString(), lineup.Dag.BeDayOfWeek().ToString(), null)
        {
        }

        private ObservableCollection<OptredenDataItem> _items = new ObservableCollection<OptredenDataItem>();
        public ObservableCollection<OptredenDataItem> Items
        {
            get { return this._items; }
        }

        public IEnumerable<OptredenDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _groupImage;
        private string _groupImagePath;

        public ImageSource GroupImage
        {
            get
            {
                if (this._groupImage == null && this._groupImagePath != null)
                {
                    this._groupImage = new BitmapImage(new Uri(FestivalDataCommon._baseUri, this._groupImagePath));
                }
                return this._groupImage;
            }
            set
            {
                this._groupImagePath = null;
                this.SetProperty(ref this._groupImage, value);
            }
        }

        public int RecipesCount
        {
            get
            {
                return this.Items.Count;
            }
        }

        public void SetGroupImage(String path)
        {
            this._groupImage = null;
            this._groupImagePath = path;
            this.OnPropertyChanged("GroupImage");
        }
    }

    /// <summary>
    /// Creates a collection of groups and items.
    /// </summary>
    public sealed class OptredenDataSource
    {
        //public event EventHandler RecipesLoaded;

        private static OptredenDataSource _optredenDataSource = new OptredenDataSource();

        private ObservableCollection<LineUpDataGroup> _allGroups = new ObservableCollection<LineUpDataGroup>();
        public ObservableCollection<LineUpDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<LineUpDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _optredenDataSource.AllGroups;
        }

        public static LineUpDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _optredenDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public async static Task<OptredenDataItem> GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            if (_optredenDataSource.AllGroups.Where(groep => groep.Items.Count() < 0).Count() > 0) await LoadLocalDataAsync();
            var matches = _optredenDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task LoadRemoteDataAsync()
        {
            if(await GetFestivalData())
            CreateOptredens();
        }

        public static async Task LoadLocalDataAsync()
        {
            //// Retrieve recipe data from Recipes.txt
            //var file = await Package.Current.InstalledLocation.GetFileAsync("Data\\Recipes.txt");
            //var result = await FileIO.ReadTextAsync(file);

            //// Parse the JSON recipe data
            //var recipes = JsonArray.Parse(result);

            //// Convert the JSON objects into RecipeDataItems and RecipeDataGroups
            //CreateBandsAndLineUps(recipes);
            LoadRemoteDataAsync();
        }

        private static async Task<bool> GetFestivalData()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/festival");
                string result = await response.Content.ReadAsStringAsync();

                var stages = new ObservableCollection<Stage>();

                //JsonObject obj = JsonObject.Parse(result);
                JsonObject jsonfest = JsonObject.Parse(result);

                Festival.SingleFestival.Name = jsonfest.GetNamedString("Name");
                App.Current.Resources["AppName"] = Festival.SingleFestival.Name;
                Festival.SingleFestival.StartDate = Convert.ToDateTime(jsonfest.GetNamedString("StartDate"));
                Festival.SingleFestival.EndDate = Convert.ToDateTime(jsonfest.GetNamedString("EndDate"));
                Festival.SingleFestival.ComputeLineUps();
                Festival.SingleFestival.Genres = await GetGenresFromJSON();
                Festival.SingleFestival.Bands = await GetBandsFromJSON();
                Festival.SingleFestival.Stages = await GetStagesFromJSON();
                Festival.SingleFestival.Optredens = await GetOptredensFromJSON();
                
                return true;
            }
            catch (Exception ex)
            {
                new MessageDialog("Could not connect to the server").ShowAsync();
                return false;
            }
        }

        private static async Task<ObservableCollection<Genre>> GetGenresFromJSON()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/festival/genres");
            string result = await response.Content.ReadAsStringAsync();

            var genres = new ObservableCollection<Genre>();

            //JsonObject obj = JsonObject.Parse(result);
            JsonArray jsonoptredens = JsonArray.Parse(result);

            foreach (JsonValue val in jsonoptredens)
            {
                try
                {
                    Genre s = new Genre()
                    {
                        ID = val.GetObject().GetNamedString("ID"),
                        Name = val.GetObject().GetNamedString("Name")
                    };
                    genres.Add(s);
                }
                catch (Exception) { }
            }
            return genres;
        }

        private static async Task<ObservableCollection<Stage>> GetStagesFromJSON()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/festival/stages");
            string result = await response.Content.ReadAsStringAsync();

            var stages = new ObservableCollection<Stage>();

            //JsonObject obj = JsonObject.Parse(result);
            JsonArray jsonoptredens = JsonArray.Parse(result);

            foreach (JsonValue val in jsonoptredens)
            {
                try
                {
                    Stage s = new Stage()
                    {
                        ID = val.GetObject().GetNamedString("ID"),
                        Name = val.GetObject().GetNamedString("Name"),
                        Logo = val.GetObject().GetNamedString("Logo"),
                        StageNumber = Convert.ToInt32(val.GetObject().GetNamedNumber("StageNumber"))
                    };
                    stages.Add(s);
                }
                catch (Exception) { }
            }
            return stages;
        }

        private static async Task<ObservableCollection<Optreden>> GetOptredensFromJSON()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/festival/optredens");
            string result = await response.Content.ReadAsStringAsync();

            var optredens = new ObservableCollection<Optreden>();

            //JsonObject obj = JsonObject.Parse(result);
            JsonArray jsonoptredens = JsonArray.Parse(result);

            foreach (JsonValue val in jsonoptredens)
            {
                try
                {
                    Optreden s = new Optreden()
                    {
                        ID = val.GetObject().GetNamedString("ID"),
                        Band = Festival.SingleFestival.Bands.Where(band => band.ID == val.GetObject().GetNamedString("BandID")).First(),
                        Stage = Festival.SingleFestival.Stages.Where(stage => stage.ID == val.GetObject().GetNamedString("StageID")).First(),
                        From = Convert.ToDateTime(val.GetObject().GetNamedString("From")),
                        Until = Convert.ToDateTime(val.GetObject().GetNamedString("Until")),
                        LineUp = Festival.SingleFestival.LineUps.Where(linup => linup.Dag.Date == Convert.ToDateTime(val.GetObject().GetNamedString("From")).Date).First()
                    };
                    optredens.Add(s);
                }
                catch (Exception) { }
            }
            return optredens;
        }

        private static async Task<ObservableCollection<Band>> GetBandsFromJSON()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/festival/bands");
            string result = await response.Content.ReadAsStringAsync();

            var bands = new ObservableCollection<Band>();

            //JsonObject obj = JsonObject.Parse(result);
            JsonArray jsonbands = JsonArray.Parse(result);

            foreach (JsonValue val in jsonbands)
            {
                try
                {
                    Band s = new Band()
                    {
                        ID = val.GetObject().GetNamedString("ID"),
                        Name = val.GetObject().GetNamedString("Name"),
                        Picture = val.GetObject().GetNamedString("Picture"),
                        Twitter = val.GetObject().GetNamedString("Twitter"),
                        Facebook = val.GetObject().GetNamedString("Facebook"),
                        Description = val.GetObject().GetNamedString("Description"),
                        Genres = new ObservableCollection<Genre>()
                    };
                    foreach (var id in val.GetObject().GetNamedArray("GenreIDs"))
                    {
                        s.Genres.Add(Festival.SingleFestival.Genres.Where(genre => genre.ID == id.GetString()).First());
                    }
                    bands.Add(s);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return bands;
        }

        private static void CreateOptredens()
        {
            foreach (var optreden in Festival.SingleFestival.Optredens)
            {
                OptredenDataItem optredenItem = new OptredenDataItem(optreden.ID,
                    optreden.From,
                    optreden.Until,
                    optreden.Band,
                    optreden.Stage,
                    null);
                            //optredenItem.UniqueId = optreden.ID;
                            //optredenItem.From =optreden.From;
                            //optredenItem.Until = optreden.Until;
                            //optredenItem.Band = optreden.Band;
                            //optredenItem.SetImage(optredenItem.Band.Picture);
                            //optredenItem.SetTileImage(optredenItem.Band.Picture);
                            //optredenItem.Stage = optreden.Stage;
                var group = new LineUpDataGroup(optreden.LineUp);
                var groups = _optredenDataSource.AllGroups.Where(groupr=>groupr.UniqueId==group.UniqueId);
                if(groups.Count()==0)
                {
                    _optredenDataSource.AllGroups.Add(group);
                            optredenItem.Group=group;
                    group.Items.Add(optredenItem);
                }else
                {
                    optredenItem.Group=groups.First();
                    groups.First().Items.Add(optredenItem);
                }
                

                        //case "group":
                        //    var recipeGroup = val.GetObject();

                        //    IJsonValue groupKey;
                        //    if (!recipeGroup.TryGetValue("key", out groupKey))
                        //        continue;

                        //    dag = _recipeDataSource.AllGroups.FirstOrDefault(c => c.UniqueId.Equals(groupKey.GetString()));

                        //    if (dag == null)
                        //        dag = CreateRecipeGroup(recipeGroup);

                        //    band.Group = dag;
                        //    break;
                    
                }
            }

        

        //private static LineUpDataGroup CreateRecipeGroup(JsonObject obj)
        //{
        //    LineUpDataGroup group = new LineUpDataGroup();

        //    foreach (var key in obj.Keys)
        //    {
        //        IJsonValue val;
        //        if (!obj.TryGetValue(key, out val))
        //            continue;

        //        switch (key)
        //        {
        //            case "key":
        //                group.UniqueId = val.GetString();
        //                break;
        //            case "title":
        //                group.Title = val.GetString();
        //                break;
        //            case "shortTitle":
        //                group.ShortTitle = val.GetString();
        //                break;
        //            case "description":
        //                group.Description = val.GetString();
        //                break;
        //            case "backgroundImage":
        //                group.SetImage(val.GetString());
        //                break;
        //            case "groupImage":
        //                group.SetGroupImage(val.GetString());
        //                break;
        //        }
        //    }

        //    _recipeDataSource.AllGroups.Add(group);
        //    return group;
        //}
    }
}
#endregion