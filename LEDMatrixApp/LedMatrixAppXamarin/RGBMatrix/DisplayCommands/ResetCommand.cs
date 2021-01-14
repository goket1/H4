using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGBMatrix.DisplayCommands
{
    class ResetCommand : DisplayCommand
    {
        public ResetCommand() : base(2)
        {

        }

        public override string GetFormattetCommand()
        {
            return $"{displayCommandType}{displayCommandEOF}";
        }
    }
}