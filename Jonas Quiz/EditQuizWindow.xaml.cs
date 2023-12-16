using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Jonas_Quiz.DataModels;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Jonas_Quiz
{
    public partial class EditQuizWindow : Window
    {
        private Quiz _quiz;
        private Question _selectedQuestion;

        public EditQuizWindow()
        {
            InitializeComponent();
            _quiz = LoadQuiz();
            QuestionsListBox.ItemsSource = _quiz.Questions;
        }

        private Quiz LoadQuiz()
        {
            // Load the quiz from the JSON file
            string quizFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyQuiz", "quiz.json");
            if (File.Exists(quizFilePath))
            {
                string json = File.ReadAllText(quizFilePath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Quiz>(json);
            }
            else
            {
                // If the file doesn't exist, create a new quiz
                return new Quiz("Default Quiz Title");
            }
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuestion = (Question)QuestionsListBox.SelectedItem;
            if (_selectedQuestion != null)
            {
                UpdateQuestionDetails();
            }
        }

        private void UpdateQuestionDetails()
        {
            if (_selectedQuestion != null)
            {
                StatementTextBox.Text = _selectedQuestion.Statement;

                
                TextBox[] answerTextBoxes = { Answer1TextBox, Answer2TextBox, Answer3TextBox };

                for (int i = 0; i < Math.Min(answerTextBoxes.Length, _selectedQuestion.Options.Count); i++)
                {
                    answerTextBoxes[i].Text = _selectedQuestion.Options[i].Option;
                }

                
                CorrectOptionComboBox.ItemsSource = _selectedQuestion.Options.Select((option, index) => new { Index = index + 1, Option = option.Option });
                CorrectOptionComboBox.SelectedIndex = _selectedQuestion.CorrectAnswer.OptionNumber - 1;
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuestion != null)
            {
                
                _selectedQuestion.Statement = StatementTextBox.Text;

                for (int i = 0; i < _selectedQuestion.Options.Count; i++)
                {
                    TextBox answerTextBox = FindName($"Answer{i + 1}TextBox") as TextBox;
                    if (answerTextBox != null)
                    {
                        _selectedQuestion.Options[i].Option = answerTextBox.Text;
                    }
                }

                
                QuestionsListBox.Items.Refresh();

                
                SaveQuiz();

                
                if (Owner is QuizWindow quizWindow)
                {
                    quizWindow.SetQuiz(_quiz);
                }
            }
        }


        private void SaveQuiz()
        {
            string quizFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyQuiz", "quiz.json");

            string json = JsonConvert.SerializeObject(_quiz);
            File.WriteAllText(quizFilePath, json);
        }







        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.Show();
                mainWindow.Focus();
            }
            else
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }

            Close(); 
        }

    }
}
