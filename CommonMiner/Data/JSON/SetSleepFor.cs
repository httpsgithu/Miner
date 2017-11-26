using System;

namespace HD
{
  public class SetSleepFor : IMessage
  {
    public long sleepForInNanoseconds;

    public SetSleepFor(
      long sleepForInNanoseconds)
    {
      this.sleepForInNanoseconds = sleepForInNanoseconds;
    }
  }
}
