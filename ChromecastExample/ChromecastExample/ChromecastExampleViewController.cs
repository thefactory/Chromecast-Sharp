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
            castDeviceManager = new ChromecastManager();
            castDeviceManager.ScanStarted += () => {
                statusLabel.Text = "Scan started";
            };
            castDeviceManager.ScanStopped += () => {
                statusLabel.Text = "Scan stopped";
            };
            castDeviceManager.DeviceCameOnline += (ChromecastDevice device) => {
                statusLabel.Text = String.Format("Online: {0}", device);

                castDeviceManager.StopScan();

                castDeviceManager.StartApplication(device, "YOUR_APPID_HERE");
            };
            castDeviceManager.ApplicationSessionStarted += (ChromecastSession session) => {
                ChromecastStream stream = new ChromecastStream(session, "other");
                statusLabel.Text = "ApplicationSessionStarted";
                stream.Send(new Dictionary<string, object>() { { "text", "Sent from my iPhone" } });
            };
            castDeviceManager.Scan(TimeSpan.FromSeconds(10));
        }
    }
}

