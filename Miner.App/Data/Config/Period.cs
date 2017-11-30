using System;

namespace HD
{
  public enum Period
  {
    Hourly, Daily, Weekly, Monthly, Yearly, Decennially
  }

  public static class PeriodExtensions
  {
    public static decimal DailyToPeriod(
      this Period period,
      decimal dailyAmount)
    {
      decimal multiple;
      switch (period)
      {
        case Period.Hourly:
          multiple = 1 / 24m;
          break;
        case Period.Daily:
          multiple = 1;
          break;
        case Period.Weekly:
          multiple = 7;
          break;
        case Period.Monthly:
          multiple = 30;
          break;
        case Period.Yearly:
          multiple = 365.2422m; // From  GraysonP: I got it off of a nasa.gov pdf: https://pumas.gsfc.nasa.gov/files/04_21_97_1.pdf
          break;
        case Period.Decennially:
          multiple = 3652.422m;
          break;
        default:
          multiple = 0;
          break;
      }

      return dailyAmount * multiple;
    }
  }
}
