using System;
using System.IO;
using System.Text;

namespace Utilities
{
    public class CreateLogFiles
    {
        private readonly string _sLogFormat;
        private readonly string _sErrorTime;

        public CreateLogFiles()
        {
            _sLogFormat = Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ==> ";
            _sErrorTime = DateTime.Now.ToString("yyyMMdd");
        }
        public void ErrorLog(string path, string fileName, string sErrMsg)
        {
            if (!path.EndsWith("\\")) path += "\\";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += string.Format("{0}_{1}.txt", fileName, _sErrorTime);
                using (var sw = File.AppendText(path))
                {
                    sw.WriteLine(_sLogFormat + sErrMsg);
                }
            }
            catch { }
        }

        public static void WriteLog(string path, string fileName, string sErrMsg)
        {
            var sLogFormat = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ==> ";
            sErrMsg = sLogFormat + sErrMsg + Environment.NewLine;

            if (!path.EndsWith("\\")) path += "\\";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += string.Format("{0}_{1}.txt", fileName, DateTime.Now.ToString("yyyyMMdd"));
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(sErrMsg);
                }
            }
            catch { }
        }
        public void WriteFile(string path,string fileName, string content)
        {
            if(!path.EndsWith("\\")) path += "\\";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writer.Write(content);
                    }
                }
            }
            catch (Exception ex){}
        }
    }
}
