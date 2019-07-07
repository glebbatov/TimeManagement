using System;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Timers;
using TomerGoldst.ProgressCircleLib;

namespace TM.Resources.menu
{
    public class TimerFragment : Android.Support.V4.App.Fragment
    {
        ProgressCircle progressCircle;
        Button btnStart, btnStop, btnReset;
        Timer timer;
        bool running;
        int hour = 0, min = 0, sec = 0;

        EditText txtTimerHour;
        EditText txtTimerMin;
        EditText txtTimerSec;

        MediaPlayer player;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_timer, container, false);

            progressCircle = rootView.FindViewById<ProgressCircle>(Resource.Id.circle_progress);
            txtTimerHour = rootView.FindViewById<EditText>(Resource.Id.txtTimerHour);
            txtTimerMin = rootView.FindViewById<EditText>(Resource.Id.txtTimerMin);
            txtTimerSec = rootView.FindViewById<EditText>(Resource.Id.txtTimerSec);

            btnStart = rootView.FindViewById<Button>(Resource.Id.btnStart);
            btnStop = rootView.FindViewById<Button>(Resource.Id.btnStop);
            btnReset = rootView.FindViewById<Button>(Resource.Id.btnReset);

            txtTimerHour.ClearFocus();

            //input limiter
            txtTimerHour.SetFilters(new Android.Text.IInputFilter[] { new InputFilterMinMax(0, 99) });
            txtTimerMin.SetFilters(new Android.Text.IInputFilter[] { new InputFilterMinMax(0, 60) });
            txtTimerSec.SetFilters(new Android.Text.IInputFilter[] { new InputFilterMinMax(0, 60) });

            btnStart.Click += delegate
            {
                if (txtTimerHour.Text == "")
                {
                    txtTimerHour.Text = "00";
                }
                if (txtTimerMin.Text == "")
                {
                    txtTimerMin.Text = "00";
                }
                if (txtTimerSec.Text == "")
                {
                    txtTimerSec.Text = "00";
                }

                hour = Convert.ToInt32(txtTimerHour.Text);
                min = Convert.ToInt32(txtTimerMin.Text);
                sec = Convert.ToInt32(txtTimerSec.Text);

                if (hour > 0 || min > 0 || sec > 0)
                {
                    timer = new Timer();
                    timer.Interval = 1000;    //1000 = 1second
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    btnReset.Enabled = false;
                    running = true;
                    txtInputDisable();
                    player = MediaPlayer.Create(Context, Resource.Drawable.alarm1);
                    ButtonsStateColor();
                }
                else
                {
                    ShowToast("Please, set the time", true);
                }
            };

            btnStop.Click += delegate
            {
                timer.Dispose();
                timer = null;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = true;
                running = false;
                txtInputEnable();
                ButtonsStateColor();
                if (player.Looping == true)
                {
                    player.Stop();
                }
            };

            btnReset.Click += delegate
            {
                hour = 00;
                min = 00;
                sec = 00;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = false;
                running = false;
                txtInputEnable();
                ResetUi();
            };

            if (running == false)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = false;
            }
            if (running == true)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnReset.Enabled = false;
            }

            return rootView;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sec--;
            if (sec <= 0)
            {
                if (min > 0 || hour > 0)
                {
                    if (min == 0)
                    {
                        if (hour >= 0)
                        {
                            hour = hour - 1;
                            min = 60;
                            if (hour < 0)
                            {
                                hour = 0;
                            }
                        }
                    }
                    min = min - 1;
                    sec = 60;
                    if (min < 0)
                    {
                        min = 0;
                    }
                }
                if (sec < 0)
                {
                    sec = 0;
                }
            }
            if (sec == 0 && min == 0 && hour == 0)
            {
                alarmFinished();
            }
            ResetUi();
        }
        private void alarmFinished()
        {
            timer.Stop();
            ShowToast("Time is up!", true);
            player.Start();
            player.Looping = true;
            ResetUi();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnReset.Enabled = false;
        }
        private void ShowToast(string text, bool IsLengthShort = false)
        {
            Handler mainHandler = new Handler(Looper.MainLooper);
            Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
            {
                var duration = IsLengthShort ? ToastLength.Short : ToastLength.Long;
                Toast.MakeText(Context, text, duration).Show();
            });

            mainHandler.Post(runnableToast);
        }
        private void txtInputDisable()
        {
            txtTimerHour.Enabled = false;
            txtTimerMin.Enabled = false;
            txtTimerSec.Enabled = false;
        }
        private void txtInputEnable()
        {
            txtTimerHour.Enabled = true;
            txtTimerMin.Enabled = true;
            txtTimerSec.Enabled = true;
        }
        private void ButtonsStateColor()
        {
            if (btnStart.Enabled == true)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimary));
            }
            if (btnStart.Enabled == false)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimaryDim));
            }

            if (btnStop.Enabled == true)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimary));
            }
            if (btnStop.Enabled == false)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimaryDim));
            }

            if (btnReset.Enabled == true)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimary));
            }
            if (btnReset.Enabled == false)
            {
                btnReset.SetTextColor(Resources.GetColor(Resource.Color.colorPrimaryDim));
            }
        }
        private void ResetUi()
        {
            this.Activity.RunOnUiThread(() => { progressCircle.Progress = sec; });
            this.Activity.RunOnUiThread(() => { txtTimerHour.Text = (string.Format("{0:00}", hour)); });
            this.Activity.RunOnUiThread(() => { txtTimerMin.Text = (string.Format("{0:00}", min)); });
            this.Activity.RunOnUiThread(() => { txtTimerSec.Text = (string.Format("{0:00}", sec)); });
        }
        /*
        private void hideTheKeyboard()
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(txtTimerHour.WindowToken, 0);
            imm.HideSoftInputFromWindow(txtTimerMin.WindowToken, 0);
            imm.HideSoftInputFromWindow(txtTimerSec.WindowToken, 0);
        }
        */
    }
}