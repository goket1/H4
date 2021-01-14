using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Net.Sockets;
using System.Diagnostics;
using static Android.Renderscripts.ScriptGroup;
using System.Threading.Tasks;
using Xamarin.Essentials;
using RGBMatrix.DisplayCommands;
using System.Collections.Generic;

namespace RGBMatrix
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        EditText editTextDisplayHost;
        EditText editTextDisplayHostPort;
        EditText editTextDisplayCommand;
        RelativeLayout relativeLayoutColorBox;
        SeekBar seekBarRed;
        SeekBar seekBarGreen;
        SeekBar seekBarBlue;
        Button buttonReset;
        GridLayout gridLayoutMatrix;

        DisplayCommand displayCommand;
        ConnectionManager connectionManager;
        Dictionary<RelativeLayout, int[]> matrixCordinateSystem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            editTextDisplayHost = FindViewById<EditText>(Resource.Id.editTextDisplayHost);
            editTextDisplayHostPort = FindViewById<EditText>(Resource.Id.editTextDisplayHostPort);
            editTextDisplayCommand = FindViewById<EditText>(Resource.Id.editTextDisplayCommand);
            relativeLayoutColorBox = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutColorBox);
            seekBarRed = FindViewById<SeekBar>(Resource.Id.seekBarRed);
            seekBarGreen = FindViewById<SeekBar>(Resource.Id.seekBarGreen);
            seekBarBlue = FindViewById<SeekBar>(Resource.Id.seekBarBlue);
            buttonReset = FindViewById<Button>(Resource.Id.buttonReset);

            gridLayoutMatrix = FindViewById<GridLayout>(Resource.Id.gridLayoutMatrix);

            displayCommand = new DrawPixelCommand(1, 1, 255, 255, 255);
            connectionManager = new ConnectionManager(editTextDisplayHost.Text, int.Parse(editTextDisplayHostPort.Text));
            connectionManager.Connect();

            PopulateMatrix(32, 32);

            buttonReset.Click += async (sender, e) =>
            {
                Reset();
            };

            editTextDisplayCommand.TextChanged += async (sender, e) =>
            {
                displayCommand = displayCommand.DisplayCommandFromString(editTextDisplayCommand.Text);
                UpdateSeekBars();
            };

            seekBarRed.ProgressChanged += async (sender, e) =>
            {
                ((DrawPixelCommand) displayCommand).R = seekBarRed.Progress;
                UpdateDisplayCommandEditTextView();
                UpdateRelativeLayoutColorBox();
            };

            seekBarGreen.ProgressChanged += async (sender, e) =>
            {
                ((DrawPixelCommand)displayCommand).G = seekBarGreen.Progress;
                UpdateDisplayCommandEditTextView();
                UpdateRelativeLayoutColorBox();
            };

            seekBarBlue.ProgressChanged += async (sender, e) =>
            {
                ((DrawPixelCommand)displayCommand).B = seekBarBlue.Progress;
                UpdateDisplayCommandEditTextView();
                UpdateRelativeLayoutColorBox();
            };
        }

        public void PopulateMatrix(int totalColumns, int totalRows)
        {
            gridLayoutMatrix.ColumnCount = totalColumns;
            gridLayoutMatrix.RowCount = totalRows;
            //gridLayoutMatrix.UseDefaultMargins = true;

            var matrixCordinateSystem = new Dictionary<RelativeLayout, int[]>() { };

            for (int i = 0; i < totalRows; i++)
            {
                for (int j = 0; j < totalRows; j++)
                {
                    RelativeLayout relativeLayout = new RelativeLayout(BaseContext);
                    relativeLayout.SetMinimumWidth(40);
                    relativeLayout.SetMinimumHeight(40);
                    relativeLayout.Background = new Android.Graphics.Drawables.ColorDrawable(new Android.Graphics.Color(0, 0, 0, 255));

                    relativeLayout.Touch += async (sender, e) =>
                    {
                        ((RelativeLayout)relativeLayout).Background = new Android.Graphics.Drawables.ColorDrawable(new Android.Graphics.Color(seekBarRed.Progress, seekBarGreen.Progress, seekBarBlue.Progress, 255));
                        //System.Diagnostics.Debug.WriteLine($"CORDINATES: {matrixCordinateSystem[((RelativeLayout)sender)][0]}, {matrixCordinateSystem[((RelativeLayout)sender)][1]}|");
                        ((DrawPixelCommand)displayCommand).X = matrixCordinateSystem[((RelativeLayout)relativeLayout)][1];
                        ((DrawPixelCommand)displayCommand).Y = matrixCordinateSystem[((RelativeLayout)relativeLayout)][0];
                        UpdateDisplay(displayCommand);
                        UpdateDisplayCommandEditTextView();
                    };

                    matrixCordinateSystem.Add(relativeLayout, new int[] { i, j });

                    gridLayoutMatrix.AddView(relativeLayout);
                }
            }
        }

        public void Reset()
        {
            connectionManager.SendData(new ResetCommand().GetFormattetCommand());
            gridLayoutMatrix.RemoveAllViews();
            PopulateMatrix(32, 32);
        }

        public void UpdateSeekBars()
        {
            seekBarRed.Progress = ((DrawPixelCommand)displayCommand).R;
            seekBarGreen.Progress = ((DrawPixelCommand)displayCommand).G;
            seekBarBlue.Progress = ((DrawPixelCommand)displayCommand).B;
        }

        public void SeekBarsChanged()
        {
            UpdateDisplayCommandEditTextView();
            UpdateRelativeLayoutColorBox();
        }

        public void UpdateRelativeLayoutColorBox()
        {
            Android.Graphics.Drawables.Drawable color = new Android.Graphics.Drawables.ColorDrawable(new Android.Graphics.Color(seekBarRed.Progress, seekBarGreen.Progress, seekBarBlue.Progress, 255));
            relativeLayoutColorBox.Background = color;
        }

        public void UpdateDisplayCommandEditTextView()
        {
            editTextDisplayCommand.Text = displayCommand.GetFormattetCommand();
        }

        public void UpdateDisplay(DisplayCommand displayCommand)
        {
            System.Diagnostics.Debug.WriteLine($"Updating display with the following command: \"{displayCommand.GetFormattetCommand()}\"");
            Task.Run(() => connectionManager.SendData(displayCommand.GetFormattetCommand()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}