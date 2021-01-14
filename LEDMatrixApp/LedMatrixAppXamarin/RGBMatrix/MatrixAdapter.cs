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

namespace RGBMatrix
{
    class MatrixAdapter : BaseAdapter
    {
        Context context;

        public MatrixAdapter(Context c)
        {
            context = c;
        }

        public override int Count
        {
            get { return 1024; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        // create a new ImageView for each item referenced by the Adapter
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            RelativeLayout relativeLayout;

            if (convertView == null)
            {  // if it's not recycled, initialize some attributes
                relativeLayout = new RelativeLayout(context);
                relativeLayout.LayoutParameters = new GridView.LayoutParams(10, 10);
                relativeLayout.SetPadding(1, 1, 1, 1);
                relativeLayout.Background = new Android.Graphics.Drawables.ColorDrawable(new Android.Graphics.Color(255, 0, 0, 255));

            }
            else
            {
                relativeLayout = (RelativeLayout) convertView;
            }

            return relativeLayout;
        }
    }
}