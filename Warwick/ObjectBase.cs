using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warwick
{
    public abstract class ObjectBase
    {
          #region "Member Variables"

        protected Guid? _UniqueId;  //local member variable which stores the object's UniqueId

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObjectBase()
        {
            //create a new unique id for this business object
            _UniqueId = Guid.NewGuid();
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// UniqueId property for every business object
        /// </summary>
        public Guid? UniqueId
        {
            get
            {
                return _UniqueId;
            }
            set
            {
                _UniqueId = value;
            }
        }

        #endregion
    }
}
