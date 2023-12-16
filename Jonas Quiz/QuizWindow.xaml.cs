using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Jonas_Quiz.DataModels;
using Newtonsoft.Json;

namespace Jonas_Quiz
{
    public partial class QuizWindow : Window, INotifyPropertyChanged
    {
        private int questionsAnswered;
        private Quiz quiz;
        private string currentQuestionText = string.Empty;

        public int QuestionsAnswered
        {
            get { return questionsAnswered; }
            set
            {
                if (questionsAnswered != value)
                {
                    questionsAnswered = value;
                    OnPropertyChanged(nameof(QuestionsAnswered));
                    OnPropertyChanged(nameof(QuestionsAnsweredLabel));

                }
            }
        }

        public string QuestionsAnsweredLabel
        {
            get { return $"Questions Answered: {QuestionsAnswered}/10"; }
        }


        public string CurrentQuestionText
        {
            get { return currentQuestionText; }
            set
            {
                if (currentQuestionText != value)
                {
                    currentQuestionText = value;
                    OnPropertyChanged(nameof(CurrentQuestionText));
                }
            }
        }

        public QuizWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            StartQuiz();
            QuestionsAnswered = 0;
        }

        public void StartQuiz()
        {
            string quizFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyQuiz", "quiz.json");
            string json = File.ReadAllText(quizFilePath);

            quiz = JsonConvert.DeserializeObject<Quiz>(json);

            ShuffleQuestions();

            currentIndex = 0;
            QuestionsAnswered = 0;
            OnPropertyChanged(nameof(QuestionsAnswered));
            LoadNextQuestion();

            ProgressBarCorrect.Value = 0;
            int initialPercentage = (int)Math.Round((ProgressBarCorrect.Value / ProgressBarCorrect.Maximum) * 100);
            ProgressBarCorrect.ToolTip = $"{initialPercentage}% Correct";
            PercentageTextBlock.Text = $"{initialPercentage}%";
        }





        private void LoadNextQuestion()
        {
            if (QuestionsAnswered < 10)
            {
                currentRandomQuestion = quiz.GetNextQuestion();

                if (currentRandomQuestion != null)
                {
                    CurrentQuestionText = currentRandomQuestion.Statement;

                    if (currentRandomQuestion.Options != null && currentRandomQuestion.Options.Any())
                    {
                        LabelOption1.Content = currentRandomQuestion.Options[0].Option;
                        LabelOption2.Content = currentRandomQuestion.Options[1].Option;
                        LabelOption3.Content = currentRandomQuestion.Options[2].Option;
                    }
                    else
                    {
                        MessageBox.Show("Error loading options for the question. Please try again.");
                        ResetQuiz();
                    }
                }
                else
                {
                    MessageBox.Show("Error loading question. Please try again.");
                    ResetQuiz();
                }
            }
            else
            {
                int correctAnswers = (int)Math.Round((ProgressBarCorrect.Value / ProgressBarCorrect.Maximum) * 10);
                MessageBox.Show($"Quiz completed! You answered {correctAnswers} questions correctly.", "Quiz Completed");
                ResetQuiz();
                Close();
            }
        }



        private void ResetQuiz()
        {
            ProgressBarCorrect.Value = 0;
            SetQuiz(quiz);
            ClearUI();
            Debug.WriteLine("Quiz reset.");
        }

        private void ClearUI()
        {
            CurrentQuestionText = string.Empty;
            LabelOption1.Content = string.Empty;
            LabelOption2.Content = string.Empty;
            LabelOption3.Content = string.Empty;
        }

        private Question? currentRandomQuestion;

        public void SetQuiz(Quiz newQuiz)
        {
            if (newQuiz != null && newQuiz.Questions != null && newQuiz.Questions.Any())
            {
                quiz = newQuiz;
                currentRandomQuestion = quiz.GetNextQuestion();
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
                QuestionsAnswered++;
                bool isCorrect = currentRandomQuestion.IsCorrectOption(selectedOption);
                Debug.WriteLine($"Is the answer correct? {isCorrect}");

                if (isCorrect)
                {
                    MessageBox.Show("Answer is correct!");
                    UpdateProgressBar(true);
                }
                else
                {
                    MessageBox.Show("Incorrect answer!");
                }

                LoadNextQuestion(); 

                Debug.WriteLine("After loading next question.");
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
                ProgressBarCorrect.Value += 10;
                Debug.WriteLine("Answer is correct.");
            }
            else
            {
                Debug.WriteLine("Answer is incorrect.");
            }

            int correctAnswers = (int)Math.Round((ProgressBarCorrect.Value / ProgressBarCorrect.Maximum) * 10);
            int correctPercentage = (int)Math.Round((ProgressBarCorrect.Value / ProgressBarCorrect.Maximum) * 100);

            ProgressBarCorrect.ToolTip = $"{correctPercentage}% Correct";
            PercentageTextBlock.Text = $"{correctPercentage}%";

            if (ProgressBarCorrect.Value == ProgressBarCorrect.Maximum)
            {
                MessageBox.Show($"Quiz completed! You answered {correctAnswers}/10 questions correctly.", "Quiz Completed");
                ProgressBarCorrect.Value = 0;
                Close();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int currentIndex = 0;

        private void ShuffleQuestions()
        {
            quiz.ShuffleQuestions();
        }

        
    }
}
