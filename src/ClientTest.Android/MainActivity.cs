using System;
using System.IO;
using System.Text;
using Android.App;
using Android.OS;
using Android.Widget;
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


            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            var txtOutput = (TextView)FindViewById(Resource.Id.txtOutput);

            var writer = new ExtendedTextWrapper(new MultiTextWriter(new TextViewWriter(txtOutput), Console.Out));
            var result = new AutoRun(typeof(ClientTest.Tests.JsonServiceClientTests).Assembly).Execute(new[] { "--labels=All", "--teamcity" }, writer, Console.In);
        }
    }

    public class TextViewWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.Default;

        private readonly TextView txtView;
        public TextViewWriter(TextView txtView) => this.txtView = txtView;

        public override void Write(char value)
        {
            txtView.Append(value.ToString());
        }

        public override void WriteLine(string value)
        {
            txtView.Append(value + System.Environment.NewLine);
        }
    }

    public class MultiTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.Default;

        private readonly TextWriter[] writers;
        public MultiTextWriter(params TextWriter[] writers) => this.writers = writers;

        public override void Write(char value)
        {
            foreach (var writer in writers)
            {
                writer.Write(value);
            }
        }
    }
}

