mergeInto(LibraryManager.library, {
  ResumeAudioContext: function () {
    if (typeof WEBAudio !== "undefined" &&
        WEBAudio.audioContext &&
        WEBAudio.audioContext.state !== "running") {
      WEBAudio.audioContext.resume();
    }
  }
});