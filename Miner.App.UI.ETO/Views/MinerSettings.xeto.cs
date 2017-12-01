using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace HD.Views //Eto views require the namespace to match the directory. 
{
    public class MinerSettings : Panel
    {
        public MinerSettings()
        {
            XamlReader.Load(this);
        }
    }
}
