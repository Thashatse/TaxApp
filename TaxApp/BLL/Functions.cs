using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Functions
    {
        IDBHandler handler = new DBHandler();
        public void logAnError(string Err)
        {
            /*
            * Logs Error Details in a text File
            */
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"" + AppDomain.CurrentDomain.BaseDirectory + "TaxAppErrorLog.txt", true))
                {
                    file.WriteLine();
                    file.WriteLine("TimeStamp: " + DateTime.Now);
                    file.WriteLine("Machine Name: " + Environment.MachineName);
                    file.WriteLine("OS Version: " + Environment.OSVersion);
                    file.WriteLine("Curent User: " + Environment.UserName);
                    file.WriteLine("User Domain: " + Environment.UserDomainName);
                    file.WriteLine("Curent Directory: " + Environment.CurrentDirectory);
                    file.WriteLine("Error: ");
                    file.WriteLine(Err);
                }
            }
            catch
            {

            }
        }

        public string generateNewInvoiceNum()
        {
            try
            {
                string invoiceNum = DateTime.Now.ToString("yyyyMMdd");

                int count = handler.getInvoiceTodaysCount();

                if (count == -1)
                {
                    logAnError("Error generating invoice number");
                    return null;
                }
                else if(count < 10)
                {
                    invoiceNum += "00" + count;
                }
                else if(count < 100)
                {
                    invoiceNum += "0" + count;
                }
                else
                {
                    invoiceNum += count;
                }

                return invoiceNum;
            }
            catch
            {
                logAnError("Error generating invoice number");
            }
            return null;
        }
    }
}
