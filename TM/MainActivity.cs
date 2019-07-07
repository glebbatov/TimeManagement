using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using Android.Views;
using TM.Resources.menu;
using Android.Content.PM;
using System;

namespace TM
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", Icon = "@drawable/icon",
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        readonly Android.Support.V4.App.Fragment AlarmFragment = new AlarmFragment();
        readonly Android.Support.V4.App.Fragment SleepCyclesFragment = new SleepCyclesFragment();
        readonly Android.Support.V4.App.Fragment StopwatchFragment = new StopwatchFragment();
        readonly Android.Support.V4.App.Fragment TimerFragment = new TimerFragment();

        Android.Support.V4.App.Fragment selectedFragment = null;
        Android.Support.V4.App.Fragment nonSelectedFragment1 = null;
        Android.Support.V4.App.Fragment nonSelectedFragment2 = null;
        Android.Support.V4.App.Fragment nonSelectedFragment3 = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //set "Stopwatch" BottomNavigationView by default
            navigation.Menu.FindItem(Resource.Id.nav_stopwatch).SetChecked(true);
            SelectDefaultFragment();

            RemoveShiftMode(navigation); //tabs names always shown
            RequestedOrientation = ScreenOrientation.Portrait;  //lock portrait screen orientation
        }

        //chose "Stopwatch" BottomNavigationView on start(navigation menu duplicate)
        private void SelectDefaultFragment()
        {
            selectedFragment = StopwatchFragment;
            nonSelectedFragment1 = AlarmFragment;
            nonSelectedFragment2 = SleepCyclesFragment;
            nonSelectedFragment3 = TimerFragment;
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, selectedFragment).Commit();
            SupportFragmentManager.BeginTransaction().Show(selectedFragment).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment1).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment2).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment3).Commit();
        }

        //navigation menu
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.nav_alarm:
                    selectedFragment = AlarmFragment;
                    nonSelectedFragment1 = SleepCyclesFragment;
                    nonSelectedFragment2 = StopwatchFragment;
                    nonSelectedFragment3 = TimerFragment;
                    break;

                case Resource.Id.nav_sleep_cycles:
                    selectedFragment = SleepCyclesFragment;
                    nonSelectedFragment1 = AlarmFragment;
                    nonSelectedFragment2 = StopwatchFragment;
                    nonSelectedFragment3 = TimerFragment;
                    break;

                case Resource.Id.nav_stopwatch:
                    selectedFragment = StopwatchFragment;
                    nonSelectedFragment1 = AlarmFragment;
                    nonSelectedFragment2 = SleepCyclesFragment;
                    nonSelectedFragment3 = TimerFragment;
                    break;

                case Resource.Id.nav_timer:
                    selectedFragment = TimerFragment;
                    nonSelectedFragment1 = AlarmFragment;
                    nonSelectedFragment2 = SleepCyclesFragment;
                    nonSelectedFragment3 = StopwatchFragment;
                    break;
            }
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, selectedFragment).Commit();
            SupportFragmentManager.BeginTransaction().Show(selectedFragment).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment1).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment2).Commit();
            SupportFragmentManager.BeginTransaction().Hide(nonSelectedFragment3).Commit();
            return true;
        }

        //tabs names always shown
        void RemoveShiftMode(BottomNavigationView view)
        {
            var menuView = (BottomNavigationMenuView)view.GetChildAt(0);
            try
            {
                var shiftingMode = menuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;

                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    item.SetShiftingMode(false);
                    // set checked value, so view will be updated
                    item.SetChecked(item.ItemData.IsChecked);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine((ex.InnerException ?? ex).Message);
            }
        }
    }
}