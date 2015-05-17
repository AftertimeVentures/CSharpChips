using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky7.CSharpChips.Exceptions {
    /// <summary>
    /// A helper structure to return an exception or the actual invokation result from ExceptionSafe Invoke methods.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public struct ExceptionOrResult<TResult> {
        public Exception Exception;
        public TResult Result;
    }
    /// <summary>
    /// A static class providing a set of methods to safely invoke methods that may throw exceptions
    /// without the need to explicitely wrap the invokation in a try {} catch() {} block. The class defines
    /// methods to invoke methods without return value, as well as returning methods.
    /// </summary>
    public static class ExceptionSafe {
        /// <summary>
        /// Safely invoke the given action with arg1 as parameter.
        /// </summary>
        /// <typeparam name="T">Type of action parameter arg1.</typeparam>
        /// <param name="action">The delegate to invoke.</param>
        /// <param name="arg1">The parameter to invoke the action with.</param>
        /// <returns>An exception shall one be fired fron the action during invokation, null otherwise.</returns>
        public static Exception Invoke<T>(Action<T> action, T arg1) {
            Exception invokationException = null;

            try {
                action(arg1);
            } catch (Exception ex) {
                invokationException = ex;
            }

            return invokationException;
        }
        /// <summary>
        /// Safely invoke the given func and return the result or the exceptuion if one is thrown.
        /// </summary>
        /// <typeparam name="T">The type of func parameter.</typeparam>
        /// <typeparam name="TResult">The type of func return value.</typeparam>
        /// <param name="func">The delegate to invoke.</param>
        /// <param name="arg1">The parameter to invoke func with.</param>
        /// <returns></returns>
        public static ExceptionOrResult<TResult> Invoke<T, TResult>(Func<T, TResult> func, T arg1) {
            ExceptionOrResult<TResult> invokationResult = new ExceptionOrResult<TResult>();

            try {
                invokationResult.Result = func(arg1);
            } catch (Exception ex) {
                invokationResult.Exception = ex;
            }

            return invokationResult;
        }
    }
}
