using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;
using NUnit.Common;
using NUnitLite;

namespace ClientTest.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "UnitTestAppDelegate"
            // you can specify it here.
            //UIApplication.Main(args, null, "UnitTestAppDelegate");

            // tests can be inside the main assembly
            //AddTest(Assembly.GetExecutingAssembly());
            //AddTest(typeof(ClientTest.JsonServiceClientTests).Assembly);
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);
            var writer = new ExtendedTextWrapper(Console.Out);
            var result = new AutoRun(typeof(ClientTest.Tests.JsonServiceClientTests).Assembly).Execute(new[] { "--labels=All", "--teamcity" }, writer, Console.In);

        }
    }
}
