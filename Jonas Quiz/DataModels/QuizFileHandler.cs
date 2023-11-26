using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jonas_Quiz.DataModels
{
    public static class QuizFileHandler
    {
        private static readonly string AppDataFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MyQuiz"
        );

        static QuizFileHandler()
        {
            Directory.CreateDirectory(AppDataFolderPath);
        }


        public static async Task SaveQuizAsync(string fileName, Quiz quiz)
        {
            string filePath = Path.Combine(AppDataFolderPath, fileName);
            string json = JsonSerializer.Serialize(quiz);

            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task<Quiz> ReadQuizAsync(string fileName)
        {
            string filePath = Path.Combine(AppDataFolderPath, fileName);

            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<Quiz>(json);
            }

            return null;
        }
    }
}
