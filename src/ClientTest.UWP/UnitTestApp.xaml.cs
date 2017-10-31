using System.Reflection;
using NUnit.Runner.Services;
using Xamarin.Forms.Platform.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientTest.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnitTestsPage : WindowsPage

    {
        public UnitTestsPage()
        {
            this.InitializeComponent();

            var nunit = new NUnit.Runner.App();

            // If you want to add tests in another assembly, add a reference and
            // duplicate the following line with a type from the referenced assembly
            nunit.AddTestAssembly(typeof(UnitTestsPage).GetTypeInfo().Assembly);

            // Do you want to automatically run tests when the app starts?
            nunit.Options = new TestOptions
            {
                AutoRun = true
            };

            LoadApplication(nunit);
        }
    }
}