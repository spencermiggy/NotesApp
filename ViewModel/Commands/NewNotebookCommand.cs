using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class NewNotebookCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public NewNotebookCommand(NotesVM vM)
        {
            VM = vM ?? throw new ArgumentNullException(nameof(vM));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.CreateNotebook();
        }
    }
}
