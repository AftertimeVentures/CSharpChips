using System;

namespace Sky7.CSharpChips.Exceptions {
    /// <summary>
    /// SuppressedException is a special class of exception that is used in ExceptionCookBook internals to indicate the fact
    /// that the original exception was suppressed. It is not recommended to use SuppressedException in user code.
    /// </summary>
    internal sealed class SuppressedException : Exception {
    }
}
