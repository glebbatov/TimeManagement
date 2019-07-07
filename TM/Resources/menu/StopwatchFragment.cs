using System.Timers;
using Android.OS;
using Android.Views;
using Android.Widget;
using TomerGoldst.ProgressCircleLib;
using Android.Content;
using System;

namespace TM.Resources.menu
{
    public class StopwatchFragment : Android.Support.V4.App.Fragment
    {
        ProgressCircle progressCircle;
        Button btnStart, btnStop, btnReset, btnLap;
        TextView txtTimer;
        Timer timer;
        LinearLayout container_row;
        bool running;
        int hour = 0, min = 0, sec = 0, milisec = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_stopWatch, container, false);
            progressCircle = rootView.FindViewById<ProgressCircle>(Resource.Id.stopwatch_circle_progress);
            txtTimer = rootView.FindViewById<TextView>(Resource.Id.stopwatch_txtTimer);
            btnStart = rootView.FindViewById<Button>(Resource.Id.stopwatch_btnStart);
            btnStop = rootView.FindViewById<Button>(Resource.Id.stopwatch_btnStop);
            btnReset = rootView.FindViewById<Button>(Resource.Id.stopwatch_btnReset);
            btnLap = rootView.FindViewById<Button>(Resource.Id.stopwatch_btnLap);
            container_row = rootView.FindViewById<LinearLayout>(Resource.Id.container_row);

            StartState();

            //sec = 55;//
            //progressCircle.SetProgressWithoutAnimation(55);//

            btnStart.Click += delegate
            {
                timer = new Timer();
                timer.Interval = 100;               //0.1 second
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnReset.Enabled = false;
                btnLap.Enabled = true;
                running = true;
                ButtonsStateColor();
            };

            btnStop.Click += delegate
            {
                timer.Dispose();
                timer = null;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = true;
                btnLap.Enabled = false;
                running = false;
                ButtonsStateColor();
            };

            btnReset.Click += delegate
            {
                hour = 0;
                min = 0;
                sec = 0;
                milisec = 0;
                txtTimer.Text = "00:00:00:0";
                progressCircle.SetProgress(0, 0);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = false;
                btnLap.Enabled = false;
                running = false;
                container_row.RemoveAllViewsInLayout();
                ButtonsStateColor();
            };

            btnLap.Click += delegate
            {
                LayoutInflater inflater_row = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                View addView = inflater_row.Inflate(Resource.Layout.fragment_stopWatch_row, null);
                TextView textContent = addView.FindViewById<TextView>(Resource.Id.stopWatch_txtRow);
                textContent.Text = txtTimer.Text;
                container_row.AddView(addView);
            };

            if (running == false)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = false;
                btnLap.Enabled = false;
            }
            if (running == true)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnReset.Enabled = false;
                btnLap.Enabled = true;
            }

            return rootView;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            milisec++;
            if (milisec >= 10)
            {
                sec++;
                milisec = 0;
            }
            if (sec == 59)
            {
                this.Activity.RunOnUiThread(() => { progressCircle.SetProgressWithoutAnimation(0); });
                min++;
                sec = 0;
            }
            if (min == 59)
            {
                hour++;
                min = 0;
            }
            if (hour > 99)
            {
                hour = 0;
            }
            this.Activity.RunOnUiThread(() => { progressCircle.Progress = sec; });
            this.Activity.RunOnUiThread(() => {
                txtTimer.Text = (string.Format("{0:00}:{1:00}:{2:00}:{3:0}", hour, min, sec, milisec));
            });
        }
        private void StartState()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnReset.Enabled = false;
            btnLap.Enabled = false;
            btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimaryDim));
            txtTimer.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:0}", hour, min, sec, milisec);
        }
        private void ButtonsStateColor()
        {
            if (btnReset.Enabled == true)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimary));
            }
            if (btnReset.Enabled == false)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimaryDim));
            }
        }
    }
}
