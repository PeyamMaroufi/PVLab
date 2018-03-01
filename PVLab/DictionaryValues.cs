using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVLab
{
    class DictionaryValues
    {
        // These are used to calculate the samling interval based on user's choice on combo box in plot class
        // Streaming is done in nano sec. Not easy to understand now huh?
        public Dictionary<double, string> dictionary = new Dictionary<double, string>();
        public List<double> dictionaryInSec = new List<double>();
        public List<double> dictionaryValueDiv = new List<double>();
        public List<double> DivValueLimit = new List<double>();

        public DictionaryValues()
        {
            dictionary.Add(1000, "1 µs/Div ");
            dictionary.Add(2000, "2 µs/Div ");
            dictionary.Add(5000, "5 µs/Div ");
            dictionary.Add(10000, "10 µs/Div ");
            dictionary.Add(20000, "20 µs/Div ");
            dictionary.Add(50000, "50 µs/Div ");
            dictionary.Add(100000, "100 µs/Div ");
            dictionary.Add(200000, "200 µs/Div ");
            dictionary.Add(500000, "500 µs/Div ");
            dictionary.Add(1 * Math.Pow(10, 6), "1   ms/Div ");
            dictionary.Add(2 * Math.Pow(10, 6), "2   ms/Div ");
            dictionary.Add(5 * Math.Pow(10, 6), "5   ms/Div ");
            dictionary.Add(10 * Math.Pow(10, 6), "10  ms/Div ");
            dictionary.Add(20 * Math.Pow(10, 6), "20  ms/Div ");
            dictionary.Add(50 * Math.Pow(10, 6), "50  ms/Div ");
            dictionary.Add(100 * Math.Pow(10, 6), "100 ms/Div ");
            dictionary.Add(200 * Math.Pow(10, 6), "200 ms/Div ");
            dictionary.Add(500 * Math.Pow(10, 6), "500 ms/Div ");
            dictionary.Add(1 * Math.Pow(10, 9), "1    s/Div ");
            dictionary.Add(2 * Math.Pow(10, 9), "2    s/Div ");
            dictionary.Add(5 * Math.Pow(10, 9), "5    s/Div ");

            // Sec
            dictionaryInSec.Add(1 * Math.Pow(10, -6)); //"10 µs/Div "
            dictionaryInSec.Add(2 * Math.Pow(10, -6)); //"20 µs/Div "
            dictionaryInSec.Add(5 * Math.Pow(10, -6)); //"50 µs/Div "
            dictionaryInSec.Add(10 * Math.Pow(10, -6)); //"10 µs/Div "
            dictionaryInSec.Add(20 * Math.Pow(10, -6)); //"20 µs/Div "
            dictionaryInSec.Add(50 * Math.Pow(10, -6)); //"50 µs/Div "
            dictionaryInSec.Add(100 * Math.Pow(10, -6)); //"100 µs/Div "
            dictionaryInSec.Add(200 * Math.Pow(10, -6)); //"200 µs/Div "
            dictionaryInSec.Add(500 * Math.Pow(10, -6)); //"500 µs/Div "
            dictionaryInSec.Add(1 * Math.Pow(10, -3));     // "1   ms/Div "
            dictionaryInSec.Add(2 * Math.Pow(10, -3));     //  "2   ms/Div "
            dictionaryInSec.Add(5 * Math.Pow(10, -3));     // "5   ms/Div "
            dictionaryInSec.Add(10 * Math.Pow(10, -3));     // "10  ms/Div "
            dictionaryInSec.Add(20 * Math.Pow(10, -3));     // "20  ms/Div "
            dictionaryInSec.Add(50 * Math.Pow(10, -3));     //  "50  ms/Div "
            dictionaryInSec.Add(100 * Math.Pow(10, -3));    // "100 ms/Div "
            dictionaryInSec.Add(200 * Math.Pow(10, -3));    // "200 ms/Div "
            dictionaryInSec.Add(500 * Math.Pow(10, -3));    // "500 ms/Div "
            dictionaryInSec.Add(1 * Math.Pow(10, 0));       //  "1    s/Div "
            dictionaryInSec.Add(2 * Math.Pow(10, 0));       //  "2    s/Div "
            dictionaryInSec.Add(5 * Math.Pow(10, 0));       //  "5    s/Div "

            // Div limits
            dictionaryValueDiv.Add(1 * 10); //"10 µs/Div "
            dictionaryValueDiv.Add(2 * 10); //"20 µs/Div "
            dictionaryValueDiv.Add(5 * 10); //"50 µs/Div "
            dictionaryValueDiv.Add(10 * 10); //"10 µs/Div "
            dictionaryValueDiv.Add(20 * 10); //"20 µs/Div "
            dictionaryValueDiv.Add(50 * 10); //"50 µs/Div "
            dictionaryValueDiv.Add(100 * 10); //"100 µs/Div "
            dictionaryValueDiv.Add(200 * 10); //"200 µs/Div "
            dictionaryValueDiv.Add(500 * 10); //"500 µs/Div "
            dictionaryValueDiv.Add(1 * 10);     // "1   ms/Div "
            dictionaryValueDiv.Add(2 * 10);     //  "2   ms/Div "
            dictionaryValueDiv.Add(5 * 10);     // "5   ms/Div "
            dictionaryValueDiv.Add(10 * 10);     // "10  ms/Div "
            dictionaryValueDiv.Add(20 * 10);     // "20  ms/Div "
            dictionaryValueDiv.Add(50 * 10);     //  "50  ms/Div "
            dictionaryValueDiv.Add(100 * 10);    // "100 ms/Div "
            dictionaryValueDiv.Add(200 * 10);    // "200 ms/Div "
            dictionaryValueDiv.Add(500 * 10);    // "500 ms/Div "
            dictionaryValueDiv.Add(1 * 10);       //  "1    s/Div "
            dictionaryValueDiv.Add(2 * 10);       //  "2    s/Div "
            dictionaryValueDiv.Add(5 * 10);       //  "5    s/Div "


            // Div limits
            DivValueLimit.Add(1000); //"10 µs/Div "
            DivValueLimit.Add(1000); //"20 µs/Div "
            DivValueLimit.Add(1000); //"50 µs/Div "
            DivValueLimit.Add(1000); //"10 µs/Div "
            DivValueLimit.Add(1000); //"20 µs/Div "
            DivValueLimit.Add(1000); //"50 µs/Div "
            DivValueLimit.Add(1000); //"100 µs/Div "
            DivValueLimit.Add(1000); //"200 µs/Div "
            DivValueLimit.Add(1000); //"500 µs/Div "
            DivValueLimit.Add(1000000);     // "1   ms/Div "
            DivValueLimit.Add(1000000);     //  "2   ms/Div "
            DivValueLimit.Add(1000000);     // "5   ms/Div "
            DivValueLimit.Add(1000000);     // "10  ms/Div "
            DivValueLimit.Add(1000000);     // "20  ms/Div "
            DivValueLimit.Add(1000000);     //  "50  ms/Div "
            DivValueLimit.Add(1000000);    // "100 ms/Div "
            DivValueLimit.Add(1000000);    // "200 ms/Div "
            DivValueLimit.Add(1000000);    // "500 ms/Div "
            DivValueLimit.Add(1 * 10);       //  "1    s/Div "
            DivValueLimit.Add(2 * 10);       //  "2    s/Div "
            DivValueLimit.Add(5 * 10);       //  "5    s/Div "


        }

    }
}
