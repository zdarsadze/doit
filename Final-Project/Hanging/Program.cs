using System.Xml.Serialization;
using System;

namespace Hanging
{
    public class Program
    {
        [Serializable]
        public class Result : IComparable<Result>
        {
            public String name { get; set; }
            public int score { get; set; }
            public Result(String name, int score) { this.name = name; this.score = score; }
            public Result() {}
            public int CompareTo(Result other)
            {
                return other.score.CompareTo(this.score);
            }
        }

        static List<Result> results = new List<Result>();

        static void Main(string[] args)
        {
            List<string> words = new List<string>
            {
            "apple", "banana", "orange", "grape", "kiwi",
            "strawberry", "pineapple", "blueberry", "peach", "watermelon"
            };
            string wordToGuess;
            char[] wordArr;
            string guessedWord;
            char[] guessedWordArr;
            bool won;
            bool guessOneChar;
            int guessLeft;
            char input;
            bool exit = false;
            string name;
            string command;
            int score;

            Random random = new Random();
            string fileName = "../../../results.xml";
            results = loadFromFile(fileName, results);

            Console.Write("Enter name: ");
            name = Console.ReadLine();
            Console.WriteLine("\nWellcome to this Hanging game " + name + " :)\n");

            while (!exit)
            {
                Console.WriteLine("Press s - start, m - my result, t - top10, q - quite\n");
                command = Console.ReadLine().ToLower();
                switch (command)
                {
                    case "s":
                        int randomIndex = random.Next(0, words.Count);
                        wordToGuess = words[randomIndex];
                        won = false;
                        guessOneChar = false;
                        guessLeft = 6;
                        score = 0;
                        wordArr = wordToGuess.ToCharArray();
                        guessedWord = new string('.', wordToGuess.Length); ;
                        guessedWordArr = guessedWord.ToCharArray();
                        break;
                    case "m":
                        printMyResult(results, name);
                        continue;
                    case "t":
                        printTop10(results);
                        continue;
                    case "q":
                        saveToFile(fileName, results);
                        exit = true;
                        Console.WriteLine("\nBye, bye");
                        continue;
                    default:
                        Console.WriteLine("Wrong input\n");
                        continue;
                }

                while (guessLeft > 0)
                {
                    Console.WriteLine("Guess word: " + guessedWord + "\n");
                    

                    while (true)
                    {
                        Console.Write(7 - guessLeft + ". input char: ");
                        try
                        {
                            input = char.Parse(Console.ReadLine());
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Wrong input\n");
                        }
                    }
                    
                    for (int i = 0; i < wordArr.Length; i++)
                    {
                        if (wordArr[i] == input && guessedWordArr[i] == '.')
                        {
                            score++;
                            guessedWordArr[i] = input;
                            guessOneChar = true;
                        }
                    }
                    guessedWord = new String(guessedWordArr);
                    if (!guessedWord.Contains("."))
                    {
                        won = true;
                        score += 10;
                    }
                    guessLeft--;
                    if (won)
                    {
                        break;
                    }
                }

                Console.WriteLine("Guess word: " + guessedWord + "\n");
                if (!guessOneChar)
                {
                    Console.WriteLine("You lost. Gove over. Your score is: " + score + ". Word is: " + wordToGuess + "\n");
                }
                else
                {
                    if (!won)
                    {
                        Console.Write("Guess whole word: ");
                        string s = Console.ReadLine();
                        if (s == wordToGuess)
                        {
                            score += 10;
                            Console.WriteLine("You won. Your score is: " + score + "\n");
                        } else
                        {
                            Console.WriteLine("You lost. Gove over. Your score is: " + score + ". Word is: " + wordToGuess + "\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You won. Your score is: " + score + "\n");
                    }
                }

                Result result = new Result(name, score);
                addResult(results, result);
            }
        }

        static void addResult(List<Result> resultList, Result result)
        {
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].name == result.name)
                {
                    if (resultList[i].score < result.score)
                    {
                        resultList[i] = result;
                    }
                    return;
                }
            }
            resultList.Add(result);
        }

        static void printMyResult(List<Result> resultList, String name)
        {
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].name == name)
                {
                    Console.WriteLine(name + " your score is: " + resultList[i].score + "\n");
                    break;
                }
            }
        }

        static void printTop10(List<Result> results)
        {
            results.Sort();
            int n = Math.Min(results.Count, 10);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i + 1}: {results[i].name} - {results[i].score}");
            }
            Console.WriteLine();
        }

        static List<Result> loadFromFile(string fileName, List<Result> results)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Result>));
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                results = (List<Result>)serializer.Deserialize(fileStream);
            }
            return results;
        }

        static void saveToFile(string fileName, List<Result> results)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Result>));
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, results);
            }
            Console.WriteLine("Results saved");
        }

















        
    }
}
