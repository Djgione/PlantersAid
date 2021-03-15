using PlantersAid.ServiceLayer.Enums;
using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public class FlatFileLogger : ILogger
    {
        private readonly String _logFolderLocation;
        private readonly DateTime _current;

        public FlatFileLogger()
        {
            _logFolderLocation = Environment.GetEnvironmentVariable("plantersAidLogFiles");
            _current = DateTime.UtcNow;
        }

        public async Task Log(string str)
        {
            using(var filestream = new FileStream(_logFolderLocation + GetFileName(), FileMode.Append, FileAccess.Write))
            {
                StreamWriter write = new StreamWriter(filestream);
                await write.WriteLineAsync(CreateTimestamp() + LogLevel.Info + " | " + str);
                write.Close();
            }

        }

        public async Task Log(string str, LogLevel level)
        {
            using (var filestream = new FileStream(_logFolderLocation + GetFileName(), FileMode.Append, FileAccess.Write))
            {
                StreamWriter write = new StreamWriter(filestream);
                await write.WriteLineAsync(CreateTimestamp() + level + " | " + str);
                write.Close();
            }
        }

        public async Task Log(Exception ex)
        {
            using (var filestream = new FileStream(_logFolderLocation + GetFileName(), FileMode.Append, FileAccess.Write))
            {
                StreamWriter write = new StreamWriter(filestream);
                await write.WriteLineAsync(CreateTimestamp() + LogLevel.Error + " | " + ex.ToString());
                write.Close();
            }
        }

        private String CreateTimestamp()
        {
            return  "[" + _current.Hour + ":" + _current.Minute + ":" + _current.Second + ":" + _current.Millisecond + "] | ";
        }

        private String GetFileName()
        {
            return _current.Year + "-" + _current.Month + "-" + _current.Day + ".txt";
        }
    }
}
