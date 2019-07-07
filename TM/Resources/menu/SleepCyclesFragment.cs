using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TM.Resources.menu
{
    public class SleepCyclesFragment : Android.Support.V4.App.Fragment
    {
        //TODO
        //sleep cycles time input (ex.90min)
        //when alarm is set = ShowToast.Short ex."Alarm is set for 08:00AM"
        //when alarm is set = ShowToast.Long ex "Sleep time for 6 cycles will be 9 hours"

        Button sleepCyclesPlusBtn, sleepCyclesMinusBtn;
        TextView sleepCyclesText;
        int sleepCyclesInt = 6;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_sleepCycles, container, false);

            sleepCyclesPlusBtn = rootView.FindViewById<Button>(Resource.Id.sleepCyclesPlus);
            sleepCyclesMinusBtn = rootView.FindViewById<Button>(Resource.Id.sleepCyclesMinus);
            sleepCyclesText = rootView.FindViewById<TextView>(Resource.Id.sleepCyclesText);

            sleepCyclesPlusBtn.Click += delegate
            {
                if (sleepCyclesInt >= 9)
                {
                    sleepCyclesInt = 9;
                }
                else
                {
                    sleepCyclesInt = sleepCyclesInt + 1;
                }
                sleepCyclesText.Text = Convert.ToString(sleepCyclesInt);
            };
            sleepCyclesMinusBtn.Click += delegate
            {
                if (sleepCyclesInt <= 1)
                {
                    sleepCyclesInt = 1;
                }
                else
                {
                    sleepCyclesInt = sleepCyclesInt - 1;
                }
                sleepCyclesText.Text = Convert.ToString(sleepCyclesInt);
            };

            return rootView;
        }
    }
}