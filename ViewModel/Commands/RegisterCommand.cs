using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public RegisterCommand(LoginVM vM)
        {
            VM = vM ?? throw new ArgumentNullException(nameof(vM));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var user = parameter as Users;

            //return (string.IsNullOrEmpty(user.UserName)
            //   || string.IsNullOrEmpty(user.Password)
            //   || string.IsNullOrEmpty(user.Email)
            //   || string.IsNullOrEmpty(user.LastName)
            //   || string.IsNullOrEmpty(user.Name));

            return true;
        }

        public void Execute(object parameter)
        {
            VM.Register();
        }
    }
}
