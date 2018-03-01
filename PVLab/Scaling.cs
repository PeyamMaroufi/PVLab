using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVLab
{
    public class Scaling
    {


        /// <summary>
        ///  Are used to converts between values. Are usable in triggered state
        /// </summary>
        /// <param name="v"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        // Convert an 16-bit ADC count into milivolts
        public static double adc_to_mv(short v, int range, int _maxValue)
        {
            return (v * StaticVariable.inputRanges[range]) / _maxValue;
        }

        // Convert a millivolt value into a 16-bit ADC count
        short mv_to_adc(short mv, short ch, int _maxValue)
        {
            return (short)((mv * _maxValue) / StaticVariable.inputRanges[ch]);
        }
    }
}
