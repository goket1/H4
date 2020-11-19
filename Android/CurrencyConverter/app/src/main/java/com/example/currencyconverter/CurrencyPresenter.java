package com.example.currencyconverter;

import android.content.Context;

import org.json.JSONObject;

import java.util.ArrayList;

public class CurrencyPresenter {

    // Init vars
    private String base;
    private CurrencyCalculator currencyCalculator;

    View currensyPresenterView;

    public CurrencyPresenter(View view, String baseCurrency, Context context) {
        // Set the view
        this.currensyPresenterView = view;
        // Create the CurrencyCalculator
        this.currencyCalculator = new CurrencyCalculator(context);

        this.base = baseCurrency;
    }

    public ArrayList<Rate> CalculateCurrency(Rate rate) throws Exception {
        return currencyCalculator.ConvertCurrency(rate, this.base);
    }

    public void setBase(String base) {
        this.base = base;
    }

    public void apiReply(JSONObject reply) {
        System.out.println("Presenter got reply from API " + reply);
    }

    public interface View {
        void CurrencyCalculated(ArrayList<Rate> rateArrayList);
    }
}
