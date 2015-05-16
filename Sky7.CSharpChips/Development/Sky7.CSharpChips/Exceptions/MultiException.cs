using System;
using System.Collections.Generic;
using System.Linq;

namespace Sky7.CSharpChips.Exceptions {
    /// <summary>
    /// MultiException is an exception class the only purpose of which is to aggregate a set
    /// of exceptions in one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiException<T> : Exception, IEnumerable<T> {
        public MultiException(IEnumerable<T> exceptions) {
            this.exceptions.AddRange(exceptions);
        }

        //  IEnumerable<T> members
        public IEnumerator<T> GetEnumerator() {
            return exceptions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return exceptions.GetEnumerator();
        }

        private List<T> exceptions = new List<T>();
    }

    /// <summary>
    /// A shorthand for MultiException&lt;Exception&gt;.
    /// </summary>
    public class MultiException : MultiException<Exception> {
        public MultiException(IEnumerable<Exception> exceptions) 
            : base(exceptions) {

        }
    }
}