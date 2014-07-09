using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warwick
{
    public class KbankProperty : ObjectBase
    {
        private string _Tx = "";
        private string _Ref1 = "";
        private string _Ref2 = "";
        //private string _Ref3 = "";
        //private string _Payer = "";
        //private string _ChequeBankCode = "";
        private string _Amount = "";
        private string _Time = "";
        //private string _TellerId = "";
        //private string _Branch = "";
        private DateTime _KbankDate = DateTime.Now;


        public KbankProperty(string tx
            , string ref1
            , string ref2
            //, string ref3
            //, string payer
            //, string chequeBankCode
            , string amount
            , string time
            //, string tellerId
            //, string branch
            , DateTime kbankDate
            )
        {
            _Tx = tx;
            _Ref1 = ref1;
            _Ref2 = ref2;
            //_Ref3 = ref3;
            //_Payer = payer;
            //_ChequeBankCode = chequeBankCode;
            _Amount = amount;
            _Time = time;
            //_TellerId = tellerId;
            //_Branch = branch;
            _KbankDate = kbankDate;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public KbankProperty()
        {
            //nothing
        }

        public string Tx
        {
            get
            {
                return _Tx;
            }
            set
            {
                _Tx = value;
            }
        }

        public string Ref1
        {
            get
            {
                return _Ref1;
            }
            set
            {
                _Ref1 = value;
            }
        }

        public string Ref2
        {
            get
            {
                return _Ref2;
            }
            set
            {
                _Ref2 = value;
            }
        }

        //public string Ref3
        //{
        //    get
        //    {
        //        return _Ref3;
        //    }
        //    set
        //    {
        //        _Ref3 = value;
        //    }
        //}

        //public string Payer
        //{
        //    get
        //    {
        //        return _Payer;
        //    }
        //    set
        //    {
        //        _Payer = value;
        //    }
        //}

        //public string ChequeBankCode
        //{
        //    get
        //    {
        //        return _ChequeBankCode;
        //    }
        //    set
        //    {
        //        _ChequeBankCode = value;
        //    }
        //}

        public string Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        public string Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
            }
        }

        //public string TellerId
        //{
        //    get
        //    {
        //        return _TellerId;
        //    }
        //    set
        //    {
        //        _TellerId = value;
        //    }
        //}

        //public string Branch
        //{
        //    get
        //    {
        //        return _Branch;
        //    }
        //    set
        //    {
        //        _Branch = value;
        //    }
        //}

        public DateTime KbankDate
        {
            get
            {
                return _KbankDate;
            }
            set
            {
                _KbankDate = value;
            }
        }

    }
}
