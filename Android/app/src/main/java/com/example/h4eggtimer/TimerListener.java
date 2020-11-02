package com.example.h4eggtimer;

public interface TimerListener {
    public void onCountDown(String timeLeftFormatted);
    public void onEggTimerStopped();
}
