using System;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Google.Chromecast.iOS {
    public class ChromecastManager : GCKApplicationSessionDelegate {
        private GCKContext context;
        private GCKDeviceManager manager;
        private ChromecastListener listener;
        private GCKApplicationSession session;
        //Device scanning
        public event Action ScanStarted;
        public event Action ScanStopped;
        public event Action<ChromecastDevice> DeviceCameOnline;
        public event Action<ChromecastDevice> DeviceWentOffline;
        //Session start/end
        public event Action<ChromecastSession> ApplicationSessionStarted;
        public event Action<NSError> SessionEnded, SessionStartFailed;

        public ChromecastManager() : this(false) {
        }

        public ChromecastManager(bool enableChromecastLogging) : this(enableChromecastLogging,
                                                                      String.Format("{0}/{1}", (NSString)NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleIdentifier"), 
                                                                          (NSString)NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion"))) {
        }

        public ChromecastManager(bool enableChromecastLogging, string userAgentString) {
            if (enableChromecastLogging) {
                GCKLogger.SharedInstance().Delegate = new ChromecastLogger();
            }
            context = new GCKContext(userAgentString);
            manager = new GCKDeviceManager(context);
            listener = new ChromecastListener(this);
            manager.AddListener(listener);
        }

        public void Scan(TimeSpan scanTime) {
            StartScan();

            NSTimer timer = NSTimer.CreateTimer(scanTime, () => {
                StopScan();
            });
            NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Default);
        }

        public void StartScan() {
            manager.StartScan();
        }

        public void StopScan() {
            manager.StopScan();
        }

        private class ChromecastListener : GCKDeviceManagerListener {
            private ChromecastManager manager;

            public ChromecastListener(ChromecastManager manager) {
                this.manager = manager;
            }

            public override void ScanStarted() {
                if (manager.ScanStarted != null) {
                    manager.ScanStarted();
                }
            }

            public override void ScanStopped() {
                if (manager.ScanStopped != null) {
                    manager.ScanStopped();
                }
            }

            public override void DeviceDidComeOnline(GCKDevice device) {
                if (manager.DeviceCameOnline != null) {
                    manager.DeviceCameOnline(new ChromecastDevice() { GCKDevice = device });
                }
            }

            public override void DeviceDidGoOffline(GCKDevice device) {
                if (manager.DeviceWentOffline != null) {
                    manager.DeviceWentOffline(new ChromecastDevice() { GCKDevice = device });
                }
            }
        }

        private class ChromecastLogger : GCKLoggerDelegate {
            public override void Message(IntPtr unmanagedFunctionName, string message) {
                string functionName = Marshal.PtrToStringAnsi(unmanagedFunctionName);
                Console.WriteLine("{0}: {1}", functionName, message);
            }
        }

        public bool StartApplication(ChromecastDevice device, string applicationName) {
            session = new GCKApplicationSession(context, device.GCKDevice);
            session.WeakDelegate = this;

            return session.StartSessionWithApplication(applicationName);
        }

        public override void ApplicationSessionDidStart() {
            if (ApplicationSessionStarted != null) {
                ApplicationSessionStarted(new ChromecastSession() { GCKApplicationSession = session });
            }
        }

        public override void ApplicationSessionDidFailToStart(GCKApplicationSessionError error) {
            if (SessionStartFailed != null) {
                SessionStartFailed(error);
            }
        }

        public override void ApplicationSessionDidEnd(GCKApplicationSessionError error) {
            if (SessionEnded != null) {
                SessionEnded(error);
            }
        }
    }

    public class ChromecastDevice {
        public GCKDevice GCKDevice { get; set; }
    }

    public class ChromecastSession {
        public GCKApplicationSession GCKApplicationSession { get; set; }
    }
    //Currently unable to subclass GCKMessageStream (which is what we would need to be able to handle DidReceiveMessage and get info back from the receiver)
    // without it causing infinite recursion/crashing.
    //  http://stackoverflow.com/questions/15750073/binding-issues-extending-a-class-leads-method-calling-itself didn't help.
    public partial class ChromecastStream {
        private GCKMessageStream stream;

        public ChromecastStream(ChromecastSession session, string protocolNamespace) {
            if (session.GCKApplicationSession.Channel != null) {
                stream = new GCKMessageStream(protocolNamespace);
                session.GCKApplicationSession.Channel.AttachMessageStream(stream);
            }
        }

        public void Send(Dictionary<string, object> dict) {
            stream.SendMessage(NSDictionary.FromObjectsAndKeys(dict.Values.AsEnumerable().ToArray(), dict.Keys.AsEnumerable().ToArray()));
        }
    }
}

