using System;
using System.Collections.Generic;
using System.Net;

namespace HD
{
  /// <summary>
  /// Async downloads with a 3 second throttle in between each request to the same domain.
  /// </summary>
  public abstract class NetworkAPI
  {
    #region Constants
    const string networkLog = "NiceHashNetwork.log";

    // Ideally we could configure this for each domain somehow
    static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(3);
    #endregion

    #region Data
    /// <summary>
    /// If we support more than just NiceHash APIs, we'll need another throttle mechanism.
    /// </summary>
    static readonly Dictionary<string, Throttle> throttlePerDomain 
      = new Dictionary<string, Throttle>();

    readonly WebClient webClient = new WebClient();

    readonly Uri uri;
    #endregion

    #region Properties
    Throttle throttle
    {
      get
      { 
        if(throttlePerDomain.TryGetValue(uri.Host, out Throttle value) == false)
        {
          value = new Throttle(defaultTimeout);
          throttlePerDomain.Add(uri.Host, value);
        }

        return value;
      }
    }
    #endregion

    #region Init
    public NetworkAPI(
      Uri uri)
    {
      Debug.Assert(uri != null);

      this.uri = uri;
      webClient.DownloadStringCompleted += OnDownloadComplete;
    }

    public void Close()
    {
      try
      {
        webClient.Dispose();
      }
      catch { }
    }
    #endregion

    #region Events
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
      Debug.Assert(string.IsNullOrWhiteSpace(content) == false);

      OnDownloadComplete(content);
    }

    protected abstract void OnDownloadComplete(
      string content);
    #endregion

    #region Public Write
    public virtual void ReadWhenReady(
      bool skipCooldownCheck = false)
    {
      if (skipCooldownCheck == false)
      {
        throttle.SleepIfNeeded();
      }

      throttle.SetLastUpdateTime();

      Log.ToFile(networkLog, uri.ToString());

      webClient.DownloadStringAsync(uri);
    }
    #endregion
  }
}
