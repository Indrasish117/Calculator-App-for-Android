using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System;

namespace Calculator_
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private TextView calcText;
        private string[] numbers = new string[2];
        private string @operator;        
        private Plugin.SimpleAudioPlayer.ISimpleAudioPlayer audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            calcText = FindViewById<TextView>(Resource.Id.textView1);
            
            audio.Load("mouse_click_2.mp3");
            Erase();

        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            audio.Play();

            if ("0123456789.".Contains(button.Text))
                AddNumOrPoint(button.Text);
            else if ("÷×+-".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();
        }

        private void Erase()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalcText();
        }

        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch(@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }

            if (result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalcText();
            }
        }

        private void AddOperator(string value)
        {
            if(numbers[1] != null)
            {
                Calculate(value);
                return;
            }

            @operator = value;
            UpdateCalcText();
        }

        private void AddNumOrPoint(string value)
        {
            try
            {
                int index = @operator == null ? 0 : 1;

                if (value == "." && numbers[index].Contains("."))
                    return;

                numbers[index] += value;
                UpdateCalcText();
            }
            catch 
            {
                return;
            }
        }

        private void UpdateCalcText() => calcText.Text = $"{numbers[0]} {@operator} {numbers[1]}";
    }
}

