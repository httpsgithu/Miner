using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HD
{
  public partial class StatsBox : UserControl
  {
        string content1, content2;
        bool firstclick = true;

        public StatsBox()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (null != label_buffer.Content)
            {
                if (firstclick)
                {
                    content1 = label.Content.ToString();
                    content2 = label_buffer.Content.ToString();
                    firstclick = false;
                }
                if (label.Content.ToString() == content1)
                {
                    label.Content = content2;
                }
                else
                {
                    label.Content = content1;
                }
            }
        }
    }
}
