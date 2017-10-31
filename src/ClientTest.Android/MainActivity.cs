using System;
using System.Reflection;
using Android.App;
using Android.OS;
using NUnit.Common;
using NUnitLite;

//using Xamarin.Android.NUnitLite;

namespace ClientTest.Android
{
    [Activity(Label = "ClientTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            //AddTest(Assembly.GetExecutingAssembly());
            //AddTest(typeof(ClientTest.JsonServiceClientTests).Assembly);
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);
            var writer = new ExtendedTextWrapper(Console.Out);
            var result = new AutoRun(typeof(ClientTest.Tests.JsonServiceClientTests).Assembly).Execute(new string[0], writer, Console.In);



            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}

