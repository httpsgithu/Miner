using System;
using Eto.Forms;
using Eto.Drawing;

namespace EtoUI
{
    /// <summary>
    /// A custom panel
    /// </summary>
    public class MyPanel : Panel
    {
        public MyPanel()
        {
            // set your content and other properties here
            Content = new Label { Text = "Some Content" };
        }
    }
}
