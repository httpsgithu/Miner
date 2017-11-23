using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD;
using System.Windows.Input;

namespace HardlyMiningUI.ViewModels
{
    class MainVM : MainViewModel
    {
        public SettingsVM Settings { get; } = new SettingsVM();

        private string _btnActionText = "Start";
        public string BtnActionText
        {
            get { return _btnActionText; }
            set { _btnActionText = value; OnPropertyChanged(); }
        }

        public MainVM()
        {

        }


        private ICommand _startStopCMD;
        public ICommand StartStopCMD
        {
            get
            {
                return _startStopCMD ?? (_startStopCMD = new CommandHandler(OnStartStopBtnClick, true));
            }
        }

        private void OnStartStopBtnClick()
        {
            if (Miner.instance.isMinerRunning)
            {
                Stop();
            }
            else
            {
                Start(true);
            }
        }

        void Start(
        bool wasManuallyStarted)
        {
            Miner.instance.Start(wasManuallyStarted);
            UpdateRunningState();
        }

        void Stop()
        {
            Miner.instance.Stop();
            UpdateRunningState();
        }

        void UpdateRunningState()
        {
            if (Miner.instance.isMinerRunning)
            {
                BtnActionText = "Stop";
            }
            else
            {
                BtnActionText = "Start";
            }
        }



    }
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
