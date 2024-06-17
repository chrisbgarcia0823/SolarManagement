namespace SolarManagement.Helpers
{
    public class ErrorLogs
    {
        public static void CreateErrorLogFile(string errorMessage, string controllerName, string actionName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("START: " + DateTime.Now +
                               "\n" + "CONTROLLER: " + controllerName +
                               "\n" + "ACTION: " + actionName +
                               "\n" + errorMessage +
                               "\n" + "END" + "\n");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("START: " + DateTime.Now +
                        "\n" + "CONTROLLER: " + controllerName +
                        "\n" + "ACTION: " + actionName +
                        "\n" + errorMessage +
                        "\n" + "END" + "\n");
                }
            }
        }
    }
}
