// Copyright (c) 2018 Payam 
// Made by Payam M. 
// Purpose: Measuring the voltage of high voltage devices scaled down to almost 30 V
// Workes for continues streaming and triggers. Includes repeat trigger and single trigger. and capturing.
// Target framwork is .NET and WINFORM. 
// 

using PS5000AImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVLab
{
    public static class StaticVariable
    {
        public static ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        public static string S { get; set; }
        public static short _handle { get; set; }
        public static int CheckRbs { get; set; } = 0;
        public static bool RunRepeat { get; set; }
        public static bool RunSingle { get; set; }
        public static bool RunStuffOrNot { get; set; }
        public static bool ISaidRun { get; set; }
        public static double TriggerAt { get; set; }
        public static double Error { get; set; }
        public static int Index { get; set; }
        public static int RaisingFallingIndex { get; set; }
        public static bool CheckBoxStatus { get; set; }
        public static Imports.Channel SelChannel { get; set; }
        public static Imports.Range SelVolt { get; set; }
        public static Imports.Coupling SelCoup { get; set; }
        public static Imports.DeviceResolution resolution { get; set; }
        public static double SampleInterval { get; set; }
        public static double NumberOfSamples { get; set; }
        public static int SelChannelIndex { get; set; }
        public static int SelRangeIndex { get; set; }
        public static string SelectedDiv { get; set; }
        public static double TimeBase { get; set; }
        public static double _maxValue { get; set; }
        public static string XAxis { get; set; }
        public static double DivValue { get; set; }
        public static double DivValueLimit { get; set; }
        public static Plot form;
        public static double[] voltages = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        public static ChannelSettings[] _channelSettings { get; set; }

    }
}
