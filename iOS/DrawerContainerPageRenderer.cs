using System;
using drawer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.ComponentModel;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(
    typeof(DrawerContainerPage),
    typeof(drawer.iOS.DrawerContainerPageRenderer)
)]

namespace drawer.iOS
{
    public class DrawerContainerPageRenderer : UIViewController, IVisualElementRenderer
    {
        private bool _disposed;
        private bool _appeared;
        private VisualElementPackager _packager;
        private VisualElementTracker _tracker;
        private EventTracker _events;

        public VisualElement Element { get; private set; }

        public UIView NativeView => _disposed ? null : View;

        public UIViewController ViewController => _disposed ? null : this;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
            => NativeView.GetSizeRequest(widthConstraint, heightConstraint);

        public void SetElement(VisualElement element)
        {
            VisualElement oldElement = Element;
            Element = element;


            ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(oldElement, element));

        }

        public void SetElementSize(Size size)
            => Element.Layout(new Rectangle(Element.X, Element.Y, size.Width, size.Height));

        Page Page => Element as Page;

        public DrawerContainerPageRenderer()
        {

        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_appeared || _disposed)
                return;

            _appeared = true;
            Page.SendAppearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (!_appeared || _disposed)
                return;

            _appeared = false;
            Page.SendDisappearing();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //var uiTapGestureRecognizer = new UITapGestureRecognizer(a => View.EndEditing(true));

            //uiTapGestureRecognizer.ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true;
            //uiTapGestureRecognizer.ShouldReceiveTouch = OnShouldReceiveTouch;
            //uiTapGestureRecognizer.DelaysTouchesBegan =
            //    uiTapGestureRecognizer.DelaysTouchesEnded = uiTapGestureRecognizer.CancelsTouchesInView = false;
            //View.AddGestureRecognizer(uiTapGestureRecognizer);

            UpdatedCurrentPage();

            _packager = new VisualElementPackager(this);
            _packager.Load();

            Element.PropertyChanged += OnHandlePropertyChanged;
            _tracker = new VisualElementTracker(this);

            _events = new EventTracker(this);
            _events.LoadEvents(View);

            //Element.SendViewInitialized(View);
        }

        private void OnHandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == DrawerContainerPage.CurrentPageProperty.PropertyName)
            {
                UpdatedCurrentPage();
            }
        }

        private void UpdatedCurrentPage()
        {
            var current = (Element as IPageContainer<Page>).CurrentPage;
            IVisualElementRenderer renderer = null;
            if ((renderer = Platform.GetRenderer(current)) == null)
            {
                Platform.SetRenderer(current, renderer = Platform.CreateRenderer(current));
            }

            renderer.NativeView.Frame = View.Frame;
            renderer.NativeView.BackgroundColor = UIColor.Red;

            current.ForceLayout();

            View.Subviews.ForEach(View => View.RemoveFromSuperview());
            View.AddSubview(renderer.NativeView);
            ViewController.AddChildViewController(renderer.ViewController);
        }
    }
}
