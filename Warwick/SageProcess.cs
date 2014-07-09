using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warwick.SageServiceReference;

namespace Warwick
{
    public class SageProcess
    {
        private static SageService sage;
        private static Config config;
        private static EmailProcess emailProcess = new EmailProcess();

        public SageProcess()
        {
            config = new Config();
        }

        public void SageRecordKbank()
        {
           emailProcess.FetchUnseenMessages();
            Console.WriteLine("New Process");
            //Console.WriteLine(emailProcess._kbankProperty);
            //sage.Dispose();
            
        }

    }
}
