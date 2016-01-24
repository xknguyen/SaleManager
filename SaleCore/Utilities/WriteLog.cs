using System;
using System.IO;

namespace SaleCore.Utilities
{
    public class WriteLog
    {
        public static void Write(string text, string logPath)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                var filename = DateTime.Now.ToString("dd-MM-yyyy") + ".log";
                
                var directory = logPath + "\\" + DateTime.Now.ToString("dd-MM-yyyy") + "\\";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                fs = new FileStream(directory + "\\" + filename, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(text);
                sw.Flush();
            }
            catch
            {
                // Nothing
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
    }
}
