using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Jonas_Quiz.DataModels;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Jonas_Quiz
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window, INotifyPropertyChanged
    {

        private int questionsAnswered;

        private Quiz quiz;


        public int QuestionsAnswered
        {
            get { return questionsAnswered; }
            set
            {
                if (questionsAnswered != value)
                {
                    questionsAnswered = value;
                    OnPropertyChanged(nameof(QuestionsAnswered));
                }
            }
        }

        

        



        public QuizWindow()
        {
            InitializeComponent();
            DataContext = this;
            quiz = new Quiz();
            StartQuiz();
        }




        private void StartQuiz()
        {
            SetQuiz(quiz);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string currentQuestionText;
        public string CurrentQuestionText
        {
            get { return currentQuestionText; }
            set
            {
                currentQuestionText = value;
                OnPropertyChanged(nameof(CurrentQuestionText));
            }
        }


        private void LoadNextQuestion()
        {
            currentRandomQuestion = quiz.GetRandomQuestion();

            if (currentRandomQuestion != null)
            {
                CurrentQuestionText = currentRandomQuestion.Statement;

                if (currentRandomQuestion.Options != null && currentRandomQuestion.Options.Any())
                {
                    LabelOption1.Content = currentRandomQuestion.Options[0].Option;
                    LabelOption2.Content = currentRandomQuestion.Options[1].Option;
                    LabelOption3.Content = currentRandomQuestion.Options[2].Option;
                }

                QuestionsAnswered++;
            }
            else
            {
                // Quiz completed or no more questions
                MessageBox.Show("Quiz completed!");
                ProgressBarCorrect.Value = 0; // Reset progress bar
                SetQuiz(quiz); // Optionally, you may want to start a new quiz
            }
        }


        private Question currentRandomQuestion;

        public void SetQuiz(Quiz quiz)
        {
            if (quiz != null && quiz.Questions != null && quiz.Questions.Any())
            {
                if (currentRandomQuestion == null || ProgressBarCorrect.Value == ProgressBarCorrect.Maximum)
                {
                    currentRandomQuestion = quiz.GetRandomQuestion();
                    CurrentQuestionText = currentRandomQuestion.Statement;

                    if (currentRandomQuestion.Options != null && currentRandomQuestion.Options.Any())
                    {
                        LabelOption1.Content = currentRandomQuestion.Options[0].Option;
                        LabelOption2.Content = currentRandomQuestion.Options[1].Option;
                        LabelOption3.Content = currentRandomQuestion.Options[2].Option;
                    }

                    QuestionsAnswered = 0;
                }
            }
        }





        private void Option1Button_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("1");
        }

        private void Option2Button_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("2");
        }

        private void Option3Button_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("3");
        }







        private void CheckAnswer(string selectedOption)
        {
            Debug.WriteLine($"Checking answer for option: {selectedOption}");

            if (quiz != null && currentRandomQuestion != null)
            {
                bool isCorrect = currentRandomQuestion.IsCorrectOption(selectedOption);
                Debug.WriteLine($"Is the answer correct? {isCorrect}");

                if (isCorrect)
                {
                    MessageBox.Show("Answer is correct!", "Correct Answer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Answer is incorrect.", "Incorrect Answer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                UpdateProgressBar(isCorrect);
                LoadNextQuestion();
            }
            else
            {
                MessageBox.Show("Quiz or current question is not initialized.");
            }
        }










        private void UpdateProgressBar(bool isCorrect)
        {
            if (isCorrect)
            {
                ProgressBarCorrect.Value += 10; // Increment the progress bar value by 10 for each correct answer
                Debug.WriteLine("Answer is correct.");
            }
            else
            {
                Debug.WriteLine("Answer is incorrect.");
            }

            if (ProgressBarCorrect.Value == ProgressBarCorrect.Maximum)
            {
                MessageBox.Show("Quiz completed!"); // You can customize this message as needed
                                                    // Optionally, you may want to reset the progress bar and start a new quiz
                ProgressBarCorrect.Value = 0;
                SetQuiz(quiz);
            }
        }



        private void BackToMenu_Click(object sender, RoutedEventArgs e)
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
