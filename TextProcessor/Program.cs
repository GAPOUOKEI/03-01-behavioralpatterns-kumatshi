using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextFileProcessor
{
    /*
* ФИО студента: Тазеев Марс Эдуардович
* номер варианта: 4 
* условие задачи (скопировать из листа задания): Создайте приложение для хранения контактов, 
* которое позволяет добавлять, редактировать и удалять контакты с помощью команд, а также поддерживает отмену этих действий.
*/
    class Program
    {
        static void Main(string[] args)
        {
            
            string csText = "// Это комментарий\nusing    System;\nclass Test\r\n{\r\n    // Другой комментарий\n}";
            string pyText = "# Комментарий тут\ndef hello():\n    print('Привет')\r\n# Конец";
            string plainText = "Привет   Мир\r\nЭто\r\nтест";

            
            TextProcessor csProcessor = new CSharpProcessor();
            TextProcessor pyProcessor = new PythonProcessor();
            TextProcessor txtProcessor = new PlainTextProcessor();

            
            Console.WriteLine("=== Обработка файла C# ===");
            Console.WriteLine("Исходный текст:\n" + csText + "\n");
            string csResult = csProcessor.ProcessText(csText);
            Console.WriteLine("Результат:\n" + csResult + "\n");

            Console.WriteLine("=== Обработка файла Python ===");
            Console.WriteLine("Исходный текст:\n" + pyText + "\n");
            string pyResult = pyProcessor.ProcessText(pyText);
            Console.WriteLine("Результат:\n" + pyResult + "\n");

            Console.WriteLine("=== Обработка простого текста ===");
            Console.WriteLine("Исходный текст:\n" + plainText + "\n");
            string txtResult = txtProcessor.ProcessText(plainText);
            Console.WriteLine("Результат:\n" + txtResult + "\n");
        }
    }

    abstract class TextProcessor
    {
        
        public string ProcessText(string text)
        {
            Console.WriteLine("Начало обработки текста...");

            text = RemoveComments(text);
            Console.WriteLine("Комментарии удалены");

            text = NormalizeLineBreaks(text);
            Console.WriteLine("Переносы строк нормализованы");

            text = FinalProcessing(text);
            Console.WriteLine("Завершающая обработка выполнена");

            return text;
        }

        
        protected abstract string RemoveComments(string text);
        protected abstract string NormalizeLineBreaks(string text);

        
        protected virtual string FinalProcessing(string text)
        {
            return text.Trim();
        }
    }

   
    class CSharpProcessor : TextProcessor
    {
        protected override string RemoveComments(string text)
        {
           
            var lines = text.Split('\n');
            var result = lines.Select(line =>
            {
                int commentIndex = line.IndexOf("//");
                if (commentIndex >= 0)
                    return line.Substring(0, commentIndex);
                return line;
            });
            return string.Join("\n", result);
        }

        protected override string NormalizeLineBreaks(string text)
        {
            
            return text.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        protected override string FinalProcessing(string text)
        {
            
            return Regex.Replace(text, @"\s+", " ").Trim();
        }
    }

    
    class PythonProcessor : TextProcessor
    {
        protected override string RemoveComments(string text)
        {
            
            var lines = text.Split('\n');
            var result = lines.Select(line =>
            {
                int commentIndex = line.IndexOf("#");
                if (commentIndex >= 0)
                    return line.Substring(0, commentIndex);
                return line;
            });
            return string.Join("\n", result);
        }

        protected override string NormalizeLineBreaks(string text)
        {
            
            return text.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }

    
    class PlainTextProcessor : TextProcessor
    {
        protected override string RemoveComments(string text)
        {
            
            return text;
        }

        protected override string NormalizeLineBreaks(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        protected override string FinalProcessing(string text)
        {
            
            return text.ToLower().Trim();
        }
    }


}