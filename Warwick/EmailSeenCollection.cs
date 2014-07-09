using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warwick
{
    public class EmailSeenCollection : ObjectBase
    {
        private string _EmailUid = "";

        public EmailSeenCollection(string uid)
        {
            _EmailUid = uid;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailSeenCollection()
        {
            //nothing
        }

        /// <summary>
        /// Person' First Name
        /// </summary>
        public string EmailUid
        {
            get
            {
                return _EmailUid;
            }
            set
            {
                _EmailUid = value;
            }
        }
    }
}
