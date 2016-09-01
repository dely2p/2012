﻿using System;
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
using Windows.Storage;

// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace ContosoCookbook
{
    public sealed partial class PreferenceUserControl : UserControl
    {
        public PreferenceUserControl()
        {
            this.InitializeComponent();
         
        }
        private void OnToggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.RoamingSettings.Values["Remember"] = Remember.IsOn;
        }
    }
}
