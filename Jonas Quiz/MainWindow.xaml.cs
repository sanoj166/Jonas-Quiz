using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Jonas_Quiz.DataModels;
using Newtonsoft.Json;

namespace Jonas_Quiz
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDataFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyQuiz");
                string quizFilePath = System.IO.Path.Combine(appDataFolderPath, "quiz.json");


                if (!Directory.Exists(appDataFolderPath))
                {
                    Directory.CreateDirectory(appDataFolderPath);

                    
                    string sourceFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyQuiz", "quiz.json");
                    System.IO.File.Copy(sourceFilePath, quizFilePath, true);
                }

                if (File.Exists(quizFilePath))
                {
                    string json = File.ReadAllText(quizFilePath);

                    Quiz quiz = JsonConvert.DeserializeObject<Quiz>(json);

                    if (quiz != null)
                    {
                        MessageBox.Show("Quiz loaded successfully!");

                        QuizWindow quizWindow = new QuizWindow();
                        quizWindow.SetQuiz(quiz);
                        quizWindow.PropertyChanged += QuizWindow_PropertyChanged;
                        quizWindow.StartQuiz();
                        quizWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to deserialize the quiz!");
                    }
                }
                else
                {
                    MessageBox.Show("Quiz file not found. Please make sure the quiz file is in the 'MyQuiz' folder.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }





        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void EditQuizButton_Click(object sender, RoutedEventArgs e)
        {
            EditQuizWindow editQuizWindow = new EditQuizWindow();
            editQuizWindow.Owner = this;
            editQuizWindow.ShowDialog();
        }



        private void QuizWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          
        }




    }
}

