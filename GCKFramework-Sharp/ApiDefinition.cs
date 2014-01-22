using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using System.Text;

namespace Google.Chromecast.iOS {
    [BaseType(typeof(NSObject))]
    public partial interface GCKDeviceManager {
        [Export("devices", ArgumentSemantic.Copy)]
        GCKDevice [] Devices { get; }

        [Export("hasDiscoveredDevices")]
        bool HasDiscoveredDevices { get; }

        [Export("context", ArgumentSemantic.Retain)]
        GCKContext Context { get; }

        [Export("scanning")]
        bool Scanning { get; }

        [Export("initWithContext:")]
        IntPtr Constructor(GCKContext context);

        [Export("startScan")]
        void StartScan();

        [Export("stopScan")]
        void StopScan();

        [Export("addListener:")]
        void AddListener(GCKDeviceManagerListener listener);

        [Export("removeListener:")]
        void RemoveListener(GCKDeviceManagerListener listener);
    }

    [Model, BaseType(typeof(NSObject))]
    public partial interface GCKDeviceManagerListener {
        [Export("scanStarted")]
        void ScanStarted();

        [Export("scanStopped")]
        void ScanStopped();

        [Export("deviceDidComeOnline:")]
        void DeviceDidComeOnline(GCKDevice device);

        [Export("deviceDidGoOffline:")]
        void DeviceDidGoOffline(GCKDevice device);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKApplicationSupportFilterListener : GCKDeviceManagerListener {
        [Export("initWithContext:applicationName:protocols:downstreamListener:")]
        IntPtr Constructor(GCKContext context, string applicationName, NSObject[] protocolNamespaces, GCKDeviceManagerListener listener);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKApplicationChannel {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKApplicationChannelDelegate Delegate { get; set; }

        [Export("sendBufferAvailableBytes")]
        int SendBufferAvailableBytes { get; }

        [Export("sendBufferPendingBytes")]
        int SendBufferPendingBytes { get; }

        [Export("initWithBufferSize:pingInterval:")]
        IntPtr Constructor(uint bufferSize, double pingInterval);

        [Export("connectTo:")]
        void ConnectTo(NSUrl url);

        [Export("disconnect")]
        void Disconnect();

        [Export("attachMessageStream:")]
        bool AttachMessageStream(GCKMessageStream stream);

        [Export("detachMessageStream:")]
        bool DetachMessageStream(GCKMessageStream stream);

        [Export("detachAllMessageStreams")]
        void DetachAllMessageStreams();
    }

    [Model, BaseType(typeof(NSObject))]
    public partial interface GCKApplicationChannelDelegate {
        [Export("applicationChannelDidConnect:")]
        void DidConnect(GCKApplicationChannel channel);

        [Export("applicationChannel:connectionDidFailWithError:")]
        void ConnectionDidFailWithError(GCKApplicationChannel channel, GCKError error);

        [Export("applicationChannel:didDisconnectWithError:")]
        void DidDisconnectWithError(GCKApplicationChannel channel, GCKError error);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKApplicationMetadata {
        [Export("name", ArgumentSemantic.Copy)]
        string Name { get; }

        [Export("title", ArgumentSemantic.Copy)]
        string Title { get; }

        [Export("iconURL", ArgumentSemantic.Copy)]
        NSUrl IconURL { get; }

        [Export("initWithName:title:iconURL:supportedProtocols:")]
        IntPtr Constructor(string name, string title, NSUrl iconURL, NSObject[] supportedProtocolNamespaces);

        [Export("doesSupportProtocol:")]
        bool DoesSupportProtocol(string protocolNamespace);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKApplicationSession : GCKApplicationChannelDelegate {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKApplicationSessionDelegate Delegate { get; set; }

        [Export("applicationInfo", ArgumentSemantic.Retain)]
        GCKApplicationMetadata ApplicationInfo { get; }

        [Export("channel", ArgumentSemantic.Retain)]
        GCKApplicationChannel Channel { get; }

        [Export("hasStarted")]
        bool HasStarted { get; }

        [Export("sessionState")]
        GCKSessionState SessionState { get; }

        [Export("bufferSize")]
        uint BufferSize { get; }

        [Export("stopApplicationWhenSessionEnds")]
        bool StopApplicationWhenSessionEnds { get; set; }

        [Export("initWithContext:device:bufferSize:")]
        IntPtr Constructor(GCKContext context, GCKDevice device, uint bufferSize);

        [Export("initWithContext:device:")]
        IntPtr Constructor(GCKContext context, GCKDevice device);

        [Export("startSession")]
        bool StartSession();

        [Export("startSessionWithApplication:")]
        bool StartSessionWithApplication(string applicationName);

        [Export("startSessionWithApplication:argument:")]
        bool StartSessionWithApplication(string applicationName, GCKMimeData argument);

        [Export("endSession")]
        bool EndSession();

        [Export("endSessionInBackgroundTask")]
        bool EndSessionInBackgroundTask();

        [Export("resumeSession")]
        bool ResumeSession();

        [Export("delegate", ArgumentSemantic.Assign)]
        [NullAllowed]
        GCKApplicationSessionDelegate WeakDelegate { get; set; }
    }

    [BaseType(typeof(NSObject))]
    [Model]
    public partial interface GCKApplicationSessionDelegate {
        [Export("applicationSessionDidStart")]
        void ApplicationSessionDidStart();

        [Export("applicationSessionDidFailToStartWithError:")]
        void ApplicationSessionDidFailToStart(GCKApplicationSessionError error);

        [Export("applicationSessionDidEndWithError:")]
        void ApplicationSessionDidEnd(GCKApplicationSessionError error);

        [Field("kGCKApplicationSessionErrorDomain", "__Internal")]
        NSString GCKApplicationSessionErrorDomain { get; }
    }

    [BaseType(typeof(NSError))]
    public partial interface GCKError {
        [Export("initWithDomain:code:additionalUserInfo:")]
        IntPtr Constructor(string domain, int code, NSDictionary additionalUserInfo);

        [Static, Export("localizedDescriptionForCode:")]
        string LocalizedDescriptionForCode(int code);

        [Field("kGCKApplicationSessionErrorCodeFailedToStartApplication", "__Internal")]
        int GCKApplicationSessionErrorCodeFailedToStartApplication { get; }

        [Field("kGCKApplicationSessionErrorCodeFailedToQueryApplication", "__Internal")]
        int GCKApplicationSessionErrorCodeFailedToQueryApplication { get; }

        [Field("kGCKApplicationSessionErrorCodeApplicationStopped", "__Internal")]
        int GCKApplicationSessionErrorCodeApplicationStopped { get; }

        [Field("kGCKApplicationSessionErrorCodeFailedToCreateChannel", "__Internal")]
        int GCKApplicationSessionErrorCodeFailedToCreateChannel { get; }

        [Field("kGCKApplicationSessionErrorCodeFailedToConnectChannel", "__Internal")]
        int GCKApplicationSessionErrorCodeFailedToConnectChannel { get; }

        [Field("kGCKApplicationSessionErrorCodeChannelDisconnected", "__Internal")]
        int GCKApplicationSessionErrorCodeChannelDisconnected { get; }

        [Field("kGCKApplicationSessionErrorCodeFailedToStopApplication", "__Internal")]
        int GCKApplicationSessionErrorCodeFailedToStopApplication { get; }

        [Field("kGCKApplicationSessionErrorProtocol", "__Internal")]
        int GCKApplicationSessionErrorProtocol { get; }

        [Field("kGCKApplicationSessionErrorTimeout", "__Internal")]
        int GCKApplicationSessionErrorTimeout { get; }

        [Field("kGCKApplicationSessionErrorAlreadyConnected", "__Internal")]
        int GCKApplicationSessionErrorAlreadyConnected { get; }

        [Field("kGCKApplicationSessionErrorCodeUnknownError", "__Internal")]
        int GCKApplicationSessionErrorCodeUnknownError { get; }
    }

    [BaseType(typeof(GCKError))]
    public partial interface GCKApplicationSessionError {
        [Export("initWithCode:additionalUserInfo:")]
        IntPtr Constructor(int code, NSDictionary userInfo);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKContentMetadata {
        [Export("title", ArgumentSemantic.Copy)]
        string Title { get; set; }

        [Export("imageURL", ArgumentSemantic.Copy)]
        NSUrl ImageURL { get; set; }

        [Export("contentInfo", ArgumentSemantic.Copy)]
        NSDictionary ContentInfo { get; set; }

        [Export("initWithTitle:imageURL:contentInfo:")]
        IntPtr Constructor(string title, NSUrl imageURL, NSDictionary contentInfo);

        [Field("kGCKVersion", "__Internal")]
        NSString GCKVersion { get; }
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKContext {
        [Export("userAgent", ArgumentSemantic.Copy)]
        string UserAgent { get; }

        [Export("initWithUserAgent:")]
        IntPtr Constructor(string userAgent);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKDevice {
        [Export("ipAddress", ArgumentSemantic.Copy)]
        string IpAddress { get; }

        [Export("deviceID", ArgumentSemantic.Copy)]
        string DeviceID { get; set; }

        [Export("friendlyName", ArgumentSemantic.Copy)]
        string FriendlyName { get; set; }

        [Export("manufacturer", ArgumentSemantic.Copy)]
        string Manufacturer { get; set; }

        [Export("modelName", ArgumentSemantic.Copy)]
        string ModelName { get; set; }

        [Export("applicationURL", ArgumentSemantic.Copy)]
        NSUrl ApplicationURL { get; set; }

        [Export("icons", ArgumentSemantic.Copy)]
        GCKDeviceIcon [] Icons { get; set; }

        [Export("initWithIPAddress:")]
        IntPtr Constructor(string ipAddress);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKDeviceIcon {
        [Export("width")]
        uint Width { get; }

        [Export("height")]
        uint Height { get; }

        [Export("depth")]
        uint Depth { get; }

        [Export("url", ArgumentSemantic.Copy)]
        NSUrl Url { get; }

        [Export("initWithWidth:height:depth:url:")]
        IntPtr Constructor(uint width, uint height, uint depth, NSUrl url);

        [Field("kGCKNetworkRequestErrorDomain", "__Internal")]
        NSString GCKNetworkRequestErrorDomain { get; }

        [Field("kGCKNetworkRequestErrorCodeOK", "__Internal")]
        int GCKNetworkRequestErrorCodeOK { get; }

        [Field("kGCKNetworkRequestErrorCodeIOError", "__Internal")]
        int GCKNetworkRequestErrorCodeIOError { get; }

        [Field("kGCKNetworkRequestErrorCodeTimeout", "__Internal")]
        int GCKNetworkRequestErrorCodeTimeout { get; }

        [Field("kGCKNetworkRequestErrorCodeInvalidResponse", "__Internal")]
        int GCKNetworkRequestErrorCodeInvalidResponse { get; }

        [Field("kGCKNetworkRequestErrorCodeNotFound", "__Internal")]
        int GCKNetworkRequestErrorCodeNotFound { get; }

        [Field("kGCKNetworkRequestErrorCodeAccessDenied", "__Internal")]
        int GCKNetworkRequestErrorCodeAccessDenied { get; }

        [Field("kGCKNetworkRequestErrorCodeBusy", "__Internal")]
        int GCKNetworkRequestErrorCodeBusy { get; }

        [Field("kGCKNetworkRequestErrorCodeNotSupported", "__Internal")]
        int GCKNetworkRequestErrorCodeNotSupported { get; }

        [Field("kGCKNetworkRequestErrorCodeCancelled", "__Internal")]
        int GCKNetworkRequestErrorCodeCancelled { get; }
    }

    [BaseType(typeof(GCKError))]
    public partial interface GCKNetworkRequestError {
        [Export("initWithCode:additionalUserInfo:")]
        IntPtr Constructor(int code, NSDictionary userInfo);
    }

    [Model, BaseType(typeof(NSObject))]
    public partial interface GCKNetworkRequestDelegate {
        [Export("networkRequestDidComplete:")]
        void DidComplete(GCKNetworkRequest request);

        [Export("networkRequest:didFailWithError:")]
        void DidFailWithError(GCKNetworkRequest request, GCKNetworkRequestError error);

        [Export("networkRequestWasCancelled:")]
        void WasCancelled(GCKNetworkRequest request);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKNetworkRequest {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKNetworkRequestDelegate Delegate { get; set; }

        [Export("responseEncoding")]
        NSStringEncoding ResponseEncoding { get; set; }

        [Export("initWithContext:")]
        IntPtr Constructor(GCKContext context);

        [Export("execute")]
        void Execute();

        [Export("cancel")]
        void Cancel();

        [Export("performHTTPGet:timeout:")]
        void PerformHTTPGet(NSUrl url, double timeout);

        [Export("performHTTPPost:data:timeout:")]
        void PerformHTTPPost(NSUrl url, GCKMimeData data, double timeout);

        [Export("performHTTPDelete:timeout:")]
        void PerformHTTPDelete(NSUrl url, double timeout);

        [Export("processResponseWithStatus:finalURL:headers:data:")]
        int ProcessResponseWithStatus(int status, NSUrl finalURL, NSDictionary headers, GCKMimeData data);

        [Export("parseJson:")]
        NSObject ParseJson(string json);

        [Export("writeJson:")]
        string WriteJson(NSObject jsonObject);

        [Export("didComplete")]
        void DidComplete();

        [Export("didFailWithError:")]
        void DidFailWithError(GCKError error);
    }

    [BaseType(typeof(GCKNetworkRequest))]
    public partial interface GCKFetchImageRequest {
        [Export("image", ArgumentSemantic.Retain)]
        UIImage Image { get; }

        [Export("initWithContext:url:preferredWidth:preferredHeight:")]
        IntPtr Constructor(GCKContext context, NSUrl url, uint width, uint height);

        [Export("initWithContext:url:")]
        IntPtr Constructor(GCKContext context, NSUrl url);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKLogger {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKLoggerDelegate Delegate { get; set; }

        [Static, Export("sharedInstance")]
        GCKLogger SharedInstance();
    }

    [BaseType(typeof(NSObject))]
    [Model]
    public partial interface GCKLoggerDelegate {
        [Export("logFromFunction:message:")]
        void Message(IntPtr functionName, string message);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKMessageSink {
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKMessageStream {
        [Export("protocolNamespace", ArgumentSemantic.Copy)]
        string ProtocolNamespace { get; }

        [Export("messageSink", ArgumentSemantic.Retain)]
        GCKMessageSink MessageSink { get; set; }

        [Export("initWithNamespace:")]
        IntPtr Constructor(string protocolNamespace);

        [Export("didAttach")]
        void DidAttach();

        [Export("didDetach")]
        void DidDetach();

        [Export("didReceiveMessage:")]
        void DidReceiveMessage(NSObject message);

        [Export("sendMessage:")]
        bool SendMessage(NSObject message);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKMediaProtocolCommand {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKMediaProtocolCommandDelegate Delegate { get; set; }

        [Export("sequenceNumber")]
        uint SequenceNumber { get; }

        [Export("type", ArgumentSemantic.Copy)]
        string Type { get; }

        [Export("cancelled")]
        bool Cancelled { get; }

        [Export("completed")]
        bool Completed { get; }

        [Export("hasError")]
        bool HasError { get; set; }

        [Export("errorDomain", ArgumentSemantic.Copy)]
        string ErrorDomain { get; set; }

        [Export("errorCode")]
        int ErrorCode { get; set; }

        [Export("errorInfo", ArgumentSemantic.Copy)]
        NSDictionary ErrorInfo { get; set; }

        [Export("initWithSequenceNumber:type:")]
        IntPtr Constructor(uint sequenceNumber, string type);

        [Export("complete")]
        void Complete();

        [Export("cancel")]
        void Cancel();
    }

    [Model, BaseType(typeof(NSObject))]
    public partial interface GCKMediaProtocolCommandDelegate {
        [Export("mediaProtocolCommandDidComplete:")]
        void Completed(GCKMediaProtocolCommand command);

        [Export("mediaProtocolCommandWasCancelled:")]
        void Cancelled(GCKMediaProtocolCommand command);
    }

    [BaseType(typeof(GCKMessageStream))]
    public partial interface GCKMediaProtocolMessageStream {
        [Export("delegate", ArgumentSemantic.Assign)]
        GCKMediaProtocolMessageStreamDelegate Delegate { get; set; }

        [Export("contentID", ArgumentSemantic.Copy)]
        string ContentID { get; }

        [Export("contentInfo", ArgumentSemantic.Copy)]
        NSDictionary ContentInfo { get; }

        [Export("playerState")]
        GCKPlayerState PlayerState { get; }

        [Export("streamPosition")]
        double StreamPosition { get; }

        [Export("streamDuration")]
        double StreamDuration { get; }

        [Export("mediaTitle", ArgumentSemantic.Copy)]
        string MediaTitle { get; }

        [Export("mediaImageURL", ArgumentSemantic.Copy)]
        NSUrl MediaImageURL { get; }

        [Export("volume")]
        double Volume { get; }

        [Export("muted")]
        bool Muted { get; }

        [Export("mediaTracks", ArgumentSemantic.Copy)]
        NSMutableArray MediaTracks { get; }

        [Export("loadMediaWithContentID:contentMetadata:")]
        GCKMediaProtocolCommand LoadMediaWithContentID(string contentID, GCKContentMetadata metadata);

        [Export("loadMediaWithContentID:contentMetadata:autoplay:")]
        GCKMediaProtocolCommand LoadMediaWithContentID(string contentID, GCKContentMetadata metadata, bool autoplay);

        [Export("resumeStream")]
        GCKMediaProtocolCommand ResumeStream();

        [Export("playStream")]
        GCKMediaProtocolCommand PlayStream();

        [Export("playStreamFrom:")]
        GCKMediaProtocolCommand PlayStreamFrom(double position);

        [Export("stopStream")]
        bool StopStream();

        [Export("setStreamVolume:")]
        GCKMediaProtocolCommand SetStreamVolume(double volume);

        [Export("setStreamMuted:")]
        GCKMediaProtocolCommand SetStreamMuted(bool muted);

        [Export("selectTracksToEnable:andDisable:")]
        GCKMediaProtocolCommand SelectTracksToEnable(GCKMediaTrack[] tracksToEnable, GCKMediaTrack[] tracksToDisable);

        [Export("requestStatus")]
        GCKMediaProtocolCommand RequestStatus();

        [Export("sendKeyResponseForRequestID:withTokens:")]
        bool SendKeyResponseForRequestID(uint requestID, string[] tokens);

        [Export("cancelCommand:")]
        bool CancelCommand(GCKMediaProtocolCommand command);

        [Export("keyRequestWasReceivedWithRequestID:method:requests:")]
        void KeyRequestWasReceivedWithRequestID(int requestID, string method, string[] requests);
    }

    [Model, BaseType(typeof(NSObject))]
    public partial interface GCKMediaProtocolMessageStreamDelegate {
        [Export("mediaProtocolMessageStreamDidReceiveStatusUpdate:")]
        void StatusUpdate(GCKMediaProtocolMessageStream stream);

        [Export("mediaProtocolMessageStreamDidUpdateTrackList:")]
        void UpdatedTrackList(GCKMediaProtocolMessageStream stream);

        [Export("mediaProtocolMessageStream:didReceiveErrorWithDomain:code:errorInfo:")]
        void Error(GCKMediaProtocolMessageStream stream, string domain, int code, NSDictionary errorInfo);
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKMediaTrack {
        [Export("initWithIdentifier:type:name:languageCode:enabled:")]
        IntPtr Constructor(int identifier, GCKMediaTrackType type, string name, string languageCode, bool enabled);

        [Export("identifier")]
        int Identifier { get; }

        [Export("type")]
        GCKMediaTrackType Type { get; }

        [Export("name", ArgumentSemantic.Copy)]
        string Name { get; }

        [Export("languageCode", ArgumentSemantic.Copy)]
        string LanguageCode { get; }

        [Export("enabled")]
        bool Enabled { get; set; }

        [Field("kGCKMimeBinary", "__Internal")]
        NSString GCKMimeBinary { get; }

        [Field("kGCKMimeForm", "__Internal")]
        NSString GCKMimeForm { get; }

        [Field("kGCKMimeHTML", "__Internal")]
        NSString GCKMimeHTML { get; }

        [Field("kGCKMimeJSON", "__Internal")]
        NSString GCKMimeJSON { get; }

        [Field("kGCKMimeText", "__Internal")]
        NSString GCKMimeText { get; }

        [Field("kGCKMimeURL", "__Internal")]
        NSString GCKMimeURL { get; }

        [Field("kGCKMimeXML", "__Internal")]
        NSString GCKMimeXML { get; }
    }

    [BaseType(typeof(NSObject))]
    public partial interface GCKMimeData {
        [Field("kGCKNetworkRequestDefaultTimeout", "__Internal")]
        double GCKNetworkRequestDefaultTimeout { get; }

        [Field("kGCKNetworkRequestHTTPOriginValue", "__Internal")]
        NSString GCKNetworkRequestHTTPOriginValue { get; }

        [Field("kGCKApplicationSessionMinBufferSize", "__Internal")]
        int GCKApplicationSessionMinBufferSize { get; }

        [Field("kGCKApplicationSessionDefaultBufferSize", "__Internal")]
        int GCKApplicationSessionDefaultBufferSize { get; }

        [Field("kGCKHTTPStatusOK", "__Internal")]
        int GCKHTTPStatusOK { get; }

        [Field("kGCKHTTPStatusCreated", "__Internal")]
        int GCKHTTPStatusCreated { get; }

        [Field("kGCKHTTPStatusNoContent", "__Internal")]
        int GCKHTTPStatusNoContent { get; }

        [Field("kGCKHTTPStatusForbidden", "__Internal")]
        int GCKHTTPStatusForbidden { get; }

        [Field("kGCKHTTPStatusNotFound", "__Internal")]
        int GCKHTTPStatusNotFound { get; }

        [Field("kGCKHTTPStatusNotImplemented", "__Internal")]
        int GCKHTTPStatusNotImplemented { get; }

        [Field("kGCKHTTPStatusServiceUnavailable", "__Internal")]
        int GCKHTTPStatusServiceUnavailable { get; }

        [Field("kGCKHTTPHeaderLocation", "__Internal")]
        NSString GCKHTTPHeaderLocation { get; }

        [Export("type", ArgumentSemantic.Copy)]
        string Type { get; }

        [Export("data", ArgumentSemantic.Copy)]
        NSData Data { get; }

        [Export("textData", ArgumentSemantic.Copy)]
        string TextData { get; }

        [Export("initWithData:type:")]
        IntPtr Constructor(NSData data, string type);

        [Export("initWithTextData:type:")]
        IntPtr Constructor(string data, string type);

        [Static, Export("mimeDataWithJsonObject:")]
        GCKMimeData MimeDataWithJsonObject(NSObject json);

        [Static, Export("mimeDataWithURL:")]
        GCKMimeData MimeDataWithURL(NSUrl url);
    }
}
