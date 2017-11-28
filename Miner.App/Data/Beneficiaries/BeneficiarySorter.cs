using System;
using System.Collections.Generic;

namespace HD
{
  /// <summary>
  /// Sort by percent time, desc.
  /// </summary>
  public class BeneficiarySorter : IComparer<Beneficiary>
  {
    public int Compare(
      Beneficiary x, 
      Beneficiary y)
    {
      return y.percentTime.CompareTo(x.percentTime);
    }
  }
}
