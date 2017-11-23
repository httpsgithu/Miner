using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace HardlyMiningUI.Views
{
    public class MinerSettings : Panel
    {
        public MinerSettings()
        {
            XamlReader.Load(this);
        }
    }
}
