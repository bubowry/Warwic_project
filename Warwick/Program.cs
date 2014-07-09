using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Warwick
{

    class Program
    {
        private static System.Timers.Timer aTimer;

        static void Main(string[] args)
        {
            //EmailCollection emailCollection1 = new EmailCollection("111");
            //EmailCollection emailCollection2 = new EmailCollection("222");
            //_emailcollection.Add(emailCollection1);
            //_emailcollection.Add(emailCollection2);

            ////foreach (EmailCollection item in _emailcollection)
            ////{
            ////    Console.WriteLine(item.EmailUid);
            ////    //print out the name of each person found in the collection
            ////}

            //bool containsItem = _emailcollection.Any(item => item.EmailUid == "222");
            //Console.WriteLine(containsItem);

            ////EmailSeenCollection emailseen1 = new EmailSeenCollection("123");
            ////EmailSeenCollection emailseen2 = new EmailSeenCollection("321");
            ////_emailseen.Add(emailseen1);
            ////_emailseen.Add(emailseen2);

            ////foreach (EmailSeenCollection item in _emailseen)
            ////{
            ////    Console.WriteLine(item.EmailUid);
            ////    //print out the name of each person found in the collection
            ////}

            //EmailProcess emailProcess = new EmailProcess();
            ////emailProcess.Read_Emails();
            ////Console.ReadKey(true);
            ////emailProcess.Read_Emails();
            ////Console.ReadKey(true);

            //emailProcess.FetchUnseenMessages();
            //Console.ReadKey(true);
            //emailProcess.FetchUnseenMessages();
            //Console.ReadKey(true);
            //emailProcess.FetchUnseenMessages();
            //Console.ReadKey(true);

            //config = new Config();
            
            //SageService sage = new SageService(config.sageUser, config.sagePassword);
            //emailProcess.FetchUnseenMessages();
            //foreach (KbankProperty item in emailProcess._kbankProperty)
            //{

            //    //Generate orde_coutomerderid

            //    //+1 customid

            //    //Bank tranfer -> Kbank Deposit
            //    Console.Write("\r\n");
            //    Console.Write(item.Tx + "\t");
            //    Console.Write(item.Ref1 + "\t");
            //    Console.Write(item.Ref2 + "\t");
            //    Console.Write(item.Payer + "\t");
            //    Console.Write(item.Amount + "\t");
            //    Console.Write(item.Time + "\t");
            //    Console.Write(item.TellerId + "\t");
            //    Console.Write(item.Branch + "\t");
            //}
            ////Console.WriteLine(emailProcess._kbankProperty);
            //sage.Dispose();
            killExcelProcess();
            // Create a timer with a ten second interval.
            aTimer = new System.Timers.Timer(10000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(updateQuery);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 20000;
            aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program.");
            Console.ReadLine();


        }

        private static void updateQuery(object source, ElapsedEventArgs e)
        {
            SageProcess sageProcess = new SageProcess();
            sageProcess.SageRecordKbank();
        }

        private static void killExcelProcess()
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }
    }
}
