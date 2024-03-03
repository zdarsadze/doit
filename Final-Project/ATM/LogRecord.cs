using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class LogRecord
    {
        public string PrivateNumber { get; set; }
        public string Message { get; set; }
        public LogRecord() { }
        public LogRecord(string privateNumber, string message)
        {
            PrivateNumber = privateNumber;
            Message = message;
        }
    }
}
