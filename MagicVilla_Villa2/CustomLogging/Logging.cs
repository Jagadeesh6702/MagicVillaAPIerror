namespace MagicVilla_Villa2.CustomLogging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("ERROR - "+ message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
