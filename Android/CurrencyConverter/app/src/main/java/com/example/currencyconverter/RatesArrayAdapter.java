package com.example.currencyconverter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.ArrayList;

public class RatesArrayAdapter extends ArrayAdapter<Rate> {
    public RatesArrayAdapter(Context context, ArrayList<Rate> users) {
        super(context, 0, users);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // Get the data item for this position

        Rate rate = getItem(position);
        // Check if an existing view is being reused, otherwise inflate the view

        if (convertView == null) {
            convertView = LayoutInflater.from(getContext()).inflate(R.layout.currencies_adapter_view_layout, parent, false);
        }
        // Lookup view for data population

        TextView tvName = (TextView) convertView.findViewById(R.id.textViewName);
        TextView tvRate = (TextView) convertView.findViewById(R.id.textViewRate);
        // Populate the data into the template view using the data object

        tvName.setText(rate.getName());
        tvRate.setText(String.valueOf(rate.getSpotRate()));
        // Return the completed view to render on screen

        return convertView;
    }
}