using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Pop3;

namespace Warwick
{
    class Config
    {
        public string emailHostName { get; set; }
        public string emailUsername { get; set; }
        public string emailPassword { get; set; }
        public int emailPort { get; set; }
        public bool emailUseSsl { get; set; }
        public AuthenticationMethod emailAuthen { get; set; }
        public List<string> emailList = new List<string>();
        public string emailFolderPath { get; set; }
        public string extractZipPath { get; set; }
        public string sageUser { get; set; }
        public string sagePassword { get; set; }

        public string kbankAccount { get; set; }

        public Config()
        {
            sageUser = "admin";
            sagePassword = "";

            kbankAccount = "026-1-10930-1";

            emailHostName = "mail.trueclarityintl.com";
            emailUsername = "test@trueclarityintl.com";
            emailPassword = "1234";
            emailPort = 110;
            emailUseSsl = false;
            emailAuthen = AuthenticationMethod.UsernameAndPassword;
            emailFolderPath = "\\testreadmail\\";
            //add email list for check payment method
            //emailList.Add("arraieot@gmail.com");
            //emailList.Add("arraieot@hotmail.com");
            emailList.Add("kraisri.c@trueclarityintl.com");
            extractZipPath = "\\extractexcel\\";
            //emailList[0] = "arraieot@gmail.com";
            //emailList[0] = "arraieot@gmail.com";
        }
    }
}
