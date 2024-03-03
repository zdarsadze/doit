using System.IO.Enumeration;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace ATM
{
    public class Program
    {
        private static readonly string UsersFileName = "../../../users.json";
        private static List<User> users = new List<User>();
        private static readonly string LogsFileName = "../../../logs.json";
        private static List<LogRecord> logs = new List<LogRecord>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            users = LoadUsersFromFile(UsersFileName);
            logs = LoadLogsFromFile(LogsFileName);
            Console.WriteLine("Welcome to ATM. Please choose operation:\n");
            Boolean exit = false;
            Boolean logout = false;
            String command;
            String FirsName;
            String LastName;
            String PrivateNumber;
            String Password;
            User user = new User();
            while (!exit)
            {
                PrintOutMenu();
                switch (command = Console.ReadLine())
                {
                    case "1":
                        Console.Write("Private Number: ");
                        PrivateNumber = Console.ReadLine();
                        Console.Write("Password: ");
                        Password = Console.ReadLine();
                        user = LoginUser(users, PrivateNumber, Password);
                        if (user != null)
                        {
                            Console.WriteLine("\nLogin successful\n");
                            break;
                        } else
                        {
                            Console.WriteLine("\nLogin failed\n");
                            continue;
                        }
                    case "2":
                        Console.Write("FirsName: ");
                        FirsName = Console.ReadLine();
                        Console.Write("LastName: ");
                        LastName = Console.ReadLine();
                        Console.Write("PrivateNumber: ");
                        PrivateNumber = Console.ReadLine();
                        try
                        {
                            RegisterUser(users, FirsName, LastName, PrivateNumber);
                            Console.WriteLine("\nUser registered successfuly\n");
                            SaveUsersToFile(users, UsersFileName);
                        } catch (ArgumentException ex)
                        {
                            Console.WriteLine("\nCan not register user. Reason: " + ex.Message + "\n");
                        }
                        continue;
                    case "0":
                        exit = true;
                        SaveUsersToFile(users, UsersFileName);
                        SaveLogsToFile(logs, LogsFileName);
                        continue;
                    default:
                        Console.WriteLine("Wrong input\n");
                        continue;
                }

                while (!logout)
                {
                    PrintInMenu();
                    switch (command = Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine(user.FirsName + " " + user.LastName + " your balance is: " + user.Balance + "\n");
                            LogRecord record = new LogRecord(user.PrivateNumber,
                                "მომხმარებელმა სახელად " + user.FirsName + " " + user.LastName + " - შეამოწმა ბალანსი : " + DateTime.Now.ToString("dd.MM.yyyy") + " - ში.");
                            logs.Add(record);
                            SaveLogsToFile(logs, LogsFileName);
                            break;
                        case "2":
                            Deposit(user);
                            break;
                        case "3":
                            Withraw(user);
                            break;
                        case "4":
                            PrintLog(logs, user.PrivateNumber);
                            break;
                        case "0":
                            Console.WriteLine(user.FirsName + " " + user.LastName + " loged out successfuly\n");
                            logout = true;
                            user = new User();
                            continue;
                        default:
                            Console.WriteLine("Wrong input\n");
                            break;
                    }
                }
            }
        }

        static List<User> LoadUsersFromFile(string FileName)
        {
            string json = File.ReadAllText(FileName);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }

        static List<LogRecord> LoadLogsFromFile(string FileName)
        {
            string json = File.ReadAllText(FileName);
            List<LogRecord> logs = JsonConvert.DeserializeObject<List<LogRecord>>(json);
            return logs;
        }

        static void SaveUsersToFile(List<User> users, string FileName)
        {
            string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        static void SaveLogsToFile(List<LogRecord> logs, string FileName)
        {
            string json = JsonConvert.SerializeObject(logs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        static void RegisterUser(List<User> users, string firsName, string lastName, string privateNumber)
        {
            int id = users.Count + 1;
            foreach (User user in users)
            {
                if (user.PrivateNumber == privateNumber)
                {
                    throw new ArgumentException("User with privateNumber " + privateNumber + " already exists");
                }
                if (firsName == "" || lastName == "")
                {
                    throw new ArgumentException("User FirstName or LastName is empty");
                }
            }
            User newUser = new User(id, firsName, lastName, privateNumber);
            users.Add(newUser);
        }

        static User LoginUser(List<User> users, string privateNumber, string password)
        {
            foreach (User user in users)
            {
                if (user.PrivateNumber == privateNumber)
                {
                    if (user.Password == password)
                    {
                        return user;
                    } else
                    { 
                        return null; 
                    }
                }
            }
            return null;
        }

        static void Deposit(User user)
        {
            Console.Write("Deposit amount: ");
            try
            {
                int amount = int.Parse(Console.ReadLine());
                if (amount <= 0) 
                {
                    Console.WriteLine("\nDeposit amount must be positive number\n");
                } else
                {
                    user.Balance += amount;
                    Console.WriteLine("\nDeposit successful. New balance is: " + user.Balance + "\n");
                    SaveUsersToFile(users, UsersFileName);

                    LogRecord record = new LogRecord(user.PrivateNumber,
                                "მომხმარებელმა სახელად " + user.FirsName + " " + user.LastName + " - შეავსო ბალანსი " + 
                                amount + " ლარით : " + DateTime.Now.ToString("dd.MM.yyyy") + " - ში. მისი მოქმედი ბალანსი შეადგენს " + 
                                user.Balance + " ლარს.");
                    logs.Add(record);
                    SaveLogsToFile(logs, LogsFileName);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("\nDeposit amount must be number\n");
            }
        }

        static void Withraw(User user)
        {
            Console.Write("Withraw amount: ");
            try
            {
                int amount = int.Parse(Console.ReadLine());
                if (amount <= 0)
                {
                    Console.WriteLine("\nWithraw amount must be positive number\n");
                }
                else
                {
                    if (amount > user.Balance)
                    {
                        Console.WriteLine("\nCan not withraw. Insufficient funds\n");
                    }
                    else
                    {
                        user.Balance -= amount;
                        Console.WriteLine("\nWithraw successful. New balance is: " + user.Balance + "\n");
                        SaveUsersToFile(users, UsersFileName);

                        LogRecord record = new LogRecord(user.PrivateNumber,
                                "მომხმარებელმა სახელად " + user.FirsName + " " + user.LastName + " - გაანაღდა " +
                                amount + " ლარი : " + DateTime.Now.ToString("dd.MM.yyyy") + " - ში. მისი მოქმედი ბალანსი შეადგენს " +
                                user.Balance + " ლარს.");
                        logs.Add(record);
                        SaveLogsToFile(logs, LogsFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nDeposit amount must be number\n");
            }
        }

        static void PrintLog(List<LogRecord> logs, string PrivateNumber)
        {
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].PrivateNumber == PrivateNumber)
                {
                    Console.WriteLine(logs[i].Message);
                }
            }
            Console.WriteLine();
        }

        static void PrintOutMenu()
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register");
            Console.WriteLine("0 - Exit\n");
        }

        static void PrintInMenu()
        {
            Console.WriteLine("1 - Balance");
            Console.WriteLine("2 - Deposit");
            Console.WriteLine("3 - Withraw");
            Console.WriteLine("4 - History");
            Console.WriteLine("0 - Logout\n");
        }
    }
}
