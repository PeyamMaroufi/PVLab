using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace PVLab
{
    public partial class PlotRealTime : Form
    {
        private readonly LineSeries _lineSeries1 = new LineSeries();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly LinearAxis _xAxis = new LinearAxis();
        private readonly LinearAxis _yAxis = new LinearAxis();

        private IPlotController _controller;
        private bool _haveNewPoints;
        private long _lastUpdateMilliseconds;
        private int _xMax;
        private int _yMax;

        public PlotModel PlotModel { get; private set; }

        public PlotRealTime()
        {
            InitializeComponent();
        }

        public void initPlot()
        {
            createPlotModel();
            addXAxis();
            addYAxis();

        }

        public void addLineSeries1()
        {
            _lineSeries1.MarkerType = MarkerType.Circle;
            _lineSeries1.StrokeThickness = 1;
            _lineSeries1.MarkerSize = 3;
            _lineSeries1.Title = "Start";
            _lineSeries1.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    PlotModel.Subtitle = "Index of nearest point in LineSeries: " + Math.Round(e.HitTestResult.Index);
                    PlotModel.InvalidatePlot(false);
                }
            };
            PlotModel.Series.Add(_lineSeries1);
        }

        public void addYAxis()
        {
            _yAxis.Minimum = 0;
            _yAxis.Title = "Y axis";
            _yAxis.MaximumPadding = 1;
            _yAxis.MinimumPadding = 1;
            _yAxis.MajorGridlineStyle = LineStyle.Solid;
            _yAxis.MinorGridlineStyle = LineStyle.Dot;
            PlotModel.Axes.Add(_yAxis);
        }

        public void addXAxis()
        {
            _xAxis.Minimum = 0;
            _xAxis.MaximumPadding = 1;
            _xAxis.MinimumPadding = 1;
            _xAxis.Position = AxisPosition.Bottom;
            _xAxis.Title = "Time";
            _xAxis.MajorGridlineStyle = LineStyle.Solid;
            _xAxis.MinorGridlineStyle = LineStyle.Dot;
            PlotModel.Axes.Add(_xAxis);
        }

        public void StopWatch()
        {
            _stopwatch.Start();
        }


        public void createPlotModel()
        {

            PlotModel = new PlotModel
            {
                Title = "Voltage Level",
                Subtitle = "Pan (right click and drag)/Zoom (Middle click and drag)/Reset (double-click)"
            };
            PlotModel.MouseDown += (sender, args) =>
            {
                if (args.ChangedButton == OxyMouseButton.Left && args.ClickCount == 2)
                {
                    foreach (var axis in PlotModel.Axes)
                        axis.Reset();

                    PlotModel.InvalidatePlot(false);
                }
            };
        }



        public void addPoints(int sample, int sampleTime)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, args) =>
            {
                var x = sampleTime;
                updateXMax(x);
                var y = sample;
                updateYMax(y);

                _lineSeries1.Points.Add(new DataPoint(x, y));

                _haveNewPoints = true;
            };
            timer.Start();

        }

        private void updateXMax(int x)
        {
            if (x > _xMax)
            {
                _xMax = x;
            }
        }

        private void updateYMax(int y)
        {
            if (y > _yMax)
            {
                _yMax = y;
            }
        }

        public void updatePlot()
        {
            CompositionTarget.Rendering += (sender, args) =>
            {
                if (_stopwatch.ElapsedMilliseconds > _lastUpdateMilliseconds + 2000 && _haveNewPoints)
                {
                    if (_yMax > 0 && _xMax > 0)
                    {
                        _yAxis.Maximum = _yMax + 3;
                        _xAxis.Maximum = _xMax + 1;
                    }

                    PlotModel.InvalidatePlot(false);

                    _haveNewPoints = false;
                    _lastUpdateMilliseconds = _stopwatch.ElapsedMilliseconds;
                }
            };
        }
    }
}
