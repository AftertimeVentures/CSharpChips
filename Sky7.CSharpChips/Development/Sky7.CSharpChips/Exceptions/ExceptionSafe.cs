using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky7.CSharpChips.Exceptions {
    public struct ExceptionOrResult<TResult> {
        public Exception Exception;
        public TResult Result;
    }

    public static class ExceptionSafe {
        public static Exception Invoke<T>(Action<T> action, T arg1) {
            Exception invokationException = null;

            try {
                action(arg1);
            } catch (Exception ex) {
                invokationException = ex;
            }

            return invokationException;
        }

        public static ExceptionOrResult<TResult> Invoke<T, TResult>(Func<T, TResult> action, T arg1) {
            ExceptionOrResult<TResult> invokationResult = new ExceptionOrResult<TResult>();

            try {
                invokationResult.Result = action(arg1);
            } catch (Exception ex) {
                invokationResult.Exception = ex;
            }

            return invokationResult;
        }
    }
}
