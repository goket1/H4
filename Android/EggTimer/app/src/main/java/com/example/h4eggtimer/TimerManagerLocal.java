package com.example.h4eggtimer;

import android.util.Log;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

public class TimerManagerLocal extends Thread implements TimerManager {

    boolean timerRunning = false;
    Cookable cookable;
    int timer = 0;
    List<TimerListener> timerListeners = new ArrayList<TimerListener>();

    @Override
    public void run() {
        Log.d("Custom", String.format("Timer started", GetTimeLeft(), GetFormatedTime()));
        while (GetTimeLeft() >= 0) {
            if (this.timerRunning) {
                SubtractFromTime(1);
                Log.d("Custom", String.format("Timer running value: %s formated: %s", GetTimeLeft(), GetFormatedTime()));
                for (TimerListener l : timerListeners) {
                    l.onCountDown(GetFormatedTime());
                }
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException e) {

                }
            }
        }
        SetRunning(false);
        StopTimer();
    }

    @Override
    public boolean TimerRunning() {
        return false;
    }

    @Override
    public Cookable cookable() {
        return null;
    }

    @Override
    public String GetFormatedTime() {
        if (timer == 0) {
            return "0:00";
        } else {
            int minutes = (int) Math.floor(this.timer / 60);
            return String.format("%s:%s", minutes, this.timer - (minutes * 60));
        }
    }

    @Override
    public int GetTimeLeft() {
        return timer;
    }

    @Override
    public boolean GetRunning() {
        return this.timerRunning;
    }

    @Override
    public int SubtractFromTime(int amount) {
        this.timer -= amount;
        return timer;
    }

    @Override
    public void SetCookable(Cookable cookable) {
        this.StopTimer();
        this.cookable = cookable;
        this.timer = cookable.timeToCook();
    }

    @Override
    public void SetRunning(boolean running) {
        this.timerRunning = running;
    }

    @Override
    public void StartTimer() {
        timerRunning = true;
        this.start();
    }

    @Override
    public void StopTimer() {
        timerRunning = false;
    }

    public void addListener(TimerListener listener) {
        timerListeners.add(listener);
    }

    public void removeListener(TimerListener listener) {
        timerListeners.remove(listener);
    }
}
