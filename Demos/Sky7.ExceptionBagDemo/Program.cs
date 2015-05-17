using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Sky7.CSharpChips.Exceptions;

namespace Sky7.ExceptionBagDemo {
    class Program {
        public const String EMailAddressPattern = "";
        public const String NameRequiredMessage = "Name is required";
        public const String EMailAddressRequiredMessage = "E-mail is required";
        public const String InvalidEMailAddressMessageTemplate = "'{0}' is not a valid e-mail address";
        public const String ErrorDisplayTemplate = "- {0}.";
        public static readonly String HorizontalLine = RenderHorizontalLine();

        static Int32 Main(string[] args) {
            bool operationComplete = false;

            String name = null;
            String emailAddress = null;

            do {
                Console.Write(HorizontalLine);
                Console.WriteLine("Sky7.ExceptionBag demo app");
                Console.Write(HorizontalLine);
                Console.Write("Name: ");
                name = Console.ReadLine();

                Console.Write("E-mail: ");
                emailAddress = Console.ReadLine();

                Console.Write(HorizontalLine);

                try {
                    using (ExceptionBag exceptionBag = new ExceptionBag()) {
                        if (String.IsNullOrEmpty(name))
                            exceptionBag.Put(new ArgumentException(NameRequiredMessage));

                        if (String.IsNullOrEmpty(emailAddress)) {
                            exceptionBag.Put(new ArgumentException(EMailAddressRequiredMessage));
                        } else {
                            if (!Regex.IsMatch(emailAddress, EMailAddressPattern))
                                exceptionBag.Put(new ArgumentException(String.Format(InvalidEMailAddressMessageTemplate, emailAddress)));
                        }
                    }

                    Console.WriteLine("Thank you, the demo is complete.");
                    operationComplete = true;
                } catch (ExceptionBag ex) {
                    Console.WriteLine("The following errors occured:");

                    foreach (Exception outOfBagException in ex) {
                        Console.WriteLine(String.Format(ErrorDisplayTemplate, outOfBagException.Message));
                    }

                    Console.Write(HorizontalLine);
                    Console.WriteLine("Please, try to enter your data once again. Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                } catch (Exception) {
                    return HandleTerminalException();
                }
            } while (!operationComplete);

            return 0;
        }

        private static String RenderHorizontalLine() {
            StringBuilder line = new StringBuilder();

            line.Append('=', Console.WindowWidth);

            return line.ToString();
        }

        private static Int32 HandleTerminalException() {
            Console.WriteLine("Unexpected error occured. The application will now terminate.");

            return 1;
        }
    }
}
