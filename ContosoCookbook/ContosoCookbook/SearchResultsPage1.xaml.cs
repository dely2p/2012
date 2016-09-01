using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using ContosoCookbook.Data;
// 계약 검색 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234240에 나와 있습니다.

namespace ContosoCookbook
{
    /// <summary>
    /// 이 페이지에서는 이 응용 프로그램을 대상으로 전역 검색을 수행한 경우의 검색 결과를 표시합니다.
    /// </summary>
    public sealed partial class SearchResultsPage1 : ContosoCookbook.Common.LayoutAwarePage
    {
        private Dictionary<string, List<RecipeDataItem>> _results = new
        Dictionary<string, List<RecipeDataItem>>();
        private UIElement _previousContent;
        private ApplicationExecutionState _previousExecutionState;

        public SearchResultsPage1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Determines how best to support navigation back to the previous application state.
        /// </summary>
        public static void Activate(String queryText, ApplicationExecutionState previousExecutionState)
        {
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame != null)
            {
                // If the app is already running and uses top-level frame navigation we can just
                // navigate to the search results
                frame.Navigate(typeof(SearchResultsPage1), queryText);
            }
            else
            {
                // Otherwise bypass navigation and provide the tools needed to emulate the back stack
                SearchResultsPage1 page = new SearchResultsPage1();
                page._previousContent = previousContent;
                page._previousExecutionState = previousExecutionState;
                page.LoadState(queryText, null);
                Window.Current.Content = page;
            }

            // Either way, active the window
            Window.Current.Activate();
        }
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the page showing the recipe that was clicked
            this.Frame.Navigate(typeof(ItemDetailPage), ((RecipeDataItem)e.ClickedItem).UniqueId);
        }

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
            //       사용자가 선택할 수 있는 결과 범주 목록을 만듭니다.
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       활성 상태에서 시작하려면 첫 번째 필터(일반적으로 "모두")만 세 번째 인수로 true를
            //       전달해야 합니다. 활성 필터의 결과가 아래의
            //       Filter_SelectionChanged에 제공됩니다.

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // Search recipes and tabulate results
            var groups = RecipeDataSource.GetGroups("AllGroups");
            string query = queryText.ToLower();
            var all = new List<RecipeDataItem>();
            _results.Add("All", all);

            foreach (var group in groups)
            {
                var items = new List<RecipeDataItem>();
                _results.Add(group.Title, items);

                foreach (var item in group.Items)
                {
                    if (item.Title.ToLower().Contains(query) || item.Directions.ToLower().Contains(query))
                    {
                        all.Add(item);
                        items.Add(item);
                    }
                }

                filterList.Add(new Filter(group.Title, items.Count, false));
            }

            filterList[0].Count = all.Count;
            // 뷰 모델을 통해 결과를 전달합니다.
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["CanGoBack"] = this._previousContent != null;
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// [뒤로] 단추를 누를 때 호출됩니다.
        /// </summary>
        /// <param name="sender">[뒤로] 단추를 나타내는 단추 인스턴스입니다.</param>
        /// <param name="e">단추를 클릭한 방법을 설명하는 이벤트 데이터입니다.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            // 응용 프로그램을 검색 결과가 요청되기 전의 상태로 되돌립니다.
            if (this.Frame != null && this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else if (this._previousContent != null)
            {
                Window.Current.Content = this._previousContent;
            }
            else
            {
                // TODO: invoke the app's normal launch behavior, using this._previousExecutionState
                //       as appropriate.  Exact details can vary from app to app, which is why an
                //       implementation isn't included in the Search Contract template.  Typically
                //       this method and OnLaunched in App.xaml.cs can call a common method.
            }
        }

        /// <summary>
        /// 기본 뷰 상태에서 ComboBox를 사용하여 필터를 선택할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">ComboBox 인스턴스입니다.</param>
        /// <param name="e">선택한 필터가 변경된 방법을 설명하는 이벤트 데이터입니다.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 선택된 필터가 무엇인지 결정합니다.
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                // 해당하는 필터 개체에 결과를 미러링하여
                // 기본 뷰 상태가 아닐 때 사용되는 RadioButton 표현에 변경이 반영될 수 있게 합니다.
                selectedFilter.Active = true;

                // TODO: this.DefaultViewModel["Results"]을
                //       to a collection of items with bindable Image, Title, Subtitle, and Description properties
                this.DefaultViewModel["Results"] = _results[selectedFilter.Name];
                // Ensure results are found
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
        /// 기본 뷰가 아닌 경우 RadioButton을 사용하여 필터를 선택할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">선택한 RadioButton 인스턴스입니다.</param>
        /// <param name="e">RadioButton을 선택한 방법을 설명하는 이벤트 데이터입니다.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // 기본 뷰 상태일 때 변경이 반영되도록 해당하는 ComboBox에서 사용된 CollectionViewSource에
            // 변경을 미러링합니다.
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        /// <summary>
        /// 검색 결과를 볼 때 사용할 수 있는 필터 중 하나를 설명하는 뷰 모델입니다.
        /// </summary>
        private sealed class Filter : ContosoCookbook.Common.BindableBase
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
    }
}
