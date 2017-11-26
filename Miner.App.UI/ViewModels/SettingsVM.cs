using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD;
namespace HardlyMiningUI.ViewModels
{
    public class SettingsVM : ViewModel
    {
        private bool _autoMineWhenIdle;
        public bool AutoMineWhenIdle
        {
            get { return _autoMineWhenIdle; }
            set { _autoMineWhenIdle = value; OnPropertyChanged(); }
        }

        private bool _startWithWindows;
        public bool StartWithWindows
        {
            get { return _startWithWindows; }
            set { _startWithWindows = value; OnPropertyChanged(); }
        }

        private double _timeToEnterIdle;
        public double TimeToEnterIdle
        {
            get { return _timeToEnterIdle; }
            set { _timeToEnterIdle = value; OnPropertyChanged(); }
        }

        private int _threadsUsed;
        public int ThreadsUsed
        {
            get { return _threadsUsed; }
            set { _threadsUsed = value; OnPropertyChanged(); }
        }

        private int _maxThreads = Environment.ProcessorCount;
        public int ThreadsMaxAvailible
        {
            get { return _maxThreads; }
        }

        private int _threadsCurrentlyUsed;
        public int ThreadsCurrentlyUsed
        {
            get { return _threadsCurrentlyUsed; }
            set { _threadsCurrentlyUsed = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _walletID;
        public string WalletID
        {
            get { return _walletID; }
            set { _walletID = value; OnPropertyChanged(); }
        }

        private byte _percentTimeToHD;
        public byte PercentTimeToHD
        {
            get { return _percentTimeToHD; }
            set { _percentTimeToHD = value; OnPropertyChanged(); }
        }

    }
}
