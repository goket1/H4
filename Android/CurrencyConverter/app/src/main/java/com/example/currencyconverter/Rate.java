package com.example.currencyconverter;

public class Rate {
    private String name;

    public String getName() {
        return name;
    }

    public double getSpotRate() {
        return spotRate;
    }

    private double spotRate;

    public Rate(String name, double spotRate) {
        this.name = name;
        this.spotRate = spotRate;
    }
}
