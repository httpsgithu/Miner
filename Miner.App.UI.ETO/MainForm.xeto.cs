using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using HD;
using System.Windows.Threading;
using System.ComponentModel;
using HardlyMiningUI.ViewModels;
using HardlyMiningUI.Views;
namespace HardlyMiningUI
{
    public class MainForm : Form
    {
        protected GenericStackControl StatsBoxes;
        public MainForm()
        {
            XamlReader.Load(this);

            StatsBoxes.ControlType = typeof(HardlyMiningUI.Views.StatsBox); //tells the GenericStackControl what type of control should be used, so it can create controls for each of the items in the set datacontext (datacontex has been set for this in the xaml to the statsboxlist)
            DataContext = new MainVM();

            MinerController controller = new MinerController(null);

        }

        protected void HandleClickMe(object sender, EventArgs e)
        {
            //MinerSettings settingsView = new MinerSettings();
            
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}
