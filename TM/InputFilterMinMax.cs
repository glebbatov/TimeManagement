using System;
using System.Diagnostics;
using Android.Text;

namespace TM
{
    //Input Filter (interface)
    //https://dzone.com/articles/xamarinandroid-implementing

    class InputFilterMinMax : Java.Lang.Object, IInputFilter
    {
        private int _min = 0;
        private int _max = 0;

        public InputFilterMinMax(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public Java.Lang.ICharSequence FilterFormatted(Java.Lang.ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            try
            {
                string val = dest.ToString().Insert(dstart, source.ToString());
                int input = int.Parse(val);
                if (IsInRange(_min, _max, input))
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FilterFormatted Error: " + ex.Message);
            }

            return new Java.Lang.String(string.Empty);
        }

        private bool IsInRange(int min, int max, int input)
        {
            return max > min ? input >= min && input <= max : input >= max && input <= min;
        }
    }
}