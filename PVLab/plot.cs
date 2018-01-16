using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace PVLab
{
    public partial class plot : Form
    {
        public plot()
        {
            InitializeComponent();

        }

        public int[] Samples { get; set; }

        public void Draw()
        {
            liveChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "voltage",
                    Values = new ChartValues<int>(Samples)
                }
            };
        }
    }
}
