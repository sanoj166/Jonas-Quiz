using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Jonas_Quiz.DataModels
{
    public class Question
    {
        public class Answer
        {
            public int OptionNumber { get; set; }
            public string? Option { get; set; }
        }

        public string? Statement { get; set; }
        public Answer CorrectAnswer { get; set; } = new Answer();
        public List<Answer> Options { get; set; } = new List<Answer>();

        public bool IsCorrectOption(string selectedOption)
        {
            return CorrectAnswer.OptionNumber.ToString() == selectedOption;
        }
    }
}





