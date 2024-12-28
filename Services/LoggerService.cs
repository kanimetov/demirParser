using System.Text.Json;

namespace DemirParser.Services
{
    public interface ILoggerService
    {
        void Log(Data data);
    }
    public class LoggerService : ILoggerService
    {
        private readonly string _logDirectory;

        public LoggerService()
        {   
            _logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Demir", "Logs");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void Log(Data data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                DateTime date = DateTime.Now;
                //date = date.AddDays(1);
                var logFileName = $"{data.Type}-{date:yyyy-MM-dd}.log";
                var logFilePath = Path.Combine(_logDirectory, logFileName);

                AppendJsonToFile(logFilePath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AppendJsonToFile(string filePath, string json)
        {
            try
            {
                int recordCount = 1;
                var tempFilePath = $"{filePath}.tmp";

                if (File.Exists(filePath))
                {
                    using var reader = new StreamReader(filePath);
                    var firstLine = reader.ReadLine();
                    if (int.TryParse(firstLine, out var count))
                    {
                        recordCount = count + 1;
                    }

                    using var writer = new StreamWriter(tempFilePath);
                    writer.WriteLine(recordCount);
                    while (!reader.EndOfStream)
                    {
                        writer.WriteLine(reader.ReadLine());
                    }

                    writer.WriteLine(json);

                }else {
                    using var writer = new StreamWriter(filePath);
                    writer.WriteLine(recordCount);
                    writer.WriteLine(json);
                    return;
                }

                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
