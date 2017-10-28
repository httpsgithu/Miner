using System;

namespace HD
{
  [Serializable]
  public class MethodLogin
  {
    public string method;
    public MethodLoginParams @params;
    /// <summary>
    /// Command ID.
    /// Starts at 1 (not 0).
    /// </summary>
    public int id;

    public MethodLogin(
      MethodLoginParams methodParams)
    {
      this.method = "login";
      this.id = 1; // TODO this should count
      this.@params = methodParams;
    }
  }

  [Serializable]
  public class MethodLoginParams
  {
    public string login;
    public string pass;
    public string agent;

    public MethodLoginParams(
      string login,
      string pass,
      string agent)
    {
      this.login = login;
      this.pass = pass;
      this.agent = agent;
    }
  }
}
