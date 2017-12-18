using Xamarin.Forms;

namespace drawer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //NavigationPage

            MainPage = new DrawerContainerPage
            {
                CurrentPage = new drawerPage(),
                Drawer = new MemuPage()
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
