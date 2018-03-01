using PS5000AImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVLab
{
    public class TimeBaseCalc
    {
        public static double timebase;
        public static double redo;
                public static double _timebase;

        public TimeBaseCalc()
        {

        }
        public static double TimeBase(double SampleInterval)
        {
            switch (StaticVariable.resolution)
            {
                case Imports.DeviceResolution.PS5000A_DR_8BIT:

                    if (SampleInterval >= 1 * Math.Pow(10, -9) && SampleInterval <= 4 * Math.Pow(10, -9))
                    {
                        redo = (Math.Log(SampleInterval * Math.Pow(10, 9)) / (Math.Log(2)));
                        _timebase = (uint)(redo);
                    }
                    else
                    {
                        redo = 125 * Math.Pow(10, 6) * SampleInterval;
                        _timebase = (uint)(redo + 2);

                    }

                    break;
                case Imports.DeviceResolution.PS5000A_DR_12BIT:
                    if (SampleInterval >= 2 * Math.Pow(10, -9) && SampleInterval <= 8 * Math.Pow(10, -9))
                    {
                        redo = (Math.Log(SampleInterval * 5 * Math.Pow(10, 8)) / (Math.Log(2)));
                        _timebase = (uint)(redo + 1);
                    }
                    else
                    {
                        redo = SampleInterval * 62500000;
                        _timebase = (uint)(redo + 3);
                    }
                    break;
                case Imports.DeviceResolution.PS5000A_DR_14BIT:
                    redo = SampleInterval * 125000000;
                    _timebase = (uint)(redo + 2);
                    break;
                case Imports.DeviceResolution.PS5000A_DR_15BIT:
                    redo = SampleInterval * 125000000;
                    _timebase = (uint)(redo + 2);
                    break;
                case Imports.DeviceResolution.PS5000A_DR_16BIT:
                    redo = SampleInterval * 62500000;
                    _timebase = (uint)(redo + 3);
                    break;

            }

            return _timebase;
        }
    }
}
