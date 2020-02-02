using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM LoginVM { get; set; }

        public event EventHandler CanExecuteChanged;

        public LoginCommand(LoginVM loginVM)
        {
            LoginVM = loginVM ?? throw new ArgumentNullException(nameof(loginVM));
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as User;

            //return (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password));

            return true;
        }

        public void Execute(object parameter)
        {
            LoginVM.Login();
        }
    }
}
