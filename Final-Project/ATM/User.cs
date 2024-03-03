using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class User
    {
        public int Id { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string PrivateNumber { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
        private static readonly Random random = new Random();
        public User() { }
        public User(int id, string firsName, string lastName, string privateNumber)
        {
            Id = id;
            FirsName = firsName;
            LastName = lastName;
            PrivateNumber = checkPrivateNumber(privateNumber);
            Password = generatePassword();
            Balance = 0;
        }
        private string generatePassword()
        {
            return "" + random.Next(0, 10) + random.Next(0, 10) + random.Next(0, 10) + random.Next(0, 10);
        }
        private string checkPrivateNumber(string number)
        {
            if (number.Length != 11)
            {
                throw new ArgumentException("PrivateNumber must be 11 in length");
            }
            for (int i = 0; i < number.Length; i++)
            {
                if (!char.IsDigit(number[i]))
                {
                    throw new ArgumentException("PrivateNumber must contain only digits");
                }
            }
            return number;
        }
    }
}
