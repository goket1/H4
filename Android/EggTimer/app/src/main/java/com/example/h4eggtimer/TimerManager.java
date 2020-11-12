package com.example.h4eggtimer;

public interface TimerManager {
    boolean TimerRunning();
    Cookable cookable();
    String GetFormatedTime();
    int GetTimeLeft();
    boolean GetRunning();
    void SetCookable(Cookable cookable);
    void SetRunning(boolean running);
    int SubtractFromTime(int amount);
    void StartTimer();
    void StopTimer();
}
