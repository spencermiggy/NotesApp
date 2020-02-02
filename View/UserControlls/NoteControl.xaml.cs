using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotesApp.View.UserControlls
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {


        public Note Note {
            get { return (Note)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(NoteControl), new PropertyMetadata(null, SetValue));

        private static void SetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NoteControl note = (NoteControl)d;

            if(note != null)
            {
                note.titleTextBlock.Text = ((Note)e.NewValue).Title;
                note.editedTextBlock.Text = ((Note)e.NewValue).UdatedTime.ToShortDateString();
                note.contentTextBlock.Text = ((Note)e.NewValue).Title;
            }
        }

        public NoteControl()
        {
            InitializeComponent();
        }
    }
}
