using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
	public class Note : INotifyPropertyChanged
	{
		private int id;
		[PrimaryKey, AutoIncrement]
		public int Id {
			get { return id; }
			set { id = value; OnPropertyChanged("Id"); }
		}

		private int notebookId;
		[Indexed]
		public int NotebookId {
			get { return notebookId; }
			set { notebookId = value; OnPropertyChanged("NotebookId"); }
		}

		private string title;

		public string Title {
			get { return title; }
			set { title = value; OnPropertyChanged("Title"); }
		}

		private DateTime dateTime;

		public DateTime CreatedTime {
			get { return dateTime; }
			set { dateTime = value; OnPropertyChanged("dateTime"); }
		}

		private DateTime updatedTime;

		public DateTime UdatedTime {
			get { return updatedTime; }
			set { updatedTime = value; OnPropertyChanged("updatedTime"); }
		}

		private string fileLocation;

		public string FileLocation {
			get { return fileLocation; }
			set { fileLocation = value; OnPropertyChanged("FileLocation"); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
