package com.example.currencyconverter;

import java.util.ArrayList;
import java.util.List;

public class MockCurrency implements CurrencyDAO {

    public MockCurrency() {
    }

    @Override
    public ArrayList<Rate> getRates(String base) {
        ArrayList<Rate> rates = new ArrayList<Rate>();
        rates.add(new Rate("DKK", 7.446592));
        rates.add(new Rate("USD", 1.184673));
        return rates;
    }
}
