using System;
using System.Collections.Generic;

namespace HD
{
  public class BeneficiarySorter : IComparer<Beneficiary>
  {
    public int Compare(Beneficiary x, Beneficiary y)
    {
      return y.percentTime.CompareTo(x.percentTime);
    }
  }
}
