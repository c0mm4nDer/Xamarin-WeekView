using Android.App;
using Android.Widget;
using Android.OS;
using Com.Alamkanak.Weekview;
using System;
using System.Collections.Generic;
using Android.Graphics;
using Java.Util;
using Java.Text;
using AndroidPersianCalendar;
using Android.Support.V7.App;
using Android.Views;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Util;

namespace Sample
{
    
    [Activity(Label = "TestWeekView", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : ActionBarActivity,
        WeekView.IEventClickListener, WeekView.IEventLongPressListener,
        MonthLoader.IMonthChangeListener
    {
        private SupportToolbar mToolbar;

        private static int TYPE_DAY_VIEW = 1;
        private static int TYPE_THREE_DAY_VIEW = 2;
        private static int TYPE_WEEK_VIEW = 3;
        private int mWeekViewType = TYPE_THREE_DAY_VIEW;
        private WeekView mWeekView;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            // Get a reference for the week view in the layout.
            mWeekView = (WeekView)FindViewById(Resource.Id.weekView);

            // Show a toast message about the touched event.
            mWeekView.SetOnEventClickListener(this);

            // The week view has infinite scrolling horizontally. We have to provide the events of a
            // month every time the month changes on the week view.
            mWeekView.MonthChangeListener = this;

            // Set long press listener for events.
            mWeekView.EventLongPress += (object sender, WeekView.EventLongPressEventArgs e) => { };

            // Set up a date time interpreter to interpret how the date and time will be formatted in
            // the week view. This is optional.
            setupDateTimeInterpreter(false);

            // Set an optional time range to display.
            mWeekView.SetMinTime(0);
            mWeekView.SetMaxTime(13);


            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "XamWeekView";

            mToolbar.MenuItemClick += MenuClickItem;

        }

        private void MenuClickItem(object sender, SupportToolbar.MenuItemClickEventArgs e)
        {
            setupDateTimeInterpreter(false);
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_today:
                    mWeekView.GoToToday();
                    break;
                case Resource.Id.action_dayView:
                    if (mWeekViewType != TYPE_DAY_VIEW)
                    {
                        e.Item.SetChecked(!e.Item.IsChecked);
                        mWeekViewType = TYPE_DAY_VIEW;
                        mWeekView.NumberOfVisibleDays=(1);

                        // Lets change some dimensions to best fit the view.
                        mWeekView.ColumnGap = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, Resources.DisplayMetrics);
                        mWeekView.TextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 12, Resources.DisplayMetrics);
                        mWeekView.EventTextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 12, Resources.DisplayMetrics);
                    }
                    break;
                case Resource.Id.action_threedays:
                    if (mWeekViewType != TYPE_THREE_DAY_VIEW)
                    {
                        e.Item.SetChecked(!e.Item.IsChecked);
                        mWeekViewType = TYPE_THREE_DAY_VIEW;
                        mWeekView.NumberOfVisibleDays=(3);

                        // Lets change some dimensions to best fit the view.
                        mWeekView.ColumnGap=(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, Resources.DisplayMetrics);
                        mWeekView.TextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 12, Resources.DisplayMetrics);
                        mWeekView.EventTextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 12, Resources.DisplayMetrics);
                    }
                    break;
                case Resource.Id.action_show_week:
                    if (mWeekViewType != TYPE_WEEK_VIEW)
                    {
                        setupDateTimeInterpreter(true);
                        e.Item.SetChecked(!e.Item.IsChecked);
                        mWeekViewType = TYPE_WEEK_VIEW;
                        mWeekView.NumberOfVisibleDays=(7);

                        // Lets change some dimensions to best fit the view.
                        mWeekView.ColumnGap=(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 2, Resources.DisplayMetrics);
                        mWeekView.TextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 10, Resources.DisplayMetrics);
                        mWeekView.EventTextSize=(int)TypedValue.ApplyDimension(ComplexUnitType.Sp, 10, Resources.DisplayMetrics);
                    }
                    break;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

       

        private void setupDateTimeInterpreter(bool shortDate)
        {
            var myDateTimeInterpreter = new MyDateTimeInterpreter(shortDate);
            mWeekView.DateTimeInterpreter = myDateTimeInterpreter;
        }

        class MyDateTimeInterpreter : Java.Lang.Object, IDateTimeInterpreter
        {
            bool mShortDate;
            public MyDateTimeInterpreter(bool shortDate)
            {
                mShortDate = shortDate;
            }

            public string InterpretDate(Calendar date)
            {
                int year = date.Get(CalendarField.Year);
                int month = date.Get(CalendarField.Month);
                int day = date.Get(CalendarField.DayOfMonth);
                SimpleDateFormat weekdayNameFormat = new SimpleDateFormat("EEE", Locale.Default);
                String weekday = weekdayNameFormat.Format(date.Time);
                SimpleDateFormat format = new SimpleDateFormat(" M/d", Locale.Default);

                String test = "";

                switch (weekday.ToLower())
                {
                    case "sat":
                        test = "شنبه";
                        break;
                    case "sun":
                        test = "یکشنبه";
                        break;
                    case "mon":
                        test = "دوشنبه";
                        break;
                    case "tue":
                        test = "سه شنبه";
                        break;
                    case "wed":
                        test = "چهارشنبه";
                        break;
                    case "thu":
                        test = "پنجشنبه";
                        break;
                    case "fri":
                        test = "جمعه";
                        break;
                }

                PersianCalendar persianCalendar = new PersianCalendar(year, month, day);

                // All android api level do not have a standard way of getting the first letter of
                // the week day name. Hence we get the first char programmatically.
                // Details: http://stackoverflow.com/questions/16959502/get-one-letter-abbreviation-of-week-day-of-a-date-in-java#answer-16959657
                if (mShortDate)
                {
                    weekday = Convert.ToString(weekday[0]);
                    return persianCalendar.getIranianDate();
                }
                else
                    return test.ToUpper() + " " + persianCalendar.getIranianDate();
            }

            public string InterpretTime(int hour, int minute)
            {
                return hour > 11 ? (hour - 12) + " PM" : (hour == 0 ? "12 AM" : hour + " AM");
            }
        }


        private String getEventTitle(Calendar time)
        {
            return String.Format("Event of {0}:{1} {2}/{3}", time.Get(CalendarField.HourOfDay), time.Get(CalendarField.Minute), time.Get(CalendarField.Month) + 1, time.Get(Calendar.DayOfMonth));
        }
        public void OnEventClick(WeekViewEvent e, RectF p1)
        {
            Toast.MakeText(this, "Clicked " + e.Name, ToastLength.Short).Show();
        }

        public void OnEventLongPress(WeekViewEvent e, RectF p1)
        {
            Toast.MakeText(this, "Long pressed event: " + e.Name, ToastLength.Short).Show();
        }

        public IList<WeekViewEvent> OnMonthChange(int newYear, int newMonth)
        {
            // Populate the week view with some events.
            List<WeekViewEvent> events = new List<WeekViewEvent>();

            Calendar startTime = Calendar.Instance;
            startTime.Set(CalendarField.HourOfDay, 3);
            startTime.Set(CalendarField.Minute, 0);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            Calendar endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.Hour, 1);
            endTime.Set(CalendarField.Month, newMonth - 1);
            WeekViewEvent _event = new WeekViewEvent(1, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_01);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.HourOfDay, 3);
            startTime.Set(CalendarField.Minute, 30);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Set(CalendarField.HourOfDay, 4);
            endTime.Set(CalendarField.Minute, 30);
            endTime.Set(CalendarField.Month, newMonth - 1);
            _event = new WeekViewEvent(10, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_02);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.HourOfDay, 4);
            startTime.Set(CalendarField.Minute, 20);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Set(CalendarField.HourOfDay, 5);
            endTime.Set(CalendarField.Minute, 0);
            _event = new WeekViewEvent(10, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_03);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.HourOfDay, 5);
            startTime.Set(CalendarField.Minute, 30);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.HourOfDay, 2);
            endTime.Set(CalendarField.Month, newMonth - 1);
            _event = new WeekViewEvent(2, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_04);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.HourOfDay, 5);
            startTime.Set(CalendarField.Minute, 0);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            startTime.Add(CalendarField.Date, 1);
            endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.HourOfDay, 3);
            endTime.Set(CalendarField.Month, newMonth - 1);
            _event = new WeekViewEvent(3, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_01);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.DayOfMonth, 15);
            startTime.Set(CalendarField.HourOfDay, 3);
            startTime.Set(CalendarField.Minute, 0);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.HourOfDay, 3);
            _event = new WeekViewEvent(4, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_02);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.DayOfMonth, 1);
            startTime.Set(CalendarField.HourOfDay, 3);
            startTime.Set(CalendarField.Minute, 0);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.HourOfDay, 3);
            _event = new WeekViewEvent(5, getEventTitle(startTime), startTime, endTime);
            _event.Color = Resources.GetColor(Resource.Color.event_color_03);
            events.Add(_event);

            startTime = Calendar.Instance;
            startTime.Set(CalendarField.DayOfMonth, startTime.GetActualMaximum(Calendar.DayOfMonth));
            startTime.Set(CalendarField.HourOfDay, 15);
            startTime.Set(CalendarField.Minute, 0);
            startTime.Set(CalendarField.Month, newMonth - 1);
            startTime.Set(CalendarField.Year, newYear);
            endTime = (Calendar)startTime.Clone();
            endTime.Add(CalendarField.HourOfDay, 3);
            _event = new WeekViewEvent(5, getEventTitle(startTime), startTime, endTime);
            _event.Color = Android.Graphics.Color.ParseColor("#59dbe0");
            events.Add(_event);

            return events;
        }


    }

}

