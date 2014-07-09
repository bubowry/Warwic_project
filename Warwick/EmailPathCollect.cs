using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warwick
{
    public class EmailPathCollect : ObjectBase
    {
        private string _EmailPath = "";

        public EmailPathCollect(string path)
        {
            _EmailPath = path;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailPathCollect()
        {
            //nothing
        }

        /// <summary>
        /// Person' First Name
        /// </summary>
        public string EmailPath
        {
            get
            {
                return _EmailPath;
            }
            set
            {
                _EmailPath = value;
            }
        }
    }
}
