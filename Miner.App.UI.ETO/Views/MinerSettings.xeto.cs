using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using HD;
namespace HD.Views //Eto views require the namespace to match the directory. 
{
    public class MinerSettings : Panel
    {
        protected TextBox WalletTextBox;
        public MinerSettings()
        {
            XamlReader.Load(this);
            //WalletTextBox.TextBinding.BindDataContext(Binding.Property((SettingsViewModel m) => m.myWallet).CatchException(ex => OnWalletException())); //eto 2.4 feature. 

        }

        bool OnWalletException()
        {
          WalletTextBox.TextColor = Color.Parse("Red");
          return true;
        }

         
    }
}
