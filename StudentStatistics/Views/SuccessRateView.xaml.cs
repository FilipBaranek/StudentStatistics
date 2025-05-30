﻿using System.Windows.Controls;
using StudentStatistics.Services;
using StudentStatistics.ViewModels;

namespace StudentStatistics.Views
{
    public partial class SuccessRateView : UserControl
    {
        public SuccessRateView(Router router)
        {
            InitializeComponent();

            DataContext = new SuccessRateViewModel(router);
        }
    }
}
