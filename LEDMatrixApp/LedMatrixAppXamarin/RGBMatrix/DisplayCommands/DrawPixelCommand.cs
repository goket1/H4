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
    public class DrawPixelCommand : DisplayCommand
    {
        private int x;
        private int y;
        private int r;
        private int g;
        private int b;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }

        public DrawPixelCommand(int x, int y, int r, int g, int b) : base(1)
        {
            this.X = x;
            this.Y = y;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public override string GetFormattetCommand()
        {
            return $"{displayCommandType}{displayCommandSeparator}{X}{displayCommandSeparator}{Y}{displayCommandSeparator}{R}{displayCommandSeparator}{G}{displayCommandSeparator}{B}{displayCommandEOF}";
        }
    }
}