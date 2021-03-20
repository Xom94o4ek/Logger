using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
    public interface ILog
    {
        /// <summary>
        /// Критичная ошибка:приложение не может далее функционировать
        /// </summary>
        /// <param name="message">сообщение</param>
        void Fatal(string message);

        /// <summary>
        /// Критичная ошибка:приложение не может далее функционировать
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Fatal(string message, Exception e);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="message">сообщение</param>
        void Error(string message);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Error(string message, Exception e);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// Запись уникальных ошибок
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void ErrorUnique(string message, Exception e);

        /// <summary>
        /// Предупреждение: на работу приложения не влияет, 
        /// но может сообщать о потенциальных проблемах в расчете
        /// </summary>
        /// <param name="message">сообщение</param>
        void Warning(string message);

        /// <summary>
        /// Предупреждение: на работу приложения не влияет, 
        /// но может сообщать о потенциальных проблемах в расчете
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Warning(string message, Exception e);


        /// <summary>
        /// Пишет в лог уникальные в течении дня ошибки 
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <remarks>
        /// Если в течении дня поступают сообщения с одинаковым содержанием,
        ///  то в лог попадут только первые вхождения. 
        /// По прошествию дня уникальность возобновляется.
        /// </remarks>>
        void WarningUnique(string message);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        void Info(string message);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        ///  /// <param name="e">Exception</param>
        void Info(string message, Exception e);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="args">аргументы</param>
        void Info(string message, params object[] args);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message">сообщение</param>
        void Debug(string message);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Debug(string message, Exception e);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args">аргументы</param>
        void DebugFormat(string message, params object[] args);

        /// <summary>
        /// Запись системных логов информационного характера
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        void SystemInfo(string message, Dictionary<object, object> properties = null);
    }
    class Logger : ILog
    {
        public DateTime now;
        Dictionary<int, string> TodayErrors = new Dictionary<int, string>();
        int ErrorCounter = 0;
        Dictionary<int, string> TodayWarnings = new Dictionary<int, string>();
        int WarningCounter = 0;
        string FatalFile = "Fatal.txt";
        string ErrorFile = "Error.txt";
        string ErrorUniqFile = "UError.txt";
        string WarningFile = "Warning.txt";
        string WarningUniqFile = "UWarning.txt";
        string InfoFile = "Info.txt";
        string DebugFile = "Debug.txt";
        string SystemInfoFile = "SInfo.txt";
        public void Fatal(string message)
        {
            WriteToFile(FatalFile, TextFormating("FATAL", message));
        }
        public void Fatal(string message, Exception e)
        {
            WriteToFile(FatalFile, TextFormating("FATAL", message,e));
        }
        public void Error(string message)
        {
            ErrorCounter++;
            WriteToFile(ErrorFile, TextFormating("ERROR", message));
        }
        public void Error(string message, Exception e)
        {
            ErrorCounter++;
            TodayErrors.Add(ErrorCounter, $"{e.Message} - {e.StackTrace}");
            WriteToFile(ErrorFile, TextFormating("ERROR", message, e));
        }
        public void Error(Exception ex)
        {
            ErrorCounter++;
            TodayErrors.Add(ErrorCounter, $"{ex.Message} - {ex.StackTrace}");
            WriteToFile(ErrorFile, TextFormating("ERROR", ex));
        }
        public void ErrorUnique(string message, Exception e)
        {
            if(!TodayErrors.ContainsValue($"{e.Message} - {e.StackTrace}"))
            {
                //Error(message, e);
                //ErrorCounter++;
                //TodayErrors.Add(TodayErrors.Count, $"{e.Message} - {e.StackTrace}");
                WriteToFile(ErrorUniqFile, TextFormating("UERROR", message, e));
            }
            Error(message,e);
        }
        public void Warning(string message)
        {
            WarningCounter++;
            TodayWarnings.Add(WarningCounter, message);
            WriteToFile(WarningFile, TextFormating("WARNING", message));
        }
        public void Warning(string message, Exception e)
        {
            WarningCounter++;
            TodayWarnings.Add(WarningCounter, message);
            WriteToFile(WarningFile, TextFormating("WARNING", message, e));
        }
        public void WarningUnique(string message)
        {
            if (!TodayWarnings.ContainsValue(message))
            {
                //Error(message, e);
                //WarningCounter++;
                //TodayWarnings.Add(WarningCounter, message);
                WriteToFile(WarningUniqFile, TextFormating("UWARNING", message));
            }
            Warning(message);
        }
        public void Info(string message)
        {
            WriteToFile(InfoFile, TextFormating("INFO", message));
        }
        public void Info(string message, Exception e)
        {
            WriteToFile(InfoFile, TextFormating("INFO", message, e));
        }
        public void Info(string message, params object[] args)
        {
            WriteToFile(InfoFile, TextFormating("INFO", message, args));
        }
        public void Debug(string message)
        {
            WriteToFile(DebugFile, TextFormating("DEBUG", message));
        }
        public void Debug(string message, Exception e)
        {
            WriteToFile(DebugFile, TextFormating("DEBUG", message, e));
        }
        public void DebugFormat(string message, params object[] args)
        {
            WriteToFile(DebugFile, TextFormating("DEBUG", message, args));
        }
        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            if (properties != null)
            {
                WriteToFile(SystemInfoFile, TextFormating("SYSINFO", message, properties));
            }
            else { WriteToFile(SystemInfoFile, TextFormating("SYSINFO", message)); }
        }
        public void WriteToFile(string FileName, string message)
        {
            now = DateTime.Now;
            string path = @String.Format("Log/{0}", now.ToString("yyyy-MM-dd"));
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists) { dirInfo.Create(); TodayErrors.Clear(); ErrorCounter = 0; TodayWarnings.Clear(); WarningCounter = 0; }
            File.AppendAllText(@String.Format("{0}/{1}", path, FileName), $"{now.ToString("dd.MM.yyyy")} {now.ToString("HH:mm:ss")} {message}\n");
        }
        public string TextFormating(string nametype, string message)
        {
            return $"({nametype}): {message}";
        }
        public string TextFormating(string nametype, string message, Exception e)
        {
            string ExcInfo = $"\n\t\t\t\tИсключение: {e.Message}\n\t\t\t\tМетод: {e.TargetSite}\n\t\t\t\tТрассировка стека: {e.StackTrace}";
            return $"({nametype}): {message}{ExcInfo}";
        }
        public string TextFormating(string nametype, Exception e)
        {
            string ExcInfo = $"Исключение: {e.Message}\n\t\t\t\tМетод: {e.TargetSite}\n\t\t\t\tТрассировка стека: {e.StackTrace}";
            return $"({nametype}): {ExcInfo}";
        }
        public string TextFormating(string nametype, string message, params object[] args)
        {
            string AllArgs = "";
            for (int i = 0; i < args.Length; i++)
            {
                AllArgs += "\n\t\t\t\t" + args[i];
            }
            return $"({nametype}): {message}{AllArgs}";
        }
        public string TextFormating(string nametype, string message, Dictionary<object, object> properties)
        {
            string AllArgs = "";
            foreach (KeyValuePair<object, object> keyValue in properties)
            {
                AllArgs += $"\n\t\t\t\t{keyValue.Key} - {keyValue.Value}";
            }
            return $"({nametype}): {message}{AllArgs}";
        }
    }
}
