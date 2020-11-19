package com.example.currencyconverter;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;

import java.lang.reflect.Array;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity implements CurrencyPresenter.View {

    private  CurrencyPresenter currencyPresenter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        currencyPresenter = new CurrencyPresenter(this, "EUR", this);
    }
    public void onButtonEggSelectedClicked(View view) {
        // Log that convert currency button has been clicked
        Log.d("Custom", String.format("Convert currency button with id: %s has been pressed", view.getId()));

        // Find the Currency amount edit text
        EditText editTextNumberDecimalConvertAmount = (EditText) findViewById(R.id.editTextNumberDecimalConvertAmount);

        try{
            CurrencyCalculated(currencyPresenter.CalculateCurrency(new Rate("USD", Double.parseDouble(editTextNumberDecimalConvertAmount.getText().toString()))));
        }catch (Exception e){
            // Catch any errors when converting and notify the user
            System.out.println("Error converting currency, exception: " + e.getMessage() + "Stack trace: ");
            e.printStackTrace();
        }
    }

    public void CurrencyCalculated(ArrayList<Rate> rateArrayList) {
        // Create var that we will use
        ListView mListView;
        RatesArrayAdapter aAdapter;
        // find converted currencies list
        mListView = (ListView) findViewById(R.id.currenciesList);
        // Create the rate array adapter
        aAdapter = new RatesArrayAdapter(this, rateArrayList);
        // Set the list adapter
        mListView.setAdapter(aAdapter);
    }
}