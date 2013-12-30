﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DemoApp.Data;
using PortableClassLibrary.Model;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace DemoApp
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : DemoApp.Common.LayoutAwarePage
    {

        //Collection of RecipeDataItem collections representing search results
        private Dictionary<string, ObservableCollection<OptredenDataItem>> _results = new Dictionary<string, ObservableCollection<OptredenDataItem>>();

        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

        //You can search by name of the band or by genre, if the querytext matches a genre, all bands of that genre are shown, bands that match are shown anyway
        //filtering is by genre

        #region search
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var queryText = navigationParameter as String;

            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       Only the first filter, typically "All", should pass true as a third argument in
            //       order to start in an active state.  Results for the active filter are provided
            //       in Filter_SelectionChanged below.

            var filterList = new List<Filter>();
            

            //search recipes and tabulate results
            IEnumerable<LineUpDataGroup> groups= OptredenDataSource.GetGroups("AllGroups");
            string query = queryText.ToLower();
            ObservableCollection<OptredenDataItem> all = new ObservableCollection<OptredenDataItem>();
            foreach (var groupie in groups)
                foreach (var optreden in groupie.Items)
                    all.Add(optreden);
                    
            //filterList.Add(new Filter("All", all.Count(), true));
            
            var bands =Festival.SingleFestival.Bands;
            foreach (Genre genre in Festival.SingleFestival.Genres)
            {
                var optredens =new ObservableCollection<OptredenDataItem>();
                if(genre.Name.ToLower().Contains(query))
                   foreach(Band band in bands.Where(ba=>ba.Genres.Where(ge=>ge.ID==genre.ID).Count()>0))
                       foreach(OptredenDataItem optreden in all.Where(optredenr=>optredenr.Band.ID==band.ID))
                        optredens.Add(optreden);
                _results.Add(genre.Name, optredens);
            }
                
            foreach(Band band in bands)
            {
                foreach(Genre genre in band.Genres)
                {
                    foreach(OptredenDataItem optreden in all.Where(optredenr=>optredenr.Band.ID==band.ID))
                        if(!_results[genre.Name].Contains(optreden)&&optreden.Band.Name.ToLower().Contains(query))
                            _results[genre.Name].Add(optreden);
                }
               
                
            }
            var allval = new ObservableCollection<OptredenDataItem>();
            foreach (KeyValuePair<string, ObservableCollection<OptredenDataItem>> keypair in _results)
                foreach(var optreden in keypair.Value)
                    if(!allval.Contains(optreden))
                    allval.Add(optreden);
            _results.Add("All", allval);
            filterList.Add(new Filter("All", allval.Count(), true));
            foreach (KeyValuePair<string, ObservableCollection<OptredenDataItem>> keypair in _results)
            {
                int count =keypair.Value.Count();
                if(count>0&&keypair.Key!="All")
                filterList.Add(new Filter(keypair.Key, count, false));
            }

            // Communicate results through the view model
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }
        #endregion

        /// <summary>
        /// Invoked when a filter is selected using the ComboBox in snapped view state.
        /// </summary>
        /// <param name="sender">The ComboBox instance.</param>
        /// <param name="e">Event data describing how the selected filter was changed.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Determine what filter was selected
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                // Mirror the results into the corresponding Filter object to allow the
                // RadioButton representation used when not snapped to reflect the change
                selectedFilter.Active = true;

                // TODO: Respond to the change in active filter by setting this.DefaultViewModel["Results"]
                //       to a collection of items with bindable Image, Title, Subtitle, and Description properties

                // Ensure results are found
                this.DefaultViewModel["Results"] = _results[selectedFilter.Name];
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // Display informational text when there are no search results.
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// Invoked when a filter is selected using a RadioButton when not snapped.
        /// </summary>
        /// <param name="sender">The selected RadioButton instance.</param>
        /// <param name="e">Event data describing how the RadioButton was selected.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // Mirror the change into the CollectionViewSource used by the corresponding ComboBox
            // to ensure that the change is reflected when snapped
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        /// <summary>
        /// View model describing one of the filters available for viewing search results.
        /// </summary>
        private sealed class Filter : DemoApp.Common.BindableBase
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemDetailPage),((OptredenDataItem)e.ClickedItem).UniqueId);
        }
    }
}
