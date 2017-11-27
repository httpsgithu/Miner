using System;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace HD
{
    public class StatsBox : Panel
    {
        public StatsBox()
        {
            XamlReader.Load(this);
        }
    }
}
