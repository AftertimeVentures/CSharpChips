using System;
using System.Collections.Generic;
using System.Linq;

namespace Sky7.Exceptions {
    /// <summary>
    /// ExceptionBag comes handy when one needs to make multiple checks and produce particular exceptions 
    /// for each check that fails, but make sure that all chacks are performed before the exception is actually fired.
    /// One frequent use case is when a method must validate its parameters and inform the caller of all validation
    /// errors by means of a single exception. This single exception constraint is important when the actual code 
    /// that produced invalid inputs (and must therefore deal with the exceptional situation), is running on a remote host, 
    /// being for example a piece of javascript running in the user's browser.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExceptionBag<T> : Exception, IDisposable, IEnumerable<T> where T : Exception {

        // Constructor(s)
        /// <summary>
        /// The default constructor. Creates an exception bag that will fire itself once disposed while non-empty.
        /// </summary>
        public ExceptionBag() 
            : this(DefaultAggregateFunc) {

        }

        /// <summary>
        /// Creates and exception bag that will wrap the exception in it using the supplied wrapper delegate
        /// and then fire the resulting exception.
        /// </summary>
        /// <param name="wrapper">The delegate to wrap a set of exceptions into one.</param>
        public ExceptionBag(Func<ExceptionBag<T>, Exception> wrapper) {
            if (wrapper == null)
                throw new ArgumentNullException("wrapper");

            this.aggregator = wrapper;
            elementaryExceptions = new List<T>();
        }

        //  ExceptionBag interface method(s)
        public void Put(T exception) {
            elementaryExceptions.Add(exception);
        }

        public bool IsEmpty {
            get {
                return elementaryExceptions.Count() == 0;
            }
        }

        //  IDisposable method(s)
        public void Dispose() {
            if (!IsEmpty)
                throw aggregator(this);
        }

        //  IEnumerable<T> methods
        public IEnumerator<T> GetEnumerator() {
            return elementaryExceptions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return elementaryExceptions.GetEnumerator();
        }

        //  Auxilliary routines (private)
        private static Exception DefaultAggregateFunc(ExceptionBag<T> exceptionBag) {
            return exceptionBag;
        }

        //  Private fields
        private readonly Func<ExceptionBag<T>, Exception> aggregator;
        private readonly List<T> elementaryExceptions;
    }

    public class ExceptionBag : ExceptionBag<Exception> {

    }
}