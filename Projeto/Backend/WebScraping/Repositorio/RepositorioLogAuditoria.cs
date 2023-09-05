
namespace WebScraping.Repositorio
{
    public class RepositorioLogAuditoria
    {
        public void Log(string message)
        {
            string logFilePath = "error_log.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();   
            }
        }
    }
}
