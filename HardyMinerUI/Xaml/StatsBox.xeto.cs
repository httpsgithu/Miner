using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace HardyMinerUI.Xaml
{
    public class StatsBox : Panel
    {
        public StatsBox()
        {
            XamlReader.Load(this);
        }
    }
}
