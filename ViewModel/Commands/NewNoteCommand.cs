using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public NewNoteCommand(NotesVM vM)
        {
            VM = vM ?? throw new ArgumentNullException(nameof(vM));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            Notebook selectedNotebook = parameter as Notebook;
            return selectedNotebook != null;
        }

        public void Execute(object parameter)
        {
            Notebook selectedNotebook = parameter as Notebook;
            VM.CreateNote(selectedNotebook.Id);
        }
    }
}
