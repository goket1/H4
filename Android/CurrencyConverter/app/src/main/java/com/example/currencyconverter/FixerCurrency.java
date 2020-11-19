package com.example.currencyconverter;

import android.content.Context;

import org.json.JSONObject;

import java.util.ArrayList;

public class FixerCurrency implements CurrencyDAO, ApiConnector.ApiReply{

    Context context;

    public FixerCurrency(Context context) {
        this.context = context;
    }

    @Override
    public ArrayList<Rate> getRates(String base) {
        ApiConnector apiConnector = new ApiConnector(this.context);
        JSONObject responseFromApi = apiConnector.get();
        System.out.println("Got something from the API: " + responseFromApi);
        return null;
    };

    @Override
    public void apiReply(JSONObject reply) {

    }
}
