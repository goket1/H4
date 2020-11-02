package com.example.h4eggtimer;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.os.Handler;
import android.widget.TextView;

import org.w3c.dom.Text;

public class MainActivity extends AppCompatActivity {

    private Handler handler = new Handler();
    public TimerManager timerManagerLocal = new TimerManagerLocal();
    TextView timerTextView;
    Button startStopButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        timerTextView = (TextView) findViewById(R.id.textViewTimer);
        startStopButton = (Button) findViewById(R.id.buttonStartStop);
    }

    private void toggleEggButtons(boolean enabled){
        findViewById(R.id.buttonSoftBoiled).setEnabled(enabled);
        findViewById(R.id.buttonMediumBoiled).setEnabled(enabled);
        findViewById(R.id.buttonHardBoiled).setEnabled(enabled);
    }

    public void onButtonEggSelectedClicked(View view){
        // An egg button has been pressed
        Log.d("Custom", String.format("An egg button with id: %s has been pressed", view.getId()));

        switch (view.getId()){
            case R.id.buttonSoftBoiled:
                Log.d("Custom", String.format("Soft boiled button pressed", view.getId()));
                timerManagerLocal.SetCookable(new CookableLocal("Soft boiled", 5 * 60));
                break;
            case R.id.buttonMediumBoiled:
                Log.d("Custom", String.format("Medium boiled button pressed", view.getId()));
                timerManagerLocal.SetCookable(new CookableLocal("Medium boiled", 8 * 60));
                break;
            case R.id.buttonHardBoiled:
                Log.d("Custom", String.format("Hard boiled button pressed", view.getId()));
                timerManagerLocal.SetCookable(new CookableLocal("Hard boiled", 10 * 60));
                break;
            default:
                throw new RuntimeException(String.format("Unknown button ID %s", view.getId()));
        }
        timerTextView.setText(timerManagerLocal.GetFormatedTime());
        findViewById(R.id.buttonStartStop).setEnabled(true);
    }

    public void StartTimer(){
        this.timerManagerLocal.StartTimer();
        this.toggleEggButtons(false);
        startStopButton.setText("Stop"); //set the text on Start / Stop button
        this.timer();
    }

    public void StopTimer(){
        this.timerManagerLocal.StopTimer();
        this.toggleEggButtons(true);
        handler.removeCallbacksAndMessages(null);
        startStopButton.setText("Start"); //set the text on Start / Stop button
    }

    public void onButtonStartStopClicked(View view){
        if(this.timerManagerLocal.GetRunning()){
            this.StopTimer();
        }else {
            this.StartTimer();
        }
    }

    public void timer(){
        this.handler.postDelayed(new Runnable(){
            @Override
            public void run() {
                timerManagerLocal.SubtractFromTime(1);
                timerTextView.setText(timerManagerLocal.GetFormatedTime());
                Log.d("Custom", String.format("Timer running value: %s formated: %s", timerManagerLocal.GetTimeLeft(), timerManagerLocal.GetFormatedTime()));
                if(timerManagerLocal.GetTimeLeft() <= 0) {
                    timerManagerLocal.SetRunning(false);
                    StopTimer();
                }else {
                    handler.postDelayed(this, 1000);
                }
            }
        },1000);
    }
}