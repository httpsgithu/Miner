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
        readonly DispatcherTimer timer = new DispatcherTimer();
        public MainForm()
        {
            XamlReader.Load(this);


            DataContext = new MainVM();

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
