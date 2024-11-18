using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class ProductView : UserControl
    {
        public ProductView(ProductViewModel productViewModel)
        {
            InitializeComponent();
            DataContext = productViewModel ?? throw new ArgumentNullException(nameof(productViewModel));
        }

    }
}