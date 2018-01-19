using System;
using System.Diagnostics;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PVLab
{
    public partial class plot : Form
    {

        public plot()
        {
            InitializeComponent();

        }

        public int[] Samples { get; set; }
        public int[] SampleTime { get; set; }
        int MinimumY;
        int MaximumY;
        public void Draw(int range)
        {
            var myModel = new PlotModel { Title = "Show voltage" };
            myModel.PlotType = PlotType.Cartesian;
            myModel.Background = OxyColor.FromRgb(255, 255, 255);
            myModel.TextColor = OxyColor.FromRgb(0, 0, 0);
            this.plot1.Model = myModel;

            // Creat line series
            var s = new LineSeries { Title = "Voltage", StrokeThickness = 1 };
            for (int i = 0; i < Samples.Length; i++)
            {
                s.Points.Add(new DataPoint(SampleTime[i], Samples[i]));
            }

            switch (range)
            {
                case 0:
                    MinimumY = -20; MaximumY = 20;
                    break;
                case 1:
                    MinimumY = -30; MaximumY = 30;
                    break;
                case 2:
                    MinimumY = -60; MaximumY = 60;
                    break;
                case 3:
                    MinimumY = -110; MaximumY = 110;
                    break;
                case 4:
                    MinimumY = -210; MaximumY = 210;
                    break;
                case 5:
                    MinimumY = -550; MaximumY = 550;
                    break;
                case 6:
                    MinimumY = -1050; MaximumY = 1050;
                    break;
                case 7:
                    MinimumY = -2050; MaximumY = 2050;
                    break;
                case 8:
                    MinimumY = -5050; MaximumY = 5050;
                    break;
                case 9:
                    MinimumY = -11000; MaximumY = 11000;
                    break;
                case 10:
                    MinimumY = -21000; MaximumY = 21000;
                    break;
                case 11:
                    MinimumY = -51000; MaximumY = 51000;
                    break;
            }

            // Add and plot the plot
            myModel.Series.Add(s);
            myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0.0, Maximum = SampleTime[SampleTime.Length - 1] + 1000, AbsoluteMinimum = 0 });
            myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = MinimumY, Maximum = MaximumY, AbsoluteMaximum = MaximumY, AbsoluteMinimum = MinimumY });
        }
        //, AbsoluteMaximum= MaximumY, AbsoluteMinimum= MinimumY

    }
}
