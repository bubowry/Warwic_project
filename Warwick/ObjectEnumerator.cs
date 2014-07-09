using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warwick
{
    public class ObjectEnumerator<T> : IEnumerator<T> where T : ObjectBase
    {
        protected ObjectCollection<T> _collection;  //enumerated collection
        protected int index;                                //current index
        protected T _current;

        public ObjectEnumerator(ObjectCollection<T> collection)
        {
            _collection = collection;
            index = -1;
            _current = default(T);
        }

        #region "Properties"

        /// <summary>
        /// Current Enumerated object in the inner collection
        /// </summary>
        public virtual T Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// Explicit non-generic interface implementation for IEnumerator (extended and required by IEnumerator<T>)
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Dispose method
        /// </summary>
        public virtual void Dispose()
        {
            _collection = null;
            _current = default(T);
            index = -1;
        }

        /// <summary>
        /// Move to next element in the inner collection
        /// </summary>
        /// <returns></returns>
        public virtual bool MoveNext()
        {
            //make sure we are within the bounds of the collection
            if (++index >= _collection.Count)
            {
                //if not return false
                return false;
            }
            else
            {
                //if we are, then set the current element to the next object in the collection
                _current = _collection[index];
            }

            //return true
            return true;
        }

        /// <summary>
        /// Reset the enumerator
        /// </summary>
        public virtual void Reset()
        {
            _current = default(T); //reset current object
            index = -1;
        }

        #endregion

    }
}
