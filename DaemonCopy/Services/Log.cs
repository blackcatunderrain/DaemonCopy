namespace DaemonCopy.Services
{
    public class Log
    {

        private readonly string _name;

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
            using var writer = new StreamWriter("error.log", true);
            var errorMessage = DateTime.Now.ToString() + "\t" + error.Message;
            writer.WriteLine(errorMessage);
        }

        public void Message(string text)
        {
            using var writer = new StreamWriter("error.log", true);
            writer.WriteLine(text);
        }
    }
}
