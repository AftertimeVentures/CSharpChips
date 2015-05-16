using System;
using System.Collections.Generic;
using System.Linq;

//namespace Sky7.GroupWebsite.Application.Frontend.Web {
//    public sealed class ExceptionPipeline {
//        public ExceptionPipeline(Exception exception) {
//            originalException = exception;
//        }

//        public void AddTranslator(Func<Exception, Exception> translator) {
//            translators.Add(translator);
//        }

//        public Exception Exception {
//            get {
//                Exception translatedException = TryTranslate();

//                if (translatedException == null)
//                    translatedException = originalException;

//                return translatedException;
//            }
//        }

//        public Exception Default(Exception translatedException) {
//            Exception ex = TryTranslate();

//            if (ex == null)
//                ex = translatedException;

//            return ex;
//        }

//        private Exception TryTranslate() {
//            Exception translatedException = null;

//            foreach (Func<Exception, Exception> translator in translators) {
//                if ((translatedException = translator(originalException)) != null)
//                    break;
//            }

//            return translatedException;
//        }

//        private readonly Exception originalException;
//        private readonly List<Func<Exception, Exception>> translators = new List<Func<Exception, Exception>>();
//    }

//    public static class ExceptionTranslationHelpers {
//        public static ExceptionPipeline Transform<T>(this Exception ex, Func<T, Exception> translator) where T : Exception {
//            return Transform<T>(new ExceptionPipeline(ex), translator);
//        }

//        public static ExceptionPipeline Transform<T>(this Exception ex, Func<T, bool> filter, Func<T, Exception> translator) where T : Exception {
//            return Transform<T>(new ExceptionPipeline(ex), filter, translator);
//        }

//        public static ExceptionPipeline Transform<T>(this ExceptionPipeline context, Func<T, Exception> translator) where T : Exception {
//            context.AddTranslator((e) => e is T ? translator(e as T) : null);

//            return context;
//        }

//        public static ExceptionPipeline Transform<T>(this ExceptionPipeline context, Func<T, bool> filter, Func<T, Exception> translator) where T : Exception {
//            context.AddTranslator((e) => e is T && filter(e as T) ? translator(e as T) : null);

//            return context;
//        }

//        public static ExceptionPipeline Preserve<T>(this ExceptionPipeline context) where T : Exception {
//            context.AddTranslator(e => e is T ? e : null);

//            return context;
//        }

//        public static ExceptionPipeline Preserve<T>(this ExceptionPipeline context, Func<T, bool> filter) where T : Exception {
//            context.AddTranslator(e => e is T && filter(e as T) ? e : null);

//            return context;
//        }

//        public static ExceptionPipeline Suppress<T>(this ExceptionPipeline context) where T : Exception {
//            context.AddTranslator(e => e is T ? new Exceptions.SuppressedException() : null);

//            return context;
//        }

//        public static void ThrowUnlessSuppressed(this Exception ex) {
//            if (ex != null && !(ex is Exceptions.SuppressedException))
//                throw ex;
//        }
//    }
//}