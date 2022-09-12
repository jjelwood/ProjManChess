using ChessApp.Core;
using ChessApp.Game;
using ChessApp.Menu;
using ChessApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace ChessApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GameModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}
