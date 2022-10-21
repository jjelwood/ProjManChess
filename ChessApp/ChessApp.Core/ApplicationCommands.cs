using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand SaveSettingsCommand { get; }
        CompositeCommand NewGame { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand SaveSettingsCommand { get; } = new CompositeCommand();

        public CompositeCommand NewGame { get; } = new CompositeCommand();
    }
}
