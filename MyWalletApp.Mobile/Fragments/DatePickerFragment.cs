using System;
using Android.App;
using Android.OS;
using Android.Widget;
using static Android.App.DatePickerDialog;
using Android.Util;

namespace MyWalletApp.Mobile.Fragments
{
    public class DatePickerFragment : DialogFragment, IOnDateSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, month + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}