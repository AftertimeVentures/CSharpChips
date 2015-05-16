using System;
using System.Collections.Generic;
using System.Linq;

namespace Sky7.CSharpChips.Exceptions {
    public class MultiException<T> : Exception {
        public MultiException(IEnumerable<T> exceptions) {
            this.exceptions.AddRange(exceptions);
        }

        private List<T> exceptions = new List<T>();
    }

    public class MultiException : MultiException<Exception> {
        public MultiException(IEnumerable<Exception> exceptions) 
            : base(exceptions) {

        }
    }
}