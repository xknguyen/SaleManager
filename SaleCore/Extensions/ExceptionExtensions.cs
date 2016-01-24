using System;
using System.Text;
using SaleCore.Utilities;

namespace SaleCore.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Thực hiện ghi log lại vào FileText
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logPath"></param>
        public static void Write(this Exception exception, string logPath)
        {
            try
            {
                var text = new StringBuilder("-----------------------------------------------------------\n\r");
                text.AppendLine("Time: " + DateTime.Now + "\n\r");
                if (exception.TargetSite.DeclaringType != null)
                    text.AppendLine("Action: " + exception.TargetSite.DeclaringType.FullName + "." + exception.TargetSite.Name + "\n\r");
                text.AppendLine("Message: " + exception.Message + "\n\r");
                text.AppendLine("-----------------------------------------------------------\n\r");
                WriteLog.Write(text.ToString(), logPath);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
