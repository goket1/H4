package com.example.h4eggtimer;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import org.w3c.dom.Text;

public class MainActivity extends AppCompatActivity {

    public enum BoilLevel {
        SOFT,
        MEDIUM,
        HARD
    }

    public BoilLevel boilLevel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void onButtonEggSelectedClicked(View view){
        // An egg button has been pressed
        Log.d("Custom", String.format("An egg button with id: %s has been pressed", view.getId()));

        TextView timerTextView = (TextView) findViewById(R.id.textViewTimer);

        switch (view.getId()){
            case R.id.buttonSoftBoiled:
                Log.d("Custom", String.format("Soft boiled button pressed", view.getId()));
                this.boilLevel = BoilLevel.SOFT;
                timerTextView.setText("5:00");
                break;
            case R.id.buttonMediumBoiled:
                Log.d("Custom", String.format("Medium boiled button pressed", view.getId()));
                this.boilLevel = BoilLevel.MEDIUM;
                timerTextView.setText("8:00");
                break;
            case R.id.buttonHardBoiled:
                Log.d("Custom", String.format("Hard boiled button pressed", view.getId()));
                this.boilLevel = BoilLevel.HARD;
                timerTextView.setText("10:00");
                break;
            default:
                throw new RuntimeException(String.format("Unknown button ID %s", view.getId()));
        }
        findViewById(R.id.buttonStart).setEnabled(true);
    }
}