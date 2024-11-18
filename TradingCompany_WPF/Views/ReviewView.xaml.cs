using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class ReviewView : UserControl
    {
        public ReviewView(ReviewViewModel reviewViewModel)
        {
            InitializeComponent();
            DataContext = reviewViewModel;
        }
    }
}