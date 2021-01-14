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
    public abstract class DisplayCommand
    {
        internal int displayCommandType;
        internal string displayCommandSeparator = ":";
        internal string displayCommandEOF = "|";

        public DisplayCommand(int displayCommandType)
        {
            this.displayCommandType = displayCommandType;
        }

        public abstract string GetFormattetCommand();

        public DisplayCommand DisplayCommandFromString(string displayCommandString)
        {
            // TODO Handle exceptions
            displayCommandString = displayCommandString.Replace(displayCommandEOF, "");
            string[] displayComandFromStringSplit = displayCommandString.Split(displayCommandSeparator);
            switch (int.Parse(displayComandFromStringSplit[0]))
            {
                case 0:
                    return new UpdateCommand();
                    break;
                case 1:
                    return new DrawPixelCommand(int.Parse(displayComandFromStringSplit[1]), int.Parse(displayComandFromStringSplit[2]), int.Parse(displayComandFromStringSplit[3]), int.Parse(displayComandFromStringSplit[4]), int.Parse(displayComandFromStringSplit[5]));
                    break;
                default:
                    // TODO Define exceptions somwhere
                    throw new Exception("Command int out of range");
            }
        }
    }
}