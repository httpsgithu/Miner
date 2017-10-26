using System;
using System.Collections.Generic;
using System.Net;

namespace HD
{
  public abstract class NetworkAPI
  {
    #region Data
    readonly WebClient webClient = new WebClient();

    readonly TimeSpan minTimeBetweenRequests;

    readonly Uri uri;

    DateTime timeOfLastRequest;
    #endregion

    #region Init
    public NetworkAPI(
      Uri uri,
      TimeSpan minTimeBetweenRequests)
    {
      this.uri = uri;
      this.minTimeBetweenRequests = minTimeBetweenRequests;
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
      timeOfLastRequest = DateTime.Now;
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

    public virtual void BeginRead(
      bool skipCooldownCheck = false)
    {
      if (skipCooldownCheck == false 
        && (DateTime.Now - timeOfLastRequest) < minTimeBetweenRequests
        || webClient.IsBusy)
      { // Too soon
        return;
      }
      timeOfLastRequest = DateTime.Now;

      webClient.DownloadStringAsync(uri);
    }
  }
}
