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
        const int SCALE = 16;
        public Form1()
        {
            InitializeComponent();

            var timer = new Timer();
            timer.Tick += (s, e) =>
            {
                var p = Utility.GetColorUnderMouse();
                Utility.Magnify(panel1.CreateGraphics(), SIZE, SCALE);
                panel2.CreateGraphics().SetColor(p, panel2.Width, panel2.Height);
                this.label1.Text = p.ToHexString();
                this.label2.Text = p.ToHLSString();
                this.label3.Text = p.ToCMYKString();
            };
            timer.Interval = 50;
            timer.Start();
        }
    }
}
