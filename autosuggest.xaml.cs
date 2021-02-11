using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace Telerik.Windows.Examples.AutoSuggestBox.FirstLook
{
    public partial class Example : UserControl
    {
        private RadCallout callout;
        private CalloutPopupSettings calloutSettings;
        private DispatcherTimer calloutTimer;

        public Example()
        {
            InitializeComponent();
            this.callout = new RadCallout()
            {
                StrokeThickness = 0,
                Content = "Text copied to clipboard",
                ArrowAnchorPoint = new Point(0.5, -0.2),
                ArrowBasePoint1 = new Point(0.45, 0),
                ArrowBasePoint2 = new Point(0.55, 0),
            };

            this.calloutSettings = new CalloutPopupSettings()
            {
                AutoClose = false,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                VerticalOffset = 2,
                ShowAnimationType = CalloutAnimation.None,
                CloseAnimationType = CalloutAnimation.None,
            };

            this.calloutTimer = new DispatcherTimer();
            this.calloutTimer.Interval = TimeSpan.FromSeconds(3);
            this.calloutTimer.Tick += OnCalloutCloseTimerTick;

        }

        private void OnCalloutCloseTimerTick(object sender, EventArgs e)
        {
            if (this.calloutTimer.IsEnabled)
            {
                CalloutPopupService.Close(this.callout);
            }
        }

        private void RadPathButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.glyphTextContentTb.Text);
            this.calloutTimer.Stop();
            CalloutPopupService.Close(this.callout);
            CalloutPopupService.Show(this.callout, sender as FrameworkElement, this.calloutSettings);
            this.calloutTimer.Start();
        }
    }
}
