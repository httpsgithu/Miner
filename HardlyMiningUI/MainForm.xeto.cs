using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using HD;
using System.Windows.Threading;
using System.ComponentModel;

namespace HardlyMiningUI
{
    public class MainForm : Form
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        public MainForm()
        {
            XamlReader.Load(this);


            DataContext = new MainViewModel();
            /*
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += OnTick;
            timer.Start();

            maxNumberOfThreads.Content = Environment.ProcessorCount;
            sliderNumberOfThreads.Maximum = Environment.ProcessorCount;
            if (Miner.isFirstLaunch)
            {
                ((MainViewModel)DataContext).shouldStartWithWindows = true;
            }
            sliderPercentToHD.Value = 0.2;
            */
        }

        protected void HandleClickMe(object sender, EventArgs e)
        {
            MessageBox.Show("I was clicked!");
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}
