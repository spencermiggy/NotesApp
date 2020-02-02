using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
		private bool isEditing;

		public bool IsEditing {
			get { return isEditing; }
			set 
			{
				isEditing = value;
				OnPropertyChanged("IsEditing");
			}
		}

		public ObservableCollection<Notebook> Notebooks { get; set; }

		private Notebook selectedNotebook;

		public Notebook SelectedNotebook {
			get { return selectedNotebook; }
			set
			{
				selectedNotebook = value;
				ReadNotes();
			}
		}

		private Note selectedNote;

		public Note SelectedNote {
			get { return selectedNote; }
			set 
			{
				selectedNote = value;
				SelectedNotecChanged?.Invoke(this, new EventArgs());
			}
		}


		public ObservableCollection<Note> Notes { get; set; }

		public NewNotebookCommand NewNotebookCommand { get; set; }

		public BeginEditCommand BeginEditCommand { get; set; }

		public HasEditedCommand HasEditedCommand { get; set; }

		public NewNoteCommand NewNoteCommand { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler SelectedNotecChanged;

		public NotesVM()
		{
			IsEditing = false;

			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			BeginEditCommand = new BeginEditCommand(this);
			HasEditedCommand = new HasEditedCommand(this);

			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			ReadNotebooks();
			ReadNotes();
		}

		public void CreateNote(int notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
				CreatedTime = DateTime.Now,
				UdatedTime = DateTime.Now,
				Title = "New note"
			};

			//DatabaseHelper.Insert<Note>(newNote);
			// is same as:
			DatabaseHelper.Insert(newNote);

			ReadNotes();
		}

		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "New notebook",
				UserId = int.Parse(App.UserId)
			};

			DatabaseHelper.Insert(newNotebook);
			ReadNotebooks();
		}

		public void ReadNotebooks()
		{
			using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			{
				var notebooks = connection.Table<Notebook>().ToList();

				Notebooks.Clear();
				notebooks.ForEach(notebook =>
				{
					Notebooks.Add(notebook);
				});
			}
		}

		public void ReadNotes()
		{
			using(SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
			{

				if (SelectedNotebook != null)
				{
					var notes = connection.Table<Note>().Where(note => note.NotebookId == SelectedNotebook.Id).ToList();

					Notes.Clear();
					notes.ForEach(note =>
					{
						Notes.Add(note);
					});
				}
			}
		}

		public void StartEditing()
		{
			IsEditing = true;
		}

		public void HasRenamed(Notebook notebook)
		{
			if(notebook != null)
			{
				DatabaseHelper.Update(notebook);
				IsEditing = false;
				ReadNotebooks();
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void UpdateSelectedNote()
		{
			DatabaseHelper.Update(SelectedNote);
		}
	}
}
