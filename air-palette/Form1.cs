using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace air_palette
{
    public partial class Form1 : Form
    {
        const int SIZE = 400;
        const int SCALE = 4;
        public Form1()
        {
            InitializeComponent();
            this.Width = this.Height = SIZE;

            var timer = new Timer();
            timer.Tick += (s, e) =>
            {
                var p = Utility.GetColorUnderMouse();
                this.Text = p.ToHexString();
                Utility.Magnify(CreateGraphics(), SIZE, SCALE);
            };
            timer.Interval = 50;
            timer.Start();
        }
    }
}
