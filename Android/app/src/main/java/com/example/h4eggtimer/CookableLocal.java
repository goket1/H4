package com.example.h4eggtimer;

public class CookableLocal implements Cookable {

    String name;
    int timeToCook;

    public CookableLocal(String name, int timeToCook){
        this.name = name;
        this.timeToCook = timeToCook;
    }

    @Override
    public String name() {
        return name;
    }

    @Override
    public int timeToCook() {
        return timeToCook;
    }
}
