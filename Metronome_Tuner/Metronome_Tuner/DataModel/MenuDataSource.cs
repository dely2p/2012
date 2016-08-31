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

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace Metronome_Tuner.Data
{
    /// <summary>
    /// Base class for <see cref="MenuDataItem"/> and <see cref="MenuDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class MenuDataCommon : Metronome_Tuner.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public MenuDataCommon(String uniqueId, String imagePath)
        {
            this._uniqueId = uniqueId;
            this._imagePath = imagePath;
        }
        
        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }
        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(MenuDataCommon._baseUri, this._imagePath));
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
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class MenuDataItem : MenuDataCommon
    {
        public MenuDataItem(String uniqueId, String imagePath, MenuDataGroup group)
            : base(uniqueId,imagePath)
        {
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
        //    get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private MenuDataGroup _group;
        public MenuDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class MenuDataGroup : MenuDataCommon
    {
        public MenuDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId,/* title, subtitle, */imagePath /*description*/)
        {
        }

        private ObservableCollection<MenuDataItem> _items = new ObservableCollection<MenuDataItem>();
        public ObservableCollection<MenuDataItem> Items
        {
            get { return this._items; }
        }

        public IEnumerable<MenuDataItem> TopItems
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
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class MenuDataSource
    {
        private static MenuDataSource _MenuDataSource = new MenuDataSource();

        private ObservableCollection<MenuDataGroup> _allGroups = new ObservableCollection<MenuDataGroup>();
        public ObservableCollection<MenuDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<MenuDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _MenuDataSource.AllGroups;
        }

        public static MenuDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _MenuDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static MenuDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _MenuDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        

        public MenuDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new MenuDataGroup("Group-1",
                    "Group Title: 1",
                    "Group Subtitle: 1",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group1.Items.Add(new MenuDataItem("Group-1-Item-1", "Assets/M_button.png", group1));
            group1.Items.Add(new MenuDataItem("Group-1-Item-2","Assets/T_button.png",group1));
            this.AllGroups.Add(group1);
        }
    }
}
