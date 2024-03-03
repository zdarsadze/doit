using System;
using static System.Formats.Asn1.AsnWriter;

namespace NumberGuess
{
    internal class Program
    {
        class Result : IComparable<Result>
        {
            public String name { get; set; }
            public int score { get; set; }
            public Result(String name, int score) { this.name = name; this.score = score; }
            public int CompareTo(Result other)
            {
                return other.score.CompareTo(this.score);
            }
        }

        static List<Result> results = new List<Result>();

        static void Main(string[] args)
        {
            bool exit = false;
            string name;
            string command;
            int score;
            int difficulty;
            int numberToGuess;
            int input;
            int numberOfGuesses = 10;
            Random random = new Random();
            string fileName = "../../../results.csv";
            loadFromFile(fileName, results);

            Console.Write("Enter name: ");
            name = Console.ReadLine();
            Console.WriteLine("\nWellcome to this GuessNumber game " + name + " :)\n");
            
            while (!exit)
            {
                numberOfGuesses = 10;
                Console.WriteLine("Press 1 - easy, 2 - medium, 3 - hard");
                Console.WriteLine("Press m - my result, t - top10, q - quite\n");
                command = Console.ReadLine().ToLower();
                switch (command)
                {
                    case "1":
                        difficulty = 1;
                        score = 10 * difficulty;
                        numberToGuess = random.Next(1, 16);
                        Console.WriteLine("Guess number between 1..15 in 10 tries\n");
                        break;
                    case "2":
                        difficulty = 2;
                        score = 10 * difficulty;
                        numberToGuess = random.Next(1, 26);
                        Console.WriteLine("Guess number between 1..25 in 10 tries\n");
                        break;
                    case "3":
                        difficulty = 3;
                        score = 10 * difficulty;
                        numberToGuess = random.Next(1, 51);
                        Console.WriteLine("Guess number between 1..50 in 10 tries\n");
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
                        Console.WriteLine("Wrong input");
                        continue;
                }

                while (numberOfGuesses > 0)
                {
                    while (true)
                    {
                        try
                        {
                            input = int.Parse(Console.ReadLine());
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Input number");
                        }
                    }
                    numberOfGuesses--;
                    if (input == numberToGuess)
                    {
                        Console.WriteLine("Won. You got score " + score + "\n");
                        break;
                    } else
                    {                       
                        score -= difficulty;
                        Console.WriteLine("Wrong. You have " + numberOfGuesses + " tries left");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("Gane over. Number to guess was: " + numberToGuess + "\n");
                        } else
                        {
                            if (input > numberToGuess)
                            {
                                Console.WriteLine("Entered number is greater then number to guess\n");
                            }
                            else
                            {
                                Console.WriteLine("Entered number is lower then number to guess\n");
                            }
                        }                                               
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

        static void loadFromFile(string fileName, List<Result> results)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    Result result = new Result(row[0], int.Parse(row[1]));
                    results.Add(result);
                }
            }
        }

        static void saveToFile(string fileName, List<Result> results)
        {

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < results.Count; i++)
                {
                    Result result = results[i];
                    writer.WriteLine(result.name + "," + result.score);
                }
            }
            Console.WriteLine("Results saved");
        }
    }
}
