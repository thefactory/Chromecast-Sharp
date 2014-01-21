using System;

namespace Google.Chromecast.iOS {
    public enum GCKSessionState {
        NotStarted = 0,
        Starting = 1,
        Started = 2,
        Ending = 3
    }

    public enum GCKPlayerState {
        Unknown = -1,
        Idle = 0,
        Stopped = 1,
        Playing = 2
    }

    public enum GCKMediaTrackType {
        Unknown = 0,
        Subtitles = 1,
        Captions = 2,
        Audio = 3,
        Video = 4
    }
}

