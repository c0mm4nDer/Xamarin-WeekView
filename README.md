# Xamarin WeekView

This project is a binding for [Android WeekView Library](https://github.com/alamkanak/Android-Week-View) Version 1.2.6

Xamarin Android Week View is an android library to display calendars (week view or day view) within the app. It supports custom styling.

![First Page](https://raw.githubusercontent.com/c0mm4nDer/Xamarin-WeekView/master/Sample/ScreenShots/Screenshots.png)


## Features ##

 - Week view calendar
 - Day view calendar
 - Custom styling
 - Horizontal and vertical scrolling
 - Infinite horizontal scrolling
 - Live preview of custom styling in xml preview window

### Changelog ###
**Version 1.2.6**

* Add empty view click listener
* Fix padding bug
* Fix bug when setting colors of different components
* Add ability to turn off fling gesture
* Add example of how to load events asynchronously in the sample app

## Getting Started

1.  Import the BindingsWeekView.dll into your project Or add binding project to your Refereneces.
 
2. Add WeekView in your xml layout:

	 ```xml
    <com.alamkanak.weekview.WeekView
            android:id="@+id/weekView"
            android:layout_width="match_parent"
            android:layout_height="match_parent"
            app:eventTextColor="@android:color/white"
            app:textSize="12sp"
            app:hourHeight="60dp"
            app:headerColumnPadding="8dp"
            app:headerColumnTextColor="#8f000000"
            app:headerRowPadding="12dp"
            app:columnGap="8dp"
            app:noOfVisibleDays="3"
            app:headerRowBackgroundColor="#ffefefef"
            app:dayBackgroundColor="#05000000"
            app:todayBackgroundColor="#1848adff"
            app:headerColumnBackground="#ffffffff"/>
    ```
   
3. Write the following code:

```csharp
// Get a reference for the week view in the layout.
mWeekView = (WeekView)FindViewById(Resource.Id.weekView);

// Show a toast message about the touched event.
mWeekView.SetOnEventClickListener(this);

// The week view has infinite scrolling horizontally. We have to provide the events of a
// month every time the month changes on the week view.
mWeekView.MonthChangeListener = this;

// Set long press listener for events.
mWeekView.EventLongPress += (object sender, WeekView.EventLongPressEventArgs e) => { };
	
```
4. Implement WeekView.IEventClickListener, WeekView.IEventLongPressListener,
        MonthLoader.IMonthChangeListener according to your need.
        
        
5. Provide the events for the `WeekView` in `WeekView.IMonthChangeListener` OnMonthChange callback. Please remember that the calendar pre-loads events of three consecutive months to enable lag-free scrolling.

```csharp
public IList<WeekViewEvent> OnMonthChange(int newYear, int newMonth)
        {
            // Populate the week view with some events.
            List<WeekViewEvent> events = new List<WeekViewEvent>();
            return events;
        }
 ```
