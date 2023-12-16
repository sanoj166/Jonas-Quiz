using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jonas_Quiz.DataModels
{
    public class Quiz
    {
        private List<Question> _questions;
        private string _title = string.Empty;

        
        public IEnumerable<Question> Questions => _questions;
        public string Title => _title;

        private int currentIndex = 0;

        public Quiz()
        {
            _questions = new List<Question>();
        }

        public Quiz(string title)
        {
            _title = title;
            _questions = new List<Question>();
        }

        public void StartQuiz()
        {
            ShuffleQuestions();
            currentIndex = 0;
        }

        public void ShuffleQuestions()
        {
            _questions = _questions.OrderBy(q => Guid.NewGuid()).ToList();
        }



        public Question? GetNextQuestion()
        {
            if (currentIndex < _questions.Count)
            {
                return _questions[currentIndex++];
            }
            else
            {
                return null; 
            }
        }

        public void AddQuestion(string statement, int correctAnswer, params string[] answers)
        {
            var updatedQuestions = new List<Question>(_questions);

            Question.Answer correctAnswerObj = new Question.Answer
            {
                OptionNumber = correctAnswer,
                Option = answers[correctAnswer - 1]
            };

            List<Question.Answer> answerOptions = new List<Question.Answer>();
            for (int i = 0; i < answers.Length; i++)
            {
                answerOptions.Add(new Question.Answer
                {
                    OptionNumber = i + 1,
                    Option = answers[i]
                });
            }

            Question question = new Question
            {
                Statement = statement,
                CorrectAnswer = correctAnswerObj,
                Options = answerOptions
            };

            updatedQuestions.Add(question);

            _questions = updatedQuestions;
        }

        public void RemoveQuestion(int index)
        {
            var updatedQuestions = new List<Question>(_questions);

            if (index >= 0 && index < updatedQuestions.Count)
            {
                updatedQuestions.RemoveAt(index);
            }

            _questions = updatedQuestions;
        }
        public bool CheckAnswer(Question question, int selectedOption)
        {
            return question.CorrectAnswer.OptionNumber == selectedOption;
        }

    }
}
