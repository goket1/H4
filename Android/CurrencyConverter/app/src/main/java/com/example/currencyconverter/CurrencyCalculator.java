package com.example.currencyconverter;

import android.content.Context;
import android.widget.ArrayAdapter;

import java.util.ArrayList;
import java.util.List;

public class CurrencyCalculator {

    private CurrencyDAO currencyDAO;

    public CurrencyCalculator(Context context) {
        currencyDAO = new FixerCurrency(context);
    }

    public ArrayList<Rate> ConvertCurrency(Rate rate, String base) throws Exception {
        ArrayList<Rate> convertedRates = new ArrayList<Rate>();
        for(Rate rateListRate : currencyDAO.getRates(base)) {
            convertedRates.add(new Rate(rateListRate.getName(), rateListRate.getSpotRate() * rate.getSpotRate()));
        }
        return convertedRates;
    }
}