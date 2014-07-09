using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Mime;
using OpenPop.Pop3;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Runtime.InteropServices;       //microsoft Excel 

namespace Warwick
{
    public class EmailProcess
    {
        private static SageService sage;
        private Config config;
        private static ObjectCollection<EmailCollection> _emailcollection = new ObjectCollection<EmailCollection>();
        //private static ObjectCollection<EmailPathCollect> _emailPathCollect = new ObjectCollection<EmailPathCollect>();
        public ObjectCollection<KbankProperty> _kbankProperty = new ObjectCollection<KbankProperty>();
        private string[] excelExt = { ".xls", ".xlsx" };


        public EmailProcess()
        {
            config = new Config();
        }

        public List<Email> FetchUnseenMessages()
        {
            
            // The client disconnects from the server when being disposed
            List<Email> emails = new List<Email>();
            //EmailPathClear();

            using (Pop3Client pop3Client = new Pop3Client())
            {
                pop3Client.Connect(config.emailHostName, config.emailPort, config.emailUseSsl);
                pop3Client.Authenticate(config.emailUsername, config.emailPassword, config.emailAuthen);
                
                // Fetch all the current uids seen
                List<string> uids = pop3Client.GetMessageUids();

                for (int i = 0; i < uids.Count; i++)
                {
                    string currentUidOnServer = uids[i];

                    if (!EmailUidExist(currentUidOnServer))
                    {
                        Message unseenMessage = pop3Client.GetMessage(i + 1);
                        if (EmailListCheck(unseenMessage.Headers.From.Address))
                        {
                            Email email = new Email()
                            {
                                MessageNumber = i,
                                Subject = unseenMessage.Headers.Subject,
                                DateSent = unseenMessage.Headers.DateSent,
                                From = unseenMessage.Headers.From.Address,
                            };

                            MessagePart body = unseenMessage.FindFirstHtmlVersion();

                            if (body != null)
                            {
                                email.Body = body.GetBodyAsText();
                            }
                            else
                            {
                                body = unseenMessage.FindFirstPlainTextVersion();
                                if (body != null)
                                {
                                    email.Body = body.GetBodyAsText();
                                }
                            }
                            List<MessagePart> attachments = unseenMessage.FindAllAttachments();

                            foreach (MessagePart attachment in attachments)
                            {
                                email.Attachments.Add(new Attachment
                                {
                                    FileName = attachment.FileName,
                                    ContentType = attachment.ContentType.MediaType,
                                    Content = attachment.Body
                                });
                                //EmailPathCollection(attachment.FileName);
                            }
                            emails.Add(email);
                            Download(email);

                            //pop3Client.DeleteMessage(i + 1);
                            
                        }
                        EmailUidCollection(currentUidOnServer);
                    }
                }

                //getExcelFile();
                //KbankDataProcess();

            }
            return emails;
        }

        private void EmailUidCollection(string uid)
        {
            EmailCollection emailCollection = new EmailCollection(uid);
            _emailcollection.Add(emailCollection);
        }

        //private void EmailPathCollection(string path)
        //{
        //    EmailPathCollect emailpathcollect = new EmailPathCollect(path);
        //    _emailPathCollect.Add(emailpathcollect);
        //}

        //private void EmailPathClear()
        //{
        //    _emailPathCollect.Clear();
        //}


        private bool EmailUidExist(string uid)
        {
            return _emailcollection.Any(item => item.EmailUid == uid);
        }

        private bool EmailListCheck(string email)
        {
            return config.emailList.Exists(element => element == email);
        }

        private void KbankPropertyCollection(string tx
            , string ref1
            , string ref2
            //, string ref3
            //, string payer
            //, string chequeBankCode
            , string amount
            , string time
            //, string tellerId
            //, string branch
            , DateTime dateKbank
            )
        {
            KbankProperty kbankProperty = new KbankProperty(
                tx
                , ref1
                , ref2
                //, ref3
                //, payer
                //, chequeBankCode
                , amount
                , time
                //, tellerId
                //, branch
                , dateKbank
                );
            _kbankProperty.Add(kbankProperty);
        }

        public void KbankPropertyClear()
        {
            _kbankProperty.Clear();
        }

        private void Download(Email email)
        {
            List<Attachment> attachments = email.Attachments;
            foreach (Attachment attachment in attachments)
            {
                if (getFileExtensionExcel(attachment.FileName))
                {
                    FileStream Stream = new FileStream(config.emailFolderPath + attachment.FileName, FileMode.Create);
                    BinaryWriter BinaryStream = new BinaryWriter(Stream);
                    BinaryStream.Write(attachment.Content);
                    BinaryStream.Close();

                    //get Detail from excel file
                    //getExcelFile(attachment.FileName);
                    KbankPropertyClear();
                    getExcelFileKBank(attachment.FileName);


                }
            }
        }

        private bool getFileExtensionExcel(string fileName)
        {
            string ext = fileName.Substring(fileName.LastIndexOf('.')).ToString();

            return excelExt.Contains(ext);
        }

        public void getExcelFile(string fileName)
        {
                Excel.Application xlApp = new Excel.Application();
                //string filePath = String.Format("{0}{1}", config.emailFolderPath,  item.EmailPath);
                string filePath = String.Format("{0}{1}", config.emailFolderPath, fileName);
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@filePath);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 1; i <= rowCount; i++)
                {
                    if (i == 3)
                    {

                    }
                    for (int j = 1; j <= colCount; j++)
                    {
                        //new line
                        if (j == 1)
                            Console.Write("\r\n");

                        //write the value to the con sole
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                    }
                }

                //close and release
                xlWorkbook.Close(0);

                //quit and release
                xlApp.Quit();
        }

        public void getExcelFileKBank(string fileName)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbooks xlWorkbooks = xlApp.Workbooks;
            
            string filePath = String.Format("{0}{1}", config.emailFolderPath, fileName);
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@filePath);
            Excel.Sheets xlSheets = xlWorkbook.Sheets;  
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            int rowAccount = 0;
            for (int i = 1; i <= rowCount; i++)
            {
                if (excelCheckKbankForm(xlRange, i))
                {
                    rowAccount = i;
                }

                if (i > rowAccount + 1 && rowAccount > 0)
                {
                    DateTime dateKbankExcel = KbankDate(xlRange);

                    if (xlRange.Cells[i, 1].Value2 != null)
                    {
                        KbankPropertyCollection(
                            xlRange.Cells[i, 2].Value2.ToString()
                            , xlRange.Cells[i, 3].Value2.ToString()
                            , xlRange.Cells[i, 4].Value2.ToString()
                            //, xlRange.Cells[i, 5].Value2.ToString()
                            //, xlRange.Cells[i, 6].Value2.ToString()
                            //, xlRange.Cells[i, 7].Value2.ToString()
                            , xlRange.Cells[i, 8].Value2.ToString()
                            , xlRange.Cells[i, 9].Value2.ToString()
                            //, xlRange.Cells[i, 10].Value2.ToString()
                            //, xlRange.Cells[i, 11].Value2.ToString()
                            , dateKbankExcel
                            );
                    }
                    else
                        break;
                }
            }

            //close and release
            xlWorkbook.Close();

            //quit and release
            xlApp.Quit();
            //...clean up...  
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlSheets);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorkbooks);

            EmailSageProcess();

        }

        private bool excelCheckKbankForm(Excel.Range xlRange, int i)
        {
            if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
            {
                if (xlRange.Cells[i, 1].Value2.ToString() == "Account No"){

                    if (xlRange.Cells[i, 2] != null 
                        && xlRange.Cells[i, 2].Value2 != null
                        && xlRange.Cells[i, 3] != null 
                        && xlRange.Cells[i, 3].Value2 != null
                        )
                    {
                        if (xlRange.Cells[i, 2].Value2.ToString() == config.kbankAccount)
                            return true;
                        else if (xlRange.Cells[i, 3].Value2.ToString() == config.kbankAccount)
                            return true;
                        else
                            return false;
                    }
                    else if (xlRange.Cells[i, 2] == null
                        && xlRange.Cells[i, 2].Value2 == null
                        && xlRange.Cells[i, 3] != null
                        && xlRange.Cells[i, 3].Value2 != null
                        )
                    {
                        if (xlRange.Cells[i, 3].Value2.ToString() == config.kbankAccount)
                            return true;
                        else
                            return false;
                    }
                    else if (xlRange.Cells[i, 2] != null
                    && xlRange.Cells[i, 2].Value2 != null
                    && xlRange.Cells[i, 3] == null
                    && xlRange.Cells[i, 3].Value2 == null
                    )
                    {
                        if (xlRange.Cells[i, 2].Value2.ToString() == config.kbankAccount)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private DateTime KbankDate(Excel.Range xlRange)
        {
            DateTime kbankDate = DateTime.Now;
            if (xlRange.Cells[2, 6] != null && xlRange.Cells[2, 6].Value2 != null)
            {
                string strKbankDate = xlRange.Cells[2, 6].Value2.ToString();
                int l = strKbankDate.IndexOf(" ");
                if (l > 0)
                {
                    int startDate = l + 2;
                    string kDate = strKbankDate.Substring(startDate, strKbankDate.Length - startDate);
                    kbankDate = DateTime.ParseExact(kDate, "dd-MM-yy", CultureInfo.InvariantCulture);
                }
            }
            return kbankDate;
        }

        private void EmailSageProcess()
        {
            sage = new SageService(config.sageUser, config.sagePassword);
            foreach (KbankProperty item in _kbankProperty)
            {

                //Generate orde_coutomerderid

                //+1 customid

                //Bank tranfer -> Kbank Deposit
                Console.Write("\r\n");
                Console.Write(item.Tx + "\t");
                Console.Write(item.Ref1 + "\t");
                Console.Write(item.Ref2 + "\t");
                //Console.Write(item.Payer + "\t");
                Console.Write(item.Amount + "\t");
                Console.Write(item.Time + "\t");
                Console.Write(item.KbankDate.Year + "\t");
                //Console.Write(item.TellerId + "\t");
                //Console.Write(item.Branch + "\t");
                string orde_customorderid = sage.SageQueryCustomID();

                //Console.WriteLine(orde_customorderid);

                sage.SageInsertOrder(item.Tx, item.Ref1, item.Ref2, item.Amount, item.Time, item.KbankDate, orde_customorderid);
                //sage.SageUpdateCustomId();

            }

            sage.Dispose();
        }

        private void killExcelProcess()
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
