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
                string quizFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyQuiz", "quiz.json");


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

                        
                        quizWindow.SetQuiz(quiz);

                        
                        quizWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to deserialize the quiz!");
                    }
                }
                else
                {
                    MessageBox.Show("Quiz file not found!");
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



        private void QuizWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          
        }




    }
}

