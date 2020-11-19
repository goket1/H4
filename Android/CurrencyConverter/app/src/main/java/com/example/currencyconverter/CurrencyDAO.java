package com.example.currencyconverter;

import java.util.ArrayList;
import java.util.List;

public interface CurrencyDAO {
    ArrayList<Rate> getRates(String base);
}
