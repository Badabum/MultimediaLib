using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    public class Logger
    {
        public static void LogException(Exception ex)
        {
            var sw = System.IO.File.AppendText("errorlog.txt");
            try
            {
                sw.WriteLine("{0} - {1}", DateTime.Now, ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
