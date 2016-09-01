using ContosoCookbook.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 그룹화된 항목 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234231에 나와 있습니다.

namespace ContosoCookbook
{
    /// <summary>
    /// 그룹화된 항목 컬렉션을 표시하는 페이지입니다.
    /// </summary>
    public sealed partial class GroupedItemsPage : ContosoCookbook.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
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
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = RecipeDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
            this.groupGridView.ItemsSource =
                this.groupedItemsViewSource.View.CollectionGroups;
        }

        /// <summary>
        /// 그룹 머리글을 클릭할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">선택한 그룹의 그룹 머리글로 사용되는 단추입니다.</param>
        /// <param name="e">클릭이 시작된 방법을 설명하는 이벤트 데이터입니다.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // 단추 인스턴스가 나타내는 그룹을 결정합니다.
            var group = (sender as FrameworkElement).DataContext;

            // 해당하는 대상 페이지로 이동합니다. 필요한 정보를 탐색 매개 변수로
            // 전달하여 새 페이지를 구성합니다.
            this.Frame.Navigate(typeof(GroupDetailPage), ((RecipeDataGroup)group).UniqueId);
        }

        /// <summary>
        /// 그룹 내의 항목을 클릭할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">클릭된 항목을 표시하는
        /// GridView(또는 응용 프로그램이 기본 뷰 상태인 경우 ListView)입니다.</param>
        /// <param name="e">클릭된 항목을 설명하는 이벤트 데이터입니다.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 해당하는 대상 페이지로 이동합니다. 필요한 정보를 탐색 매개 변수로
            // 전달하여 새 페이지를 구성합니다.
            var itemId = ((RecipeDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }
    }
}
