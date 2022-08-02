using Prism.Mvvm;

namespace ChessApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Chess App";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
