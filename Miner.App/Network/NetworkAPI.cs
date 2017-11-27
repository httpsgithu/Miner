using System;
using System.Net;

namespace HD
{
  /// <summary>
  /// TODO need a static throttle.  No more than 1 request every 3 seconds.
  /// </summary>
  public abstract class NetworkAPI
  {
    #region Data
    readonly WebClient webClient = new WebClient();

    readonly Uri uri;

    readonly Throttle throttle;
    #endregion

    #region Init
    public NetworkAPI(
      Uri uri,
      TimeSpan minTimeBetweenRequests)
    {
      this.uri = uri;
      throttle = new Throttle(minTimeBetweenRequests);
      webClient.DownloadStringCompleted += OnDownloadComplete;
    }

    public void Close()
    {
      webClient.Dispose();
    }
    #endregion

    void OnDownloadComplete(
      object sender,
      DownloadStringCompletedEventArgs e)
    {
      throttle.SetLastUpdateTime();
      if (e.Cancelled)
      {
        Log.NetworkError(nameof(NetworkAPI), nameof(OnDownloadComplete), e.Error);
        return;
      }

      string content = e.Result;
      OnDownloadComplete(content);
    }

    protected abstract void OnDownloadComplete(
      string content);

    public virtual void ReadWhenReady(
      bool skipCooldownCheck = false)
    {
      if (skipCooldownCheck)
      {
        throttle.SetLastUpdateTime();
      }
      else
      {
        throttle.SleepIfNeeded();
      }
      webClient.DownloadStringAsync(uri);
    }
  }
}
