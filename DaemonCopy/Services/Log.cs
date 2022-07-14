using System;
using System.IO;

namespace DaemonCopy.Services
{
    class Log
    {

        string _name;

        public string Name
        {
            get { return _name; }
        }

        public Log()
        {
            _name = "noname";
        }

        public Log(string name)
        {
            _name = name;
        }

        public void Fatal(Exception error)
        {
            var writer = new StreamWriter("error.log", true);
            var errorMessage = DateTime.Now.ToString() + "\t" + error.Message;
            writer.WriteLine(errorMessage);
            writer.Close();
        }

        public void Message(string text)
        {
            var writer = new StreamWriter("error.log", true);
            writer.WriteLine(text);
            writer.Close();
        }
    }
}