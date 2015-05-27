using System;
using System.Collections.Generic;
using System.Linq;

namespace Sky7.CSharpChips.Exceptions {
    /// <summary>
    /// ExceptionCookBook allows a developer to specify rules called recipes that define how an exception
    /// is to be treated. Recipes are simple instructions to transform an exception into a different kind 
    /// of exception, suppress certain exceptions, or keep them unchanged. Adding a recipe is simple: just
    /// call the AddRecipe methood and supply a proper delegate as parameter. However it is recommended to 
    /// use extension methods defined in ExceptionCookBookExtensions class that allow to define various 
    /// kinds of recipes using the fluent API syntax. For more information on specific recipy methods,
    /// please refer to documentation on the ExceptionCookBookExtensions class.
    /// When applied to an exception, the cookbok tries to apply each recipe in the order of addition. If 
    /// the recipy (transform) returns null, the recipy is considereed as inapplicable to the exception,
    /// and the next recipy is picked. Once a recipy returns a not null Exception object, that exception 
    /// is chosen as the output of the cookbook. The exception that appears as the result can be accessed
    /// via the read-only ResultingException property. Accessing ResultingException property causes
    /// the cookbok to execute the list of recipes and find the suitable one. The resulting exception can 
    /// also be thrown by applying the ThrowUnlessSuppressed extension method to the cookbook. Doing so 
    /// will fire an the resulting exception unless it the exception was suppresed bythe cookbook.
    /// </summary>
    public sealed class ExceptionCookBook {
        //  Constructor
        /// <summary>
        /// Creates a new instance of ExceptionCookBook to be applied to the provided exception.
        /// </summary>
        /// <param name="exception">The exception to be "cooked" using the cookbook.</param>
        public ExceptionCookBook(Exception exception) {
            originalException = exception;
        }

        public Exception TranslatedExeption { get { return translatedException; } }

        public void AddTransform(Func<Exception, bool> sieve, Func<Exception, Exception> transform) {
            recipes.Add(
                e => {
                    bool result = false;

                    if (result = sieve(originalException))
                        translatedException = transform(originalException);

                    return result;
                }
            );
        }

        public void AddHandler(Func<Exception, bool> sieve, Action<Exception> handler) {
            throw new NotImplementedException();
        }

        public void AddSupression(Func<Exception, bool> sieve) {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public Object Result {
        //    get {
        //        Exception transformedException = TryTransform();

        //        if (transformedException == null)
        //            transformedException = originalException;

        //        return transformedException;
        //    }
        //}

        public Exception TransformDefault(Exception transformedException) {
            Exception ex = TryTransform();

            if (ex == null)
                ex = transformedException;

            return ex;
        }

        /// <summary>
        /// Apply the given transform if no other 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Exception TransformDefault(Func<Exception, Exception> transform) {
            Exception ex = TryTransform();

            if (ex == null)
                ex = transform(originalException);

            return ex;
        }

        //  Private methods
        /// <summary>
        /// Tries to "cook" the exception using the list of recipes in the cookbook. Returns the cooked exception
        /// or null if no suitable revipe was found.
        /// </summary>
        /// <returns>The cooked exception or null.</returns>
        private Exception TryTransform() {
            Exception transformedException = null;

            foreach (Func<Exception, bool> recipe in recipes) {
                if (recipe(originalException))
                    break;
            }

            return translatedException;
        }

        private void Execute() {
            foreach (Func<Exception, bool> recipe in recipes) {

            }
        }

        //  Private fields
        private readonly Exception originalException;
        private Exception translatedException;
        private readonly Object result;
        private readonly List<Func<Exception, bool>> recipes = new List<Func<Exception, bool>>();
    }

    public static class ExceptionCookBookExtensions {
        /// <summary>
        /// Creates a new cookbook to cook the given exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The esception to cook.</param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public static ExceptionCookBook Transform(this Exception ex, Func<Exception, Exception> recipe) {
            return Transform<Exception>(new ExceptionCookBook(ex), recipe);
        }
        /// <summary>
        /// Creates a new cookbook to cook the given exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The esception to cook.</param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public static ExceptionCookBook Transform<T>(this Exception ex, Func<T, Exception> recipe) where T : Exception {
            return Transform<T>(new ExceptionCookBook(ex), recipe);
        }
        /// <summary>
        /// Creates a new cookbook to cook the given exception and adds a recipe to transform exceptions of type T 
        /// that pass the given sieve using the recipe delegate.
        /// </summary>
        /// <typeparam name="T">The type of exceptions to aplly the new recipe to.</typeparam>
        /// <param name="cookBook">The cookbook to add the new recipe to.</param>
        /// <param name="sieve">The sieve to filter exceptions to which the recipe is applicable.</param>
        /// <param name="recipe">The delegate to transform the exception.</param>
        /// <returns>The cookbook to support fluent API.</returns>
        public static ExceptionCookBook Transform<T>(this Exception ex, Func<T, bool> sieve, Func<T, Exception> recipe) where T : Exception {
            return Transform<T>(new ExceptionCookBook(ex), sieve, recipe);
        }
        /// <summary>
        /// Adds a recipe to transform exceptions of type T using the recipe delegate.
        /// </summary>
        /// <typeparam name="T">The type of exceptions to aplly the new recipe to.</typeparam>
        /// <param name="cookBook">The cookbook to add the new recipe to.</param>
        /// <param name="transform">The delegate to transform the exception.</param>
        /// <returns>The cookbook to support fluent API.</returns>
        public static ExceptionCookBook Transform<T>(this ExceptionCookBook cookBook, Func<T, Exception> transform) where T : Exception {
            cookBook.AddTransform(e => e is T, e => transform(e as T));

            return cookBook;
        }
        /// <summary>
        /// Adds a recipe to transform exceptions of type T that pass the given sieve using the recipe delegate.
        /// </summary>
        /// <typeparam name="T">The type of exceptions to aplly the new recipe to.</typeparam>
        /// <param name="cookBook">The cookbook to add the new recipe to.</param>
        /// <param name="sieve">The sieve to filter exceptions to which the recipe is applicable.</param>
        /// <param name="tramsform">The delegate to transform the exception.</param>
        /// <returns>The cookbook to support fluent API.</returns>
        public static ExceptionCookBook Transform<T>(this ExceptionCookBook cookBook, Func<T, bool> sieve, Func<T, Exception> tramsform) where T : Exception {
            cookBook.AddTransform(e => e is T && sieve(e as T), e => tramsform(e as T));

            return cookBook;
        }
        /// <summary>
        /// Adds a recipe to keep exceptions of type T unchanged.
        /// </summary>
        /// <typeparam name="T">The type of exception to </typeparam>
        /// <param name="cookBook">The cookbook to add the new recipe to.</param>
        /// <returns></returns>
        public static ExceptionCookBook Preserve<T>(this ExceptionCookBook cookBook) where T : Exception {
            cookBook.AddTransform(e => e is T, e => e);

            return cookBook;
        }
        /// <summary>
        /// Adds a recipe to keep exceptions of type T that pass the given sieve unchanged.
        /// </summary>
        /// <typeparam name="T">The type of exception to </typeparam>
        /// <param name="cookBook">The cookbook to add the new recipe to.</param>
        /// <param name="sieve">The sieve to filter exceptions that are affected by the new recipe.</param>
        /// <returns></returns>
        public static ExceptionCookBook Preserve<T>(this ExceptionCookBook cookBook, Func<T, bool> sieve) where T : Exception {
            cookBook.AddTransform(e => e is T && sieve(e as T), e => e);

            return cookBook;
        }
        /// <summary>
        /// Adds a recipe to suppress exceptions of type T.
        /// </summary>
        /// <typeparam name="T">The type of exception to be suppressed by the new recipe.</typeparam>
        /// <param name="cookBook">The cookbok to add the new recipe to.</param>
        /// <returns></returns>
        public static ExceptionCookBook Suppress<T>(this ExceptionCookBook cookBook) where T : Exception {
            cookBook.AddSupression(e => e is T);

            return cookBook;
        }
        /// <summary>
        /// Adds a recipe to suppress exceptions of type T that passes the given sieve.
        /// </summary>
        /// <typeparam name="T">The type of exception to be suppressed by the new recipe.</typeparam>
        /// <param name="cookBook">The cookbok to add the new recipe to.</param>
        /// <param name="sieve">The sieve to filter exceptions.</param>
        /// <returns></returns>
        public static ExceptionCookBook Suppress<T>(this ExceptionCookBook cookBook, Func<T, bool> sieve) where T : Exception {
            cookBook.AddSupression(e => e is T && sieve(e as T));

            return cookBook;
        }

        public static ExceptionCookBook Handle<T>(this ExceptionCookBook cookBook, Action<T> handler) where T : Exception {
            cookBook.AddHandler(e => e is T, e => handler(e as T));

            return cookBook;
        }

        public static ExceptionCookBook Handle<T>(this ExceptionCookBook cookBook, Func<T, bool> sieve, Action<T> handler) where T : Exception {
            cookBook.AddHandler(e => e is T && sieve(e as T), e => handler(e as T));

            return cookBook;
        }
        /// <summary>
        /// Throws the ResultingException of the cookbook, unless it is a suppressed exception.
        /// </summary>
        /// <param name="cookBook">The ExceptionCookBook to throw exception from.</param>
        public static void ThrowUnlessSuppressed(this ExceptionCookBook cookBook) {
            if (!(cookBook.TranslatedExeption is SuppressedException))
                throw cookBook.TranslatedExeption;
        }

        /// <summary>
        /// Throws the ResultingException of the pot, unless it is as suppressed exception.
        /// </summary>
        /// <param name="exPot">The ExceptionCookBook to throw exception from.</param>
        public static void ThrowUnlessSuppressed(this Exception ex) {
            if (ex != null && !(ex is SuppressedException))
                throw ex;
        }
    }
}