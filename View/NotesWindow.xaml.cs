using NotesApp.Model;
using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        SpeechRecognitionEngine recognizer;

        NotesVM notesVM;

        public NotesWindow()
        {
            InitializeComponent();

            notesVM = new NotesVM();
            container.DataContext = notesVM;
            notesVM.SelectedNotecChanged += notesVM_SelectedNoteChanged;

            SetSpeechRecognizer();

            var fontFamillies = Fonts.SystemFontFamilies.OrderBy(familly => familly.Source);
            FontFamillyComboBox.ItemsSource = fontFamillies;

            List<double> fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 28, 48, 56, 72 };
            FontSizeComboBox.ItemsSource = fontSizes;
        }

        private void notesVM_SelectedNoteChanged(object sender, EventArgs e)
        {
            Note note = notesVM.SelectedNote;

            contentRichTextBox.Document.Blocks.Clear();

            if(note != null && !string.IsNullOrEmpty(note.FileLocation))
            {
                FileStream fs = new FileStream(note.FileLocation, FileMode.Open);
                TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
                fs.Close();
            }
        }

        private void SetSpeechRecognizer()
        {
            RecognizerInfo culture = SpeechRecognitionEngine.InstalledRecognizers().FirstOrDefault();

            //var threadCulture = Thread.CurrentThread.CurrentCulture;

            //var currentCulture = SpeechRecognitionEngine.InstalledRecognizers()
            //    .Where(rec => rec.Culture.Equals(Thread.CurrentThread.CurrentCulture))
            //    .FirstOrDefault();

            recognizer = new SpeechRecognitionEngine(culture);

            GrammarBuilder builder = new GrammarBuilder();
            builder.Culture = recognizer.RecognizerInfo.Culture;
            builder.AppendDictation();
            Grammar grammaer = new Grammar(builder);

            recognizer.LoadGrammar(grammaer);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if(string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;

            contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(recognizedText)));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfCharacters = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd).Text.Length;

            statusTextBlock.Text = $"Document Length: {amountOfCharacters} characters";
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = ((ToggleButton)sender).IsChecked ?? false;

            if (isButtonChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold); 
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = ((ToggleButton)sender).IsChecked ?? false;

            if (!isButtonChecked)
            {
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                recognizer.RecognizeAsyncStop();
            }
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = selectedState != DependencyProperty.UnsetValue && selectedState.Equals(FontWeights.Bold);

            selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicButton.IsChecked = selectedState != DependencyProperty.UnsetValue && selectedState.Equals(FontStyles.Italic);

            selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = selectedState != DependencyProperty.UnsetValue && selectedState.Equals(TextDecorations.Underline);

            FontFamillyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontSizeComboBox.Text = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton)sender;
            bool isButtonChecked = button.IsChecked ?? false;

            if (isButtonChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = ((ToggleButton)sender).IsChecked ?? false;

            if (isButtonChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void FontFamillyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FontFamillyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamillyComboBox.SelectedItem);
            }
        }

        private void FontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.Text);
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            string rtfFile = Path.Combine(Environment.CurrentDirectory, $"{notesVM.SelectedNote.Id}.rtf");
            notesVM.SelectedNote.FileLocation = rtfFile;

            FileStream fs = new FileStream(rtfFile, FileMode.Create);
            TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
            range.Save(fs, DataFormats.Rtf);
            fs.Close();

            notesVM.UpdateSelectedNote();
        }
    }
}
