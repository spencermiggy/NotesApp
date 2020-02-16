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
		private Users user;

		public Users User {
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

			User = new Users();
		}

		public async void Login()
		{
			//using(SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			//{
			//	connection.CreateTable<Users>();

			//	var user = connection.Table<Users>().Where(usr => usr.UserName == User.UserName).FirstOrDefault();

			//	if(user.Password == User.Password)
			//	{
			//		App.UserId = user.Id;
			//		HasLoggedIn?.Invoke(this, new EventArgs());
			//	}
			//}

			try
			{
				var user = (await App.MobileServiceClient.GetTable<Users>().Where(u => User.UserName == u.UserName)
						.ToListAsync()).FirstOrDefault();

				if(user.Password == User.Password)
				{
					App.UserId = user.Id;
					HasLoggedIn?.Invoke(this, new EventArgs());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		public async void Register()
		{
			//using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			//{
			//	connection.CreateTable<User>();

			//	var result = DatabaseHelper.Insert(User);

			//	if(result)
			//	{
			//		App.UserId = User.Id.ToString();
			//		HasLoggedIn?.Invoke(this, new EventArgs());
			//	}
			//}

			try
			{
				await App.MobileServiceClient.GetTable<Users>().InsertAsync(User);
				App.UserId = User.Id.ToString();
				HasLoggedIn?.Invoke(this, new EventArgs());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
