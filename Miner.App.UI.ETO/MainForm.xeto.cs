using System;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace HD
{
    public class MainForm : Form
    {
        protected GenericStackControl StatsBoxes;

        public MainForm()
        {
            XamlReader.Load(this);

            StatsBoxes.ControlType = typeof(StatsBox); //tells the GenericStackControl what type of control should be used, so it can create controls for each of the items in the set datacontext (datacontex has been set for this in the xaml to the statsboxlist)
            DataContext = new MainViewModel();
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
