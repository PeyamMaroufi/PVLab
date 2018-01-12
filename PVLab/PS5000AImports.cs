/**************************************************************************
 * 
 * Filename: PS5000AImports.cs
 *
 * Description:
 *   This file contains .NET wrapper calls needed to support the 
 *   PicoScope 5000 series flexible resolution oscilloscopes using 
 *   the ps5000a driver API functions. It also has the enums and structs 
 *   required by the (wrapped) function calls.
 *   
 *  Copyright (C) 2013 - 2017 Pico Technology Ltd. See LICENSE file for terms.  
 *   
 **************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS5000AImports
{
    class Imports
    {
        #region constants
        // Drivers name ps5000a for pico 5000a series
        private const string _DRIVER_FILENAME = "ps5000a.dll";

        #endregion

        #region Driver enums

        /// <summary>
        /// Channels name.
        /// </summary>

        public enum Channel : uint
        {
            ChannelA,
            ChannelB,
            ChannelC,
            ChannelD,
            External,
            Aux,
            None
        }

        /// <summary>
        /// Inputs range in term of voltage
        /// </summary>

        public enum Range : uint
        {
            Range_10mV,
            Range_20mV,
            Range_50mV,
            Range_100mV,
            Range_200mV,
            Range_500mV,
            Range_1V,
            Range_2V,
            Range_5V,
            Range_10V,
            Range_20V,
            Range_MAX_RANGE
        }

        /// <summary>
        /// Time line units
        /// </summary>
		public enum ReportedTimeUnits : uint
        {
            FemtoSeconds,
            PicoSeconds,
            NanoSeconds,
            MicroSeconds,
            MilliSeconds,
            Seconds,
        }

        /// <summary>
        /// 
        /// </summary>

        public enum ThresholdMode : uint
        {
            Level,
            Window
        }

        public enum ThresholdDirection : uint
        {
            // Values for level threshold mode
            //
            Above,
            Below,
            Rising,
            Falling,
            RisingOrFalling,

            // Values for window threshold mode
            //
            Inside = Above,
            Outside = Below,
            Enter = Rising,
            Exit = Falling,
            EnterOrExit = RisingOrFalling,
            PositiveRunt = 9,
            NegativeRunt,

            None = Rising,
        }

        public enum DownSamplingMode : uint
        {
            None,
            Aggregate
        }

        public enum PulseWidthType : uint
        {
            None,
            LessThan,
            GreaterThan,
            InRange,
            OutOfRange
        }

        public enum TriggerState : uint
        {
            DontCare,
            True,
            False,
        }

        public enum RatioMode : uint
        {
            None = 0,
            Aggregate = 1,
            Average = 4,
            Decimate = 2
        }

        public enum DeviceResolution : uint
        {
            PS5000A_DR_8BIT,
            PS5000A_DR_12BIT,
            PS5000A_DR_14BIT,
            PS5000A_DR_15BIT,
            PS5000A_DR_16BIT
        }

        public enum Coupling : uint
        {
            PS5000A_AC,
            PS5000A_DC
        }

        public enum SweepType : int
        {
            PS5000A_UP,
            PS5000A_DOWN,
            PS5000A_UPDOWN,
            PS5000A_DOWNUP,
            PS5000A_MAX_SWEEP_TYPES
        }

        public enum WaveType : int
        {
            PS5000A_SINE,
            PS5000A_SQUARE,
            PS5000A_TRIANGLE,
            PS5000A_RAMP_UP,
            PS5000A_RAMP_DOWN,
            PS5000A_SINC,
            PS5000A_GAUSSIAN,
            PS5000A_HALF_SINE,
            PS5000A_DC_VOLTAGE,
            PS5000A_WHITE_NOISE,
            PS5000A_MAX_WAVE_TYPES
        }

        public enum ExtraOperations : int
        {
            PS5000A_ES_OFF,
            PS5000A_WHITENOISE,
            PS5000A_PRBS // Pseudo-Random Bit Stream
        }

        public enum SigGenTrigType : int
        {
            PS5000A_SIGGEN_RISING,
            PS5000A_SIGGEN_FALLING,
            PS5000A_SIGGEN_GATE_HIGH,
            PS5000A_SIGGEN_GATE_LOW
        }

        public enum SigGenTrigSource : int
        {
            PS5000A_SIGGEN_NONE,
            PS5000A_SIGGEN_SCOPE_TRIG,
            PS5000A_SIGGEN_AUX_IN,
            PS5000A_SIGGEN_EXT_IN,
            PS5000A_SIGGEN_SOFT_TRIG
        }

        public enum IndexMode : int
        {
            PS5000A_SINGLE,
            PS5000A_DUAL,
            PS5000A_QUAD,
            PS5000A_MAX_INDEX_MODES
        }

        public enum BandwidthLimiter : uint
        {
            S5000A_BW_FULL,
            PS5000A_BW_20MHZ
        }

        #endregion

        # region structs

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerChannelProperties
        {
            public short ThresholdMajor;
            public ushort HysteresisMajor;
            public short ThresholdMinor;
            public ushort HysteresisMinor;
            public Channel Channel;
            public ThresholdMode ThresholdMode;


            public TriggerChannelProperties(
                short thresholdMajor,
                ushort hysteresisMajor,
                short thresholdMinor,
                ushort hysteresisMinor,
                Channel channel,
                ThresholdMode thresholdMode)
            {
                this.ThresholdMajor = thresholdMajor;
                this.HysteresisMajor = hysteresisMajor;
                this.ThresholdMinor = thresholdMinor;
                this.HysteresisMinor = hysteresisMinor;
                this.Channel = channel;
                this.ThresholdMode = thresholdMode;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerConditions
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState External;
            public TriggerState Aux;
            public TriggerState Pwq;

            public TriggerConditions(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState external,
                TriggerState aux,
                TriggerState pwq)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.External = external;
                this.Aux = aux;
                this.Pwq = pwq;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerConditionsV2
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState External;
            public TriggerState Aux;
            public TriggerState Pwq;
            public TriggerState Digital;

            public TriggerConditionsV2(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState external,
                TriggerState aux,
                TriggerState pwq,
                TriggerState digital)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.External = external;
                this.Aux = aux;
                this.Pwq = pwq;
                this.Digital = digital;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PwqConditions
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState External;
            public TriggerState Aux;

            public PwqConditions(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState external,
                TriggerState aux)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.External = external;
                this.Aux = aux;
            }
        }

        #endregion

        #region Driver Imports
        #region Callback delegates
        public delegate void ps5000aBlockReady(short handle, short status, IntPtr pVoid);

        public delegate void ps5000aStreamingReady(
                                                short handle,
                                                int noOfSamples,
                                                uint startIndex,
                                                short ov,
                                                uint triggerAt,
                                                short triggered,
                                                short autoStop,
                                                IntPtr pVoid);

        public delegate void ps5000DataReady(
                                                short handle,
                                                short status,
                                                int noOfSamples,
                                                short overflow,
                                                IntPtr pVoid);
        #endregion


        /// <summary>
        ///  Opens the driver
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="serial"></param>
        /// <param name="resolution"></param>
        /// <returns>        
        /// PICO_OK
        /// PICO_OS_NOT_SUPPORTED
        /// PICO_INVALID_DEVICE_RESOLUTION.
        /// PICO_OPEN_OPERATION_IN_PROGRESS
        /// PICO_EEPROM_CORRUPT
        /// PICO_KERNEL_DRIVER_TOO_OLD
        /// PICO_FPGA_FAIL
        /// PICO_MEMORY_CLOCK_FREQUENCY
        /// PICO_FW_FAIL
        /// PICO_MAX_UNITS_OPENED
        /// PICO_NOT_FOUND (if the specified unit was not found)
        /// PICO_NOT_RESPONDING
        /// PICO_MEMORY_FAIL
        /// PICO_ANALOG_BOARD
        /// PICO_CONFIG_FAIL_AWG
        /// PICO_INITIALISE_FPGA
        /// PICO_POWER_SUPPLY_NOT_CONNECTED</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aOpenUnit")]
        public static extern uint OpenUnit(out short handle, StringBuilder serial, DeviceResolution resolution);


        /// <summary>
        /// sets the resolution a specified device will run
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="resolution"></param>
        /// <returns>
        /// resolution, determines the resolution of the device when opened,
        /// the available values are one of the PS5000A_DEVICE_RESOLUTION.
        /// If resolution is out of range the device will return
        /// PICO_INVALID_DEVICE_RESOLUTION.
        /// PICO_OK
        /// PICO_INVALID_DEVICE_RESOLUTION
        /// PICO_OS_NOT_SUPPORTED
        /// PICO_OPEN_OPERATION_IN_PROGRESS
        /// PICO_EEPROM_CORRUPT
        /// PICO_KERNEL_DRIVER_TOO_OLD
        /// PICO_FPGA_FAIL
        /// PICO_MEMORY_CLOCK_FREQUENCY
        /// PICO_FW_FAIL
        /// PICO_MAX_UNITS_OPENED
        /// PICO_NOT_FOUND(if the specified unit was not found)
        /// PICO_NOT_RESPONDING
        /// PICO_MEMORY_FAIL
        /// PICO_ANALOG_BOARD
        /// PICO_CONFIG_FAIL_AWG
        /// PICO_INITIALISE_FPGA
        /// PICO_POWER_SUPPLY_NOT_CONNECTED</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDeviceResolution")]
        public static extern uint SetDeviceResolution(short handle, DeviceResolution resolution);

        /// <summary>
        /// retrieves the resolution specified device will ru
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="resolution"></param>
        /// <returns>PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION 
        /// PICO_NULL_PARAMETER</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetDeviceResolution")]
        public static extern uint GetDeviceResolution(short handle, out DeviceResolution resolution);


        /// <summary>
        /// close a scope device
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>PICO_OK
        /// PICO_HANDLE_INVALID
        /// PICO_USER_CALLBACK
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aCloseUnit")]
        public static extern uint CloseUnit(short handle);

        /// <summary>
        /// start block mode
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="noOfPreTriggerSamples"></param>
        /// <param name="noOfPostTriggerSamples"></param>
        /// <param name="timebase"></param>
        /// <param name="timeIndisposedMs"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="lpps5000aBlockReady"></param>
        /// <param name="pVoid"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_BUFFERS_NOT_SET(in Overlapped mode)
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_SEGMENT_OUT_OF_RANGE
        /// PICO_INVALID_CHANNEL
        /// PICO_INVALID_TRIGGER_CHANNEL
        /// PICO_INVALID_CONDITION_CHANNEL
        /// PICO_TOO_MANY_SAMPLES
        /// PICO_INVALID_TIMEBASE
        /// PICO_NOT_RESPONDING
        /// PICO_CONFIG_FAIL
        /// PICO_INVALID_PARAMETER
        /// PICO_NOT_RESPONDING
        /// PICO_TRIGGER_ERROR
        /// PICO_DRIVER_FUNCTION
        /// PICO_FW_FAIL
        /// PICO_NOT_ENOUGH_SEGMENTS(in Bulk mode)
        /// PICO_PULSE_WIDTH_QUALIFIER
        /// PICO_SEGMENT_OUT_OF_RANGE(in Overlapped mode)
        /// PICO_STARTINDEX_INVALID(in Overlapped mode)
        /// PICO_INVALID_SAMPLERATIO(in Overlapped mode)
        /// PICO_CONFIG_FAIL</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aRunBlock")]
        public static extern uint RunBlock(
                                                short handle,
                                                int noOfPreTriggerSamples,
                                                int noOfPostTriggerSamples,
                                                uint timebase,
                                                out int timeIndisposedMs,
                                                uint segmentIndex,
                                                ps5000aBlockReady lpps5000aBlockReady,
                                                IntPtr pVoid);

        /// <summary>
        /// This function stops the scope device from sampling data.
        /// When running the device in streaming mode, you should always call this function after
        /// the end of a capture to ensure that the scope is ready for the next capture.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aStop")]
        public static extern uint Stop(short handle);

        /// <summary>
        /// Setting the channels.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channel"></param>
        /// <param name="enabled"></param>
        /// <param name="dc"></param>
        /// <param name="range"></param>
        /// <param name="analogueOffset"></param>
        /// <returns>PICO_OK
        /// PICO_USER_CALLBACK
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_CHANNEL
        /// PICO_INVALID_VOLTAGE_RANGE
        /// PICO_INVALID_COUPLING
        /// PICO_INVALID_ANALOGUE_OFFSET
        /// PICO_DRIVER_FUNCTION</returns>

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetChannel")]
        public static extern uint SetChannel(
                                                short handle,
                                                Channel channel,
                                                short enabled,
                                                Coupling dc,
                                                Range range,
                                                float analogueOffset);
        /// <summary>
        /// This function returns a status code and outputs the maximum ADC count value to a
        /// parameter.The output value depends on the currently selected resolution.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="value"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_USER_CALLBACK
        /// PICO_INVALID_HANDLE
        /// PICO_TOO_MANY_SEGMENTS
        /// PICO_MEMORY
        /// PICO_DRIVER_FUNCTION</returns>

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aMaximumValue")]
        public static extern uint MaximumValue(
                                         short handle,
                                         out short value);

        /// <summary>
        /// handle, the handle of the required device
        /// channel, the channel you want to use with the buffer.Use one of
        /// these values:
        /// PS5000A_CHANNEL_A
        /// PS5000A_CHANNEL_B
        /// PS5000A_CHANNEL_C
        /// PS5000A_CHANNEL_D
        /// * buffer, the location of the buffer
        /// bufferLth, the size of the buffer array
        /// segmentIndex, the number of the memory segment to be used
        /// mode, the downsampling mode.See ps5000aGetValues for the
        /// available modes, but note that a single call to
        /// ps5000aSetDataBuffer can only associate one buffer with one
        /// downsampling mode.If you intend to call ps5000aGetValues with
        /// more than one downsampling mode activated, then you must call
        /// ps5000aSetDataBuffer several times to associate a separate
        /// buffer with each downsampling mode.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channel"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferLth"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="ratioMode"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_CHANNEL
        /// PICO_RATIO_MODE_NOT_SUPPORTED
        /// PICO_SEGMENT_OUT_OF_RANGE
        /// PICO_DRIVER_FUNCTION
        /// PICO_INVALID_PARAMETER</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDataBuffer")]
        public static extern uint SetDataBuffer(
                                                    short handle,
                                                    Channel channel,
                                                    short[] buffer,
                                                    int bufferLth,
                                                    uint segmentIndex,
                                                    RatioMode ratioMode);
        /// <summary>
        /// This function tells the driver the location of one or two buffers for receiving data. You
        /// need to allocate memory for the buffers before calling this function.If you do not need
        /// two buffers, because you are not using aggregate mode, then you can optionally use
        /// ps5000aSetDataBuffer instead. 
        /// 
        /// 
        /// handle, the handle of the required device.
        /// channel, the channel for which you want to set the buffers.Use
        /// one of these constants:
        /// PS5000A_CHANNEL_A
        /// PS5000A_CHANNEL_B
        /// PS5000A_CHANNEL_C
        /// PS5000A_CHANNEL_D
        /// * bufferMax, a buffer to receive the maximum data values in
        /// aggregation mode, or the non-aggregated values otherwise.
        /// * bufferMin, a buffer to receive the minimum aggregated data
        /// values.Not used in other downsampling modes.
        /// bufferLth, the size of the bufferMax and bufferMin arrays.
        /// segmentIndex, the number of the memory segment to be used
        /// mode: see ps5000aGetValues
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channel"></param>
        /// <param name="bufferMax"></param>
        /// <param name="bufferMin"></param>
        /// <param name="bufferLth"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="ratioMode"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_CHANNEL
        /// PICO_RATIO_MODE_NOT_SUPPORTED
        /// PICO_SEGMENT_OUT_OF_RANGE
        /// PICO_DRIVER_FUNCTION
        /// PICO_INVALID_PARAMETER</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDataBuffers")]
        public static extern uint SetDataBuffers(
                                                    short handle,
                                                    Channel channel,
                                                    short[] bufferMax,
                                                    short[] bufferMin,
                                                    int bufferLth,
                                                    uint segmentIndex,
                                                    RatioMode ratioMode);

        /// <summary>
        /// handle, the handle of the required device
        /// channelA, channelB, channelC, channelD, ext, the
        /// direction in which the signal must pass through the threshold to
        /// activate the trigger.See the table below for allowable values. If using
        /// a level trigger in conjunction with a pulse-width trigger, see the
        /// description of the direction argument to
        /// ps5000aSetPulseWidthQualifier for more information.
        /// aux: not used
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channelA"></param>
        /// <param name="channelB"></param>
        /// <param name="channelC"></param>
        /// <param name="channelD"></param>
        /// <param name="ext"></param>
        /// <param name="aux"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_INVALID_PARAMETER
        /// </returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelDirections")]
        public static extern uint SetTriggerChannelDirections(
                                                                short handle,
                                                                ThresholdDirection channelA,
                                                                ThresholdDirection channelB,
                                                                ThresholdDirection channelC,
                                                                ThresholdDirection channelD,
                                                                ThresholdDirection ext,
                                                                ThresholdDirection aux);

        /// <summary>
        /// Gets the timebase of the measuring
        /// </summary>
        /// </arguments>handle, the handle of the required device.
        /// sampleInterval, on entry, the requested time interval between
        /// samples; on exit, the actual time interval used.
        /// sampleIntervalTimeUnits, the unit of time used for
        /// sampleInterval.Use one of these values:
        /// PS5000A_FS PS5000A_PS
        /// PS5000A_NS PS5000A_US
        /// PS5000A_MS PS5000A_S
        /// maxPreTriggerSamples, the maximum number of raw samples
        /// before a trigger event for each enabled channel. If no trigger
        /// condition is set this argument is ignored.
        /// maxPostTriggerSamples, the maximum number of raw samples
        /// after a trigger event for each enabled channel. If no trigger condition
        /// is set, this argument states the maximum number of samples to be
        /// stored.
        /// autoStop, a flag that specifies if the streaming should stop when
        /// all of maxSamples have been captured.
        /// downSampleRatio,
        /// downSampleRatioMode: see ps5000aGetValues
        /// overviewBufferSize, the size of the overview buffers. These are
        /// temporary buffers used for storing the data before returning it to the
        /// application.The size is the same as the bufferLth value passed to
        /// ps5000aSetDataBuffer.</arguments>
        /// 
        /// <param name="handle"></param>
        /// <param name="timebase"></param>
        /// <param name="noSamples"></param>
        /// <param name="timeIntervalNanoseconds"></param>
        /// <param name="maxSamples"></param>
        /// <param name="segmentIndex"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_ETS_MODE_SET
        /// PICO_USER_CALLBACK
        /// PICO_NULL_PARAMETER
        /// PICO_INVALID_PARAMETER
        /// PICO_STREAMING_FAILED
        /// PICO_NOT_RESPONDING
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_TRIGGER_ERROR
        /// PICO_INVALID_SAMPLE_INTERVAL
        /// PICO_INVALID_BUFFER
        /// PICO_DRIVER_FUNCTION
        /// PICO_FW_FAIL
        /// PICO_MEMORY</returns>

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetTimebase")]
        public static extern uint GetTimebase(
                                                 short handle,
                                                 uint timebase,
                                                 int noSamples,
                                                 out int timeIntervalNanoseconds,
                                                 out int maxSamples,
                                                 uint segmentIndex);

        /// <summary>
        /// Get the values of measuring
        /// handle, the handle of the required device.
        /// startIndex, a zero-based index that indicates the start point for
        /// data collection.It is measured in sample intervals from the start of
        /// the buffer.
        /// * noOfSamples, on entry, the number of samples required.On
        /// exit, the actual number retrieved. The number of samples retrieved
        /// will not be more than the number requested, and the data retrieved
        /// starts at startIndex.
        /// downSampleRatio, the downsampling factor that will be applied to
        /// the raw data.
        /// downSampleRatioMode, which downsampling mode to use.The
        /// available values are:
        /// PS5000A_RATIO_MODE_NONE (downSampleRatio is ignored)
        /// PS5000A_RATIO_MODE_AGGREGATE
        /// PS5000A_RATIO_MODE_AVERAGE
        /// PS5000A_RATIO_MODE_DECIMATE
        /// AGGREGATE, AVERAGE, DECIMATE are single-bit constants that can
        /// be ORed to apply multiple downsampling modes to the same data.
        /// segmentIndex, the zero-based number of the memory segment
        /// where the data is stored.
        /// * overflow, on exit, a set of flags that indicate whether an
        /// overvoltage has occurred on any of the channels.It is a bit field with
        /// bit 0 denoting Channel A.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="startIndex"></param>
        /// <param name="noOfSamples"></param>
        /// <param name="downSampleRatio"></param>
        /// <param name="downSampleRatioMode"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="overflow"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_NO_SAMPLES_AVAILABLE
        /// PICO_DEVICE_SAMPLING
        /// PICO_NULL_PARAMETER
        /// PICO_SEGMENT_OUT_OF_RANGE
        /// PICO_STARTINDEX_INVALID
        /// PICO_ETS_NOT_RUNNING
        /// PICO_BUFFERS_NOT_SET
        /// PICO_INVALID_PARAMETER
        /// PICO_TOO_MANY_SAMPLES
        /// PICO_DATA_NOT_AVAILABLE
        /// PICO_STARTINDEX_INVALID
        /// PICO_INVALID_SAMPLERATIO
        /// PICO_INVALID_CALL
        /// PICO_NOT_RESPONDING
        /// PICO_MEMORY
        /// PICO_RATIO_MODE_NOT_SUPPORTED
        /// PICO_DRIVER_FUNCTION</returns>

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetValues")]
        public static extern uint GetValues(
                                                short handle,
                                                uint startIndex,
                                                ref uint noOfSamples,
                                                uint downSampleRatio,
                                                DownSamplingMode downSampleRatioMode,
                                                uint segmentIndex,
                                                out short overflow);
        /// <summary>
        /// handle, the handle of the required device
        /// * conditions, an array of PS5000A_PWQ_CONDITIONS structures
        /// specifying the conditions that should be applied to each channel.In
        /// the simplest case, the array consists of a single element.When there
        /// are several elements, the overall trigger condition is the logical OR of
        /// all the elements.If conditions is NULL then the pulse-width
        /// qualifier is not used.
        /// nConditions, the number of elements in the conditions array.
        /// If nConditions is zero then the pulse-width qualifier is not used.
        /// Range: 0 to PS5000A_MAX_PULSE_WIDTH_QUALIFIER_COUNT.
        /// direction, the direction of the signal required for the pulse width
        /// trigger to fire. See PS5000A_THRESHOLD_DIRECTION constants for
        /// the list of possible values.Each channel of the oscilloscope (except
        /// the EXT input) has two thresholds for each direction—for example,
        /// PS5000A_RISING and PS5000A_RISING_LOWER—so that one can be
        /// used for the pulse-width qualifier and the other for the level trigger.
        /// The driver will not let you use the same threshold for both triggers;
        ///         so, for example, you cannot use PS5000A_RISING as the
        ///         direction argument for both ps5000aSetTriggerConditions
        /// and ps5000aSetPulseWidthQualifier at the same time.There is
        /// no such restriction when using window triggers.
        /// lower, the lower limit of the pulse-width counter, in samples.
        /// upper, the upper limit of the pulse-width counter, in samples.This
        /// parameter is used only when the type is set to
        /// PS5000A_PW_TYPE_IN_RANGE or
        /// PS5000A_PW_TYPE_OUT_OF_RANGE.
        /// type, the pulse-width type, one of these constants:
        /// PS5000A_PW_TYPE_NONE: do not use the pulse width qualifier
        /// PS5000A_PW_TYPE_LESS_THAN: pulse width less than lower
        /// PS5000A_PW_TYPE_GREATER_THAN: pulse width greater than
        /// lower
        /// PS5000A_PW_TYPE_IN_RANGE: pulse width between lower and
        /// upper
        /// PS5000A_PW_TYPE_OUT_OF_RANGE: pulse width not between
        /// lower and upper
        ///  </summary>
        /// <param name="handle"></param>
        /// <param name="conditions"></param>
        /// <param name="nConditions"></param>
        /// <param name="direction"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <param name="type"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_CONDITIONS
        /// PICO_PULSE_WIDTH_QUALIFIER
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetPulseWidthQualifier")]
        public static extern uint SetPulseWidthQualifier(
                                                            short handle,
                                                            PwqConditions[] conditions,
                                                            short nConditions,
                                                            ThresholdDirection direction,
                                                            uint lower,
                                                            uint upper,
                                                            PulseWidthType type);

        /// <summary>
        /// handle, the handle of the required device.
        /// enable, zero to disable the trigger, any non-zero value to set the
        /// trigger.
        /// source, the channel on which to trigger.
        /// threshold, the ADC count at which the trigger will fire.
        /// direction, the direction in which the signal must move to cause a
        /// trigger.The following directions are supported: ABOVE, BELOW,
        /// RISING, FALLING and RISING_OR_FALLING.
        /// delay, the time between the trigger occurring and the first sample.
        /// For example, if delay= 100 then the scope would wait 100 sample
        /// periods before sampling. At a timebase of 500 MS/s, or 2 ns per
        /// sample, the total delay would then be 100 x 2 ns = 200 ns.Range: 0
        /// to MAX_DELAY_COUNT.
        /// autoTrigger_ms, the number of milliseconds the device will wait if
        /// no trigger occurs.If this is set to zero, the scope device will wait
        /// indefinitely for a trigger.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="enable"></param>
        /// <param name="channel"></param>
        /// <param name="threshold"></param>
        /// <param name="direction"></param>
        /// <param name="delay"></param>
        /// <param name="autoTriggerMs"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_CHANNEL
        /// PICO_INVALID_PARAMETER
        /// PICO_MEMORY
        /// PICO_CONDITIONS
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSimpleTrigger")]
        public static extern uint SetSimpleTrigger(
                                                        short handle,
                                                        short enable,
                                                        Channel channel,
                                                        short threshold,
                                                        ThresholdDirection direction,
                                                        uint delay,
                                                        short autoTriggerMs);

        /// <summary>
        /// handle, the handle of the required device.
        /// * channelProperties, a pointer to an array of
        /// PS5000A_TRIGGER_CHANNEL_PROPERTIES structures describing the
        /// requested properties.The array can contain a single element
        /// describing the properties of one channel, or a number of elements
        /// describing several channels. If NULL is passed, triggering is switched
        /// off.
        /// nChannelProperties, the size of the channelProperties array.
        /// If zero, triggering is switched off.
        /// auxOutputEnable: not used
        /// autoTriggerMilliseconds, the time in milliseconds for which the
        /// scope device will wait before collecting data if no trigger event occurs.
        /// If this is set to zero, the scope device will wait indefinitely for a
        /// trigger.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channelProperties"></param>
        /// <param name="nChannelProperties"></param>
        /// <param name="auxOutputEnable"></param>
        /// <param name="autoTriggerMilliseconds"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_TRIGGER_ERROR
        /// PICO_MEMORY
        /// PICO_INVALID_TRIGGER_PROPERTY
        /// PICO_DRIVER_FUNCTION
        /// PICO_INVALID_PARAMETER</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelProperties")]
        public static extern uint SetTriggerChannelProperties(
                                                                    short handle,
                                                                    TriggerChannelProperties[] channelProperties,
                                                                    short nChannelProperties,
                                                                    short auxOutputEnable,
                                                                    int autoTriggerMilliseconds);
        /// <summary>
        /// handle,
        ///  startIndex,
        /// * noOfSamples,
        /// downSampleRatio,
        /// downSampleRatioMode,
        /// segmentIndex: see ps5000aGetValues
        /// * overflow: see ps5000aGetValuesBulk
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="conditions"></param>
        /// <param name="nConditions"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_PARAMETER
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelConditions")]
        public static extern uint SetTriggerChannelConditions(
                                                                    short handle,
                                                                    TriggerConditions[] conditions,
                                                                    short nConditions);

        /// <summary>
        /// handle, the handle of the required device
        /// delay, the time between the trigger occurring and the first sample.
        /// For example, if delay = 100 then the scope would wait 100 sample
        /// periods before sampling. At a timebase of 500 MS/s, or 2 ns per
        /// sample, the total delay would then be:
        /// 100 x 2 ns = 200 ns
        /// Range: 0 to MAX_DELAY_COUNT
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="delay"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_USER_CALLBACK
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerDelay")]
        public static extern uint SetTriggerDelay(short handle, uint delay);

        /// <summary>
        /// handle, the handle of the device from which information is
        /// required.If an invalid handle is passed, only the driver versions can
        /// be read.
        /// * string, on exit, the unit information string selected specified by
        /// the info argument.If string is NULL, only requiredSize is
        /// returned.
        /// stringLength, the maximum number of 8-bit integers(int8_t)
        /// that may be written to string.
        /// * requiredSize, on exit, the required length of the string
        /// array.
        /// info, a number specifying what information is required.The
        /// possible values are listed in the table below.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="infoString"></param>
        /// <param name="stringLength"></param>
        /// <param name="requiredSize"></param>
        /// <param name="info"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_NULL_PARAMETER
        /// PICO_INVALID_INFO
        /// PICO_INFO_UNAVAILABLE
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetUnitInfo")]
        public static extern uint GetUnitInfo(
                                                    short handle,
                                                    StringBuilder infoString,
                                                    short stringLength,
                                                    out short requiredSize,
                                                    uint info);
        /// <summary>
        /// This function tells the oscilloscope to start collecting data in streaming mode. When
        /// data has been collected from the device it is downsampled if necessary and then
        /// delivered to the application.Call ps5000aGetStreamingLatestValues to retrieve
        /// the data.See Using streaming mode for a step-by-step guide to this process.
        /// When a trigger is set, the total number of samples stored in the driver is the sum of
        /// maxPreTriggerSamples and maxPostTriggerSamples. If autoStop is false then
        /// this will become the maximum number of samples without downsampling.
        ///
        /// handle, the handle of the required device.
        /// * sampleInterval, on entry, the requested time interval between
        /// samples; on exit, the actual time interval used.
        /// sampleIntervalTimeUnits, the unit of time used for
        /// sampleInterval.Use one of these values:
        /// PS5000A_FS PS5000A_PS
        /// PS5000A_NS PS5000A_US
        /// PS5000A_MS PS5000A_S
        /// maxPreTriggerSamples, the maximum number of raw samples
        /// before a trigger event for each enabled channel. If no trigger
        /// condition is set this argument is ignored.
        /// maxPostTriggerSamples, the maximum number of raw samples
        /// after a trigger event for each enabled channel. If no trigger condition
        /// is set, this argument states the maximum number of samples to be
        /// stored.
        /// autoStop, a flag that specifies if the streaming should stop when
        /// all of maxSamples have been captured.
        /// downSampleRatio,
        /// downSampleRatioMode: see ps5000aGetValues
        /// overviewBufferSize, the size of the overview buffers. These are
        /// temporary buffers used for storing the data before returning it to the
        /// application.The size is the same as the bufferLth value passed to
        /// ps5000aSetDataBuffer.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="sampleInterval"></param>
        /// <param name="sampleIntervalTimeUnits"></param>
        /// <param name="maxPreTriggerSamples"></param>
        /// <param name="maxPostPreTriggerSamples"></param>
        /// <param name="autoStop"></param>
        /// <param name="downSamplingRatio"></param>
        /// <param name="downSampleRatioMode"></param>
        /// <param name="overviewBufferSize"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_ETS_MODE_SET
        /// PICO_USER_CALLBACK
        /// PICO_NULL_PARAMETER
        /// PICO_INVALID_PARAMETER
        /// PICO_STREAMING_FAILED
        /// PICO_NOT_RESPONDING
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_TRIGGER_ERROR
        /// PICO_INVALID_SAMPLE_INTERVAL
        /// PICO_INVALID_BUFFER
        /// PICO_DRIVER_FUNCTION
        /// PICO_FW_FAIL
        /// PICO_MEMORY</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aRunStreaming")]
        public static extern uint RunStreaming(
                                                    short handle,
                                                    ref uint sampleInterval,
                                                    ReportedTimeUnits sampleIntervalTimeUnits,
                                                    uint maxPreTriggerSamples,
                                                    uint maxPostPreTriggerSamples,
                                                    short autoStop,
                                                    uint downSamplingRatio,
                                                    RatioMode downSampleRatioMode,
                                                    uint overviewBufferSize);
        /// <summary>
        /// handle, the handle of the required device.
        /// lpPs5000AReady, a pointer to your ps5000aStreamingReady
        /// callback function.
        /// * pParameter, a void pointer that will be passed to the
        /// ps5000aStreamingReady callback function.The callback function
        /// may optionally use this pointer to return information to the
        /// application.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="lpps5000aStreamingReady"></param>
        /// <param name="pVoid"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_INVALID_HANDLE
        /// PICO_NO_SAMPLES_AVAILABLE
        /// PICO_INVALID_CALL
        /// PICO_BUSY
        /// PICO_NOT_RESPONDING
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetStreamingLatestValues")]
        public static extern uint GetStreamingLatestValues(
                                                                short handle,
                                                                ps5000aStreamingReady lpps5000aStreamingReady,
                                                                IntPtr pVoid);
        /// <summary>
        /// handle, the handle of the device
        /// nCaptures, the number of waveforms to capture in one run
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="nCaptures"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_PARAMETER
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetNoOfCaptures")]
        public static extern uint SetNoOfRapidCaptures(
                                                        short handle,
                                                        uint nCaptures);
        /// <summary>
        /// handle, the value returned from opening the device.
        /// * maxsegments, (output) the maximum number of segments
        /// allowed.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="nSegments"></param>
        /// <param name="nMaxSamples"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION
        /// PICO_NULL_PARAMETER</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aMemorySegments")]
        public static extern uint MemorySegments(
                                                    short handle,
                                                    uint nSegments,
                                                    out int nMaxSamples);

        /// <summary>
        /// handle, the handle of the device
        /// * noOfSamples, on entry, the number of samples required; on
        /// exit, the actual number retrieved.The number of samples retrieved
        /// will not be more than the number requested.The data retrieved
        /// always starts with the first sample captured.
        /// fromSegmentIndex, the first segment from which the waveform
        /// should be retrieved
        /// toSegmentIndex, the last segment from which the waveform
        /// should be retrieved
        /// downSampleRatio,
        /// downSampleRatioMode: see ps5000aGetValues
        /// * overflow, an array of integers equal to or larger than the
        /// number of waveforms to be retrieved. Each segment index has a
        /// corresponding entry in the overflow array, with overflow[0]
        /// containing the flags for the segment numbered fromSegmentIndex
        /// and the last element in the array containing the flags for the segment
        /// numbered toSegmentIndex. Each element in the array is a bit field
        /// as described under ps5000aGetValues.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="noOfSamples"></param>
        /// <param name="fromSegmentIndex"></param>
        /// <param name="toSegmentIndex"></param>
        /// <param name="downSampleRatio"></param>
        /// <param name="downSampleRatioMode"></param>
        /// <param name="overflow"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_INVALID_HANDLE
        /// PICO_INVALID_PARAMETER
        /// PICO_INVALID_SAMPLERATIO
        /// PICO_ETS_NOT_RUNNING
        /// PICO_BUFFERS_NOT_SET
        /// PICO_TOO_MANY_SAMPLES
        /// PICO_SEGMENT_OUT_OF_RANGE
        /// PICO_NO_SAMPLES_AVAILABLE
        /// PICO_NOT_RESPONDING
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetValuesBulk")]
        public static extern uint GetValuesRapid(
                                                    short handle,
                                                    ref uint noOfSamples,
                                                    uint fromSegmentIndex,
                                                    uint toSegmentIndex,
                                                    uint downSampleRatio,
                                                    DownSamplingMode downSampleRatioMode,
                                                    short[] overflow);
        /// <summary>
        /// handle, the handle of the device.
        /// powerstate, the required state of the unit.Either
        /// PICO_POWER_SUPPLY_CONNECTED or
        /// PICO_POWER_SUPPLY_NOT_CONNECTED.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="status"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_POWER_SUPPLY_REQUEST_INVALID
        /// PICO_INVALID_PARAMETER
        /// PICO_NOT_RESPONDING
        /// PICO_INVALID_HANDLE</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aChangePowerSource")]
        public static extern uint ChangePowerSource(
                                                        short handle,
                                                        uint status);
        /// <summary>
        /// * count, on exit, the number of PicoScope 5000 Series units found
        /// * serials, on exit, a list of serial numbers separated by commas
        /// and terminated by a final null. Example:
        /// AQ005/139,VDR61/356,ZOR14/107. Can be NULL on entry if serial
        /// numbers are not required.
        /// * serialLth, on entry, the length of the int8_t buffer pointed to
        /// by serials; on exit, the length of the string written to serials
        ///         /// </summary>
        /// <param name="count"></param>
        /// <param name="serials"></param>
        /// <param name="serialLength"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_BUSY
        /// PICO_NULL_PARAMETER
        /// PICO_FW_FAIL
        /// PICO_CONFIG_FAIL
        /// PICO_MEMORY_FAIL
        /// PICO_CONFIG_FAIL_AWG
        /// PICO_INITIALISE_FPGA</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aEnumerateUnits")]
        public static extern uint EnumerateUnits(
                                                    out short count,
                                                    StringBuilder serials,
                                                    ref short serialLength);
        /// <summary>
        /// handle, the handle of the required device
        /// offsetVoltage, the voltage offset, in microvolts, to be applied to
        /// the waveform.
        /// pkToPk, the peak-to-peak voltage, in microvolts, of the waveform
        /// signal.
        /// Note that if the signal voltages defined by the combination of
        /// offsetVoltage and pkToPk extend outside the voltage range of
        /// the signal generator, the output waveform will be clipped.
        /// startDeltaPhase, the initial value added to the phase
        /// accumulator as the generator begins to step through the waveform
        /// buffer.
        ///     stopDeltaPhase, the final value added to the phase accumulator
        /// before the generator restarts or reverses the sweep.
        /// deltaPhaseIncrement, the amount added to the delta phase
        /// value every time the dwellCount period expires. This determines
        /// the amount by which the generator sweeps the output frequency in
        /// each dwell period.
        /// dwellCount, the time, in 50 ns steps, between successive
        /// additions of deltaPhaseIncrement to the delta phase
        /// accumulator. This determines the rate at which the generator sweeps
        /// the output frequency.
        /// Minimum value: PS5000A_MIN_DWELL_COUNT
        /// * arbitraryWaveform, a buffer that holds the waveform pattern
        /// as a set of samples equally spaced in time.If pkToPk is set to its
        /// maximum (4 V) and offsetVoltage is set to 0, then a sample of
        /// –32768 corresponds to –2 V, and +32767 to +2 V.
        /// arbitraryWaveformSize, the size of the arbitrary waveform
        /// buffer, in samples, from MIN_SIG_GEN_BUFFER_SIZE to
        /// PS5X42A_MAX_SIG_GEN_BUFFER_SIZE,
        /// PS5X43A_MAX_SIG_GEN_BUFFER_SIZE or
        /// PS5X44A_MAX_SIG_GEN_BUFFER_SIZE, depending on the
        /// oscilloscope model.
        /// sweepType, determines whether the startDeltaPhase is swept
        /// up to the stopDeltaPhase, down to it, or repeatedly up and down.
        /// Use one of these values:
        /// PS5000A_UP
        /// PS5000A_DOWN
        /// PS5000A_UPDOWN
        /// PS5000A_DOWNUP
        /// operation, the type of waveform to be produced, specified by one
        /// of the following enumerated types:
        /// PS5000A_ES_OFF, normal signal generator operation specified
        /// by wavetype.
        /// PS5000A_WHITENOISE, the signal generator produces white
        /// noise and ignores all settings except pkToPk and
        /// offsetVoltage.
        /// PS5000A_PRBS, produces a random bitstream with a bit rate
        /// specified by the start and stop frequency.
        /// indexMode, specifies how the signal will be formed from the
        /// arbitrary waveform data. Single and dual index modes are possible.
        /// Use one of these constants:
        /// PS5000A_SINGLE
        /// PS5000A_DUAL
        /// shots,
        /// sweeps,
        /// triggerType,
        /// triggerSource,
        /// extInThreshold: see ps5000aSigGenBuiltIn
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="offsetVoltage"></param>
        /// <param name="pkTopk"></param>
        /// <param name="startDeltaPhase"></param>
        /// <param name="stopDeltaPhase"></param>
        /// <param name="deltaPhaseIncrement"></param>
        /// <param name="dwellCount"></param>
        /// <param name="arbitaryWaveform"></param>
        /// <param name="arbitaryWaveformSize"></param>
        /// <param name="sweepType"></param>
        /// <param name="operation"></param>
        /// <param name="indexMode"></param>
        /// <param name="shots"></param>
        /// <param name="sweeps"></param>
        /// <param name="triggerType"></param>
        /// <param name="triggerSource"></param>
        /// <param name="extInThreshold"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_AWG_NOT_SUPPORTED
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_BUSY
        /// PICO_INVALID_HANDLE
        /// PICO_SIG_GEN_PARAM
        /// PICO_SHOTS_SWEEPS_WARNING
        /// PICO_NOT_RESPONDING
        /// PICO_WARNING_EXT_THRESHOLD_CONFLICT
        /// PICO_NO_SIGNAL_GENERATOR
        /// PICO_SIGGEN_OFFSET_VOLTAGE
        /// PICO_SIGGEN_PK_TO_PK
        /// PICO_SIGGEN_OUTPUT_OVER_VOLTAGE
        /// PICO_DRIVER_FUNCTION
        /// PICO_SIGGEN_WAVEFORM_SETUP_FAILED</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenArbitrary")]
        public static extern uint SetSigGenArbitrary(
                                                        short handle,
                                                        int offsetVoltage,
                                                        uint pkTopk,
                                                        uint startDeltaPhase,
                                                        uint stopDeltaPhase,
                                                        uint deltaPhaseIncrement,
                                                        uint dwellCount,
                                                        short[] arbitaryWaveform,
                                                        int arbitaryWaveformSize,
                                                        SweepType sweepType,
                                                        ExtraOperations operation,
                                                        IndexMode indexMode,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);
        /// <summary>
        /// se ovan. samma sak
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="startDeltaPhase"></param>
        /// <param name="stopDeltaPhase"></param>
        /// <param name="deltaPhaseIncrement"></param>
        /// <param name="dwellCount"></param>
        /// <param name="sweepType"></param>
        /// <param name="shots"></param>
        /// <param name="sweeps"></param>
        /// <param name="triggerType"></param>
        /// <param name="triggerSource"></param>
        /// <param name="extInThreshold"></param>
        /// <returns>
        /// PICO_OK if successful.
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION
        /// PICO_NO_SIGNAL_GENERATOR
        /// PICO_WARNING_AUX_OUTPUT_CONFLICT
        /// PICO_WARNING_EXT_THRESHOLD_CONFLICT
        /// PICO_SIGGEN_OFFSET_VOLTAGE
        /// PICO_SIGGEN_OUTPUT_OVER_VOLTAGE
        /// PICO_SIGGEN_PK_TO_PK
        /// PICO_SIGGEN_OFFSET_VOLTAGE
        /// PICO_SIG_GEN_PARAM
        /// PICO_SHOTS_SWEEPS_WARNING
        /// PICO_AWG_NOT_SUPPORTED
        /// PICO_BUSY
        /// PICO_SIGGEN_WAVEFORM_SETUP_FAILED
        /// PICO_NOT_RESPONDING
        /// PICO_POWER_SUPPLY_UNDERVOLTAGE
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_POWER_SUPPLY_CONNECTED</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenPropertiesArbitrary")]
        public static extern uint SetSigGenPropertiesArbitrary(
                                                        short handle,
                                                        uint startDeltaPhase,
                                                        uint stopDeltaPhase,
                                                        uint deltaPhaseIncrement,
                                                        uint dwellCount,
                                                        SweepType sweepType,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);
        /// <summary>
        /// handle, the handle of the required device
        /// offsetVoltage, the voltage offset, in microvolts, to be applied to
        /// the waveform
        /// pkToPk, the peak-to-peak voltage, in microvolts, of the waveform
        /// signal.
        /// Note that if the signal voltages described by the combination of
        /// offsetVoltage and pkToPk extend outside the voltage range of
        /// the signal generator, the output waveform will be clipped.
        /// waveType, the type of waveform to be generated.
        /// PS5000A_SINE sine wave
        /// PS5000A_SQUARE square wave
        /// PS5000A_TRIANGLE triangle wave
        /// PS5000A_DC_VOLTAGE DC voltage
        /// The following waveTypes apply to B models only:
        /// PS5000A_RAMP_UP rising sawtooth
        /// PS5000A_RAMP_DOWN falling sawtooth
        /// PS5000A_SINC sin (x)/x
        /// PS5000A_GAUSSIAN Gaussian
        /// PS5000A_HALF_SINE half (full-wave rectified) sine
        /// startFrequency, the frequency that the signal generator will
        /// initially produce.For allowable values see
        /// PS5000A_SINE_MAX_FREQUENCY and related values.
        /// stopFrequency, the frequency at which the sweep reverses direction
        /// or returns to the initial frequency
        /// increment, the amount of frequency increase or decrease in sweep
        /// mode
        /// dwellTime, the time for which the sweep stays at each frequency, in
        /// seconds
        /// sweepType, whether the frequency will sweep from
        /// startFrequency to stopFrequency, in the opposite direction, or
        /// repeatedly reverse direction. Use one of these constants:
        /// PS5000A_UP
        /// PS5000A_DOWN
        /// PS5000A_UPDOWN
        /// PS5000A_DOWNUP
        /// operation, the type of waveform to be produced, specified by one of
        /// the following enumerated types (B models only):
        /// PS5000A_ES_OFF, normal signal generator operation specified by
        /// wavetype.
        /// PS5000A_WHITENOISE, the signal generator produces white noise
        /// and ignores all settings except pkToPk and offsetVoltage.
        /// PS5000A_PRBS, produces a random bitstream with a bit rate
        /// specified by the start and stop frequency.
        /// shots,
        /// 0: sweep the frequency as specified by sweeps
        /// 1...PS5000A_MAX_SWEEPS_SHOTS: the number of cycles of the
        /// waveform to be produced after a trigger event. sweeps must be
        /// zero.
        /// PS5000A_SHOT_SWEEP_TRIGGER_CONTINUOUS_RUN: start and run
        /// continuously after trigger occurs
        /// sweeps,
        /// 0: produce number of cycles specified by shots
        /// 1..PS5000A_MAX_SWEEPS_SHOTS: the number of times to sweep
        /// the frequency after a trigger event, according to sweepType.
        /// shots must be zero.
        /// PS5000A_SHOT_SWEEP_TRIGGER_CONTINUOUS_RUN: start a sweep
        /// and continue after trigger occurs
        /// triggerType, the type of trigger that will be applied to the signal
        /// generator:
        /// PS5000A_SIGGEN_RISING trigger on rising edge
        /// PS5000A_SIGGEN_FALLING trigger on falling edge
        /// PS5000A_SIGGEN_GATE_HIGH run while trigger is high
        /// PS5000A_SIGGEN_GATE_LOW run while trigger is low
        /// triggerSource, the source that will trigger the signal generator.
        /// PS5000A_SIGGEN_NONE run without waiting for trigger
        /// PS5000A_SIGGEN_SCOPE_TRIG use scope trigger
        /// PS5000A_SIGGEN_EXT_IN use EXT input
        /// PS5000A_SIGGEN_SOFT_TRIG wait for software trigger
        /// provided by
        /// ps5000aSigGenSoftwareCo
        /// ntrol
        /// PS5000A_SIGGEN_TRIGGER_RAW reserved
        /// If a trigger source other than P5000A_SIGGEN_NONE is specified, then
        /// either shots or sweeps, but not both, must be non-zero.
        /// extInThreshold, used to set trigger level for external trigger
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="startFrequency"></param>
        /// <param name="stopFrequency"></param>
        /// <param name="increment"></param>
        /// <param name="dwellTime"></param>
        /// <param name="sweepType"></param>
        /// <param name="shots"></param>
        /// <param name="sweeps"></param>
        /// <param name="triggerType"></param>
        /// <param name="triggerSource"></param>
        /// <param name="extInThreshold"></param>
        /// <returns>PICO_OK if successful.
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION
        /// PICO_NO_SIGNAL_GENERATOR
        /// PICO_SIG_GEN_PARAM
        /// PICO_WARNING_AUX_OUTPUT_CONFLICT
        /// PICO_WARNING_EXT_THRESHOLD_CONFLICT
        /// PICO_SIGGEN_DC_VOLTAGE_NOT_CONFIGURABLE
        /// PICO_BUSY
        /// PICO_SIGGEN_WAVEFORM_SETUP_FAILED
        /// PICO_NOT_RESPONDING
        /// PICO_POWER_SUPPLY_UNDERVOLTAGE
        /// PICO_USB3_0_DEVICE_NON_USB3_0_PORT
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_POWER_SUPPLY_CONNECTED</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenPropertiesBuiltIn")]
        public static extern uint SetSigGenPropertiesBuiltIn(
                                                        short handle,
                                                        double startFrequency,
                                                        double stopFrequency,
                                                        double increment,
                                                        double dwellTime,
                                                        SweepType sweepType,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);
        /// <summary>
        /// handle, the handle of the required device
        /// offsetVoltage,
        /// pkToPk,
        /// waveType,
        /// startFrequency,
        /// stopFrequency,
        /// increment,
        /// dwellTime,
        /// sweepType,
        /// operation,
        /// shots,
        /// sweeps,
        /// triggerType,
        /// triggerSource,
        /// extInThreshold: see ps5000aSetSigGenBuiltIn
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="offsetVoltage"></param>
        /// <param name="pkTopk"></param>
        /// <param name="waveType"></param>
        /// <param name="startFrequency"></param>
        /// <param name="stopFrequency"></param>
        /// <param name="increment"></param>
        /// <param name="dwellTime"></param>
        /// <param name="sweepType"></param>
        /// <param name="operation"></param>
        /// <param name="shots"></param>
        /// <param name="sweeps"></param>
        /// <param name="triggerType"></param>
        /// <param name="triggerSource"></param>
        /// <param name="extInThreshold"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_BUSY
        /// PICO_POWER_SUPPLY_CONNECTED
        /// PICO_POWER_SUPPLY_NOT_CONNECTED
        /// PICO_INVALID_HANDLE
        /// PICO_SIG_GEN_PARAM
        /// PICO_SHOTS_SWEEPS_WARNING
        /// PICO_NOT_RESPONDING
        /// PICO_WARNING_AUX_OUTPUT_CONFLICT
        /// PICO_WARNING_EXT_THRESHOLD_CONFLICT
        /// PICO_NO_SIGNAL_GENERATOR
        /// PICO_SIGGEN_OFFSET_VOLTAGE
        /// PICO_SIGGEN_PK_TO_PK
        /// PICO_SIGGEN_OUTPUT_OVER_VOLTAGE
        /// PICO_DRIVER_FUNCTION
        /// PICO_SIGGEN_WAVEFORM_SETUP_FAILED
        /// PICO_NOT_RESPONDING</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenBuiltInV2")]
        public static extern uint SetSigGenBuiltInV2(
                                                        short handle,
                                                        int offsetVoltage,
                                                        uint pkTopk,
                                                        WaveType waveType,
                                                        double startFrequency,
                                                        double stopFrequency,
                                                        double increment,
                                                        double dwellTime,
                                                        SweepType sweepType,
                                                        ExtraOperations operation,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);

        /// <summary>
        /// This function converts a frequency to a phase count for use with the arbitrary
        /// waveform generator(AWG). The value returned depends on the length of the buffer,
        /// the index mode passed and the device model.The phase count can then be sent to the
        /// driver through ps5000aSetSigGenArbitrary or
        /// ps5000aSetSigGenPropertiesArbitrary
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="frequency"></param>
        /// <param name="indexMode"></param>
        /// <param name="bufferLength"></param>
        /// <param name="phase"></param>
        /// <returns>
        /// PICO_OK
        /// PICO_NOT_SUPPORTED_BY_THIS_DEVICE, if the device does not
        /// have an AWG.
        /// PICO_SIGGEN_FREQUENCY_OUT_OF_RANGE, if the frequency is out
        /// of range.
        /// PICO_NULL_PARAMETER, if phase is a NULL pointer.
        /// PICO_SIG_GEN_PARAM, if indexMode or bufferLength is out of
        /// range.
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION</returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSigGenFrequencyToPhase")]
        public static extern uint SigGenFrequencyToPhase(
                                                                short handle,
                                                                double frequency,
                                                                IndexMode indexMode,
                                                                uint bufferLength,
                                                                out uint phase);
        /// <summary>
        /// This function returns the range of possible sample values and waveform buffer sizes
        /// that can be supplied to ps5000aSetSignGenArbitrary for setting up the arbitrary
        /// waveform generator(AWG). These values vary between different models in the
        /// PicoScope 5000 Series.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="minArbitraryWaveformValue"></param>
        /// <param name="maxArbitraryWaveformValue"></param>
        /// <param name="minArbitraryWaveformSize"></param>
        /// <param name="maxArbitraryWaveformSize"></param>
        /// <returns>PICO_OK
        /// PICO_NOT_SUPPORTED_BY_THIS_DEVICE, if the device does not
        /// have an arbitrary waveform generator.
        /// PICO_NULL_PARAMETER, if all the parameter pointers are NULL.
        /// PICO_INVALID_HANDLE
        /// PICO_DRIVER_FUNCTION
        /// </returns>
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSigGenArbitraryMinMaxValues")]
        public static extern uint SigGenArbitraryMinMaxValues(
                                                                short handle,
                                                                out short minArbitraryWaveformValue,
                                                                out short maxArbitraryWaveformValue,
                                                                out uint minArbitraryWaveformSize,
                                                                out uint maxArbitraryWaveformSize);

        #endregion
    }
}
