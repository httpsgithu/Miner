using System;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace HD.Views
{
    public class StatsBox : Panel
    {
        public StatsBox()
        {
            XamlReader.Load(this);
        }
    }
}
