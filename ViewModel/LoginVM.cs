using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class LoginVM
    {
		private User user;

		public User User {
			get { return user; }
			set { user = value; }
		}

		public RegisterCommand RegisterCommand { get; set; }

		public LoginCommand LoginCommand { get; set; }

		public event EventHandler HasLoggedIn;

		public LoginVM()
		{
			RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);

			User = new User();
		}

		public void Login()
		{
			using(SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			{
				connection.CreateTable<User>();

				var user = connection.Table<User>().Where(usr => usr.UserName == User.UserName).FirstOrDefault();

				if(user.Password == User.Password)
				{
					App.UserId = user.Id.ToString();
					HasLoggedIn?.Invoke(this, new EventArgs());
				}
			}
		}

		public void Register()
		{
			using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			{
				connection.CreateTable<User>();

				var result = DatabaseHelper.Insert(User);

				if(result)
				{
					App.UserId = User.Id.ToString();
					HasLoggedIn?.Invoke(this, new EventArgs());
				}
			}
		}
	}
}
