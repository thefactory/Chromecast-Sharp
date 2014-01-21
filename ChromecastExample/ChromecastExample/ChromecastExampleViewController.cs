using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Google.Chromecast.iOS;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;

namespace ChromecastExample {
    public partial class ChromecastExampleViewController : UIViewController {
        ChromecastManager castDeviceManager;
        UILabel statusLabel;

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

            statusLabel = new UILabel(new RectangleF(10, 50, 300, 20)) {
                Text = "Loading ..."
            };
            View.AddSubview(statusLabel);

            SetupChromecast();
        }

        private void SetupChromecast() {
            castDeviceManager = new ChromecastManager("Test/1.0");
            castDeviceManager.ScanStarted += () => {
                statusLabel.Text = "Scan started";
            };
            castDeviceManager.ScanStopped += () => {
                statusLabel.Text = "Scan stopped";
            };
            castDeviceManager.DeviceCameOnline += (ChromecastDevice device) => {
                statusLabel.Text = String.Format("Online: {0}", device);

                castDeviceManager.StopScan();

                castDeviceManager.StartApplication(device, "77ad2269-71f2-4825-a030-637aea30dc8b");
            };
            castDeviceManager.ApplicationSessionStarted += (ChromecastSession session) => {
                int index = 0;
                ChromecastStream stream = new ChromecastStream(session, "other");
                statusLabel.Text = "ApplicationSessionStarted";
                Size maxSize = new Size(1280, 720);

                NSTimer timer = NSTimer.CreateRepeatingTimer(TimeSpan.FromMilliseconds(33), () => {
                    UIGraphics.BeginImageContext(maxSize);
                    CGContext ctx = UIGraphics.GetCurrentContext();

                    ctx.ClearRect(new RectangleF(new PointF(0, 0), maxSize));
                    ctx.SetFillColor(UIColor.Green.CGColor);
                    ctx.FillEllipseInRect(new RectangleF(index, index, 20, 20));

                    UIImage image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();

                    stream.Send(new Dictionary<string, object>() { { "count", index }, { "image", String.Format("data:image/png;base64,{0}", image.AsPNG().GetBase64EncodedString(NSDataBase64EncodingOptions.None)) } });
                    index = (index + 1) % Math.Min(maxSize.Width, maxSize.Height);
                });
                NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Default);
            };
            castDeviceManager.Scan(TimeSpan.FromSeconds(10));
        }
    }
}

