Chromecast-Sharp
================

C# wrapper for Google Chromecast libraries (currently using version 1.0.9) and a basic example.

To use the example app, do the following:

1. On your test server (the machine that will serve up your Receiver page), claim a static IP (can be an internal address, just not localhost) or a DNS entry. You'll be giving this to Google during the Chromecast registration process.
2. Register your Chromecast for whitelisting and get an Application ID for your IP/DNS from step #1 + the path to rcvr/receiver.html (for example http://172.16.1.102/rcvr/receiver.html)
3. Serve the rcvr/receiver.html page on port 80 ('sudo python -m SimpleHTTPServer 80' from this root directory)
4. Replace the occurences of YOUR_APPID_HERE with the Application ID that you got when reigstering your Chromecast in step 2.
5. Run the app and see the magic.
