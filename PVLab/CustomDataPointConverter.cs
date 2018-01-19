using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PVLab
{
    class CustomDataPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hitResult = values[0] as TrackerHitResult;
            if (hitResult == null)
                return string.Empty;

            var tooltipProvider = values[1] as CustomTooltipProvider;
            if (tooltipProvider == null)
                return string.Empty;

            if (tooltipProvider.GetCustomTooltip == null)
                return hitResult.Text;

            var customTooltip = tooltipProvider.GetCustomTooltip(hitResult);
            if (string.IsNullOrWhiteSpace(customTooltip))
                return hitResult.Text;

            return hitResult.Text + Environment.NewLine + customTooltip;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
