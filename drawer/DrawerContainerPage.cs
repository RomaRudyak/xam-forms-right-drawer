using System;
using Xamarin.Forms;

namespace drawer
{
    [ContentProperty(nameof(CurrentPage))]
    public class DrawerContainerPage : Page, IPageContainer<Page>
    {
        public static BindableProperty CurrentPageProperty =
            BindableProperty.Create(
                nameof(CurrentPage),
                typeof(Page),
                typeof(DrawerContainerPage),
                default(Page)
            );

        public static BindableProperty DrawerProperty =
            BindableProperty.Create(
                nameof(Drawer),
                typeof(Page),
                typeof(DrawerContainerPage),
                default(Page)
            );

        public Page CurrentPage
        {
            get => (Page)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public Page Drawer
        {
            get => (Page)GetValue(DrawerProperty);
            set => SetValue(DrawerProperty, value);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            CurrentPage?.Layout(new Rectangle(x, y, width, height));
        }
    }
}
