package com.example.tennisbolden;

import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;

import android.graphics.Point;
import android.graphics.Rect;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.TranslateAnimation;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.List;

public class MainActivity extends AppCompatActivity {

    SensorManager sensorManager = null;
    TextView sensorTextView = null;
    ImageView tennisBallImageView = null;
    TranslateAnimation animation = null;

    List list;


    //region Vars for translating the ball position
    private int accelerationAmplification = 50;

    private float maxX = 0;
    private float maxY = 0;

    private float posX = 0;
    private float posY = 0;

    private float lastPosX = 0;
    private float lastPosY = 0;

    //endregion

    SensorEventListener sensorEventListener = new SensorEventListener() {
        public void onAccuracyChanged(Sensor sensor, int accuracy) {
        }

        public void onSensorChanged(SensorEvent event) {
            float[] values = event.values;
            MoveTennisBall(-values[0], -values[1]);
        }
    };

    // TODO Make it more generic?
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Get a SensorManager instance
        sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);

        // Find UI elements
        sensorTextView = (TextView) findViewById(R.id.sensorTextView);
        tennisBallImageView = (ImageView) findViewById(R.id.tennisBallImageView);

        // Find the accelerometer
        list = sensorManager.getSensorList(Sensor.TYPE_ACCELEROMETER);
        if (list.size() > 0) {
            // Accelerometer found
            sensorManager.registerListener(sensorEventListener, (Sensor) list.get(0), SensorManager.SENSOR_DELAY_NORMAL);
        } else {
            // No Accelerometer found
            Toast.makeText(getBaseContext(), "Error: No Accelerometer.", Toast.LENGTH_LONG).show();
        }
    }

    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        // region Find max width and height of the application
        // We do this in onWindowFocusChanged, because in onCreate the view has not et been initialised
        // Find the constraint layout to get it's size
        ConstraintLayout mainConstraint = (ConstraintLayout) findViewById(R.id.mainConstraint);

        // Get the size of the mainConstraint and minus it with the size of the ball
        maxY = mainConstraint.getHeight() - tennisBallImageView.getHeight();
        maxX = mainConstraint.getWidth() - tennisBallImageView.getWidth();

        // Set tennis ball position to the middle
        posX = maxX / 2;
        posY = maxY / 2;

        // Print the dimensions
        System.out.println("Found max\nX: " + maxX + "\nY: " + maxY);
        //endregion
    }

    private void MoveTennisBall(float x, float y) {
        // Find the new position
        posX += x * accelerationAmplification;
        posY -= y * accelerationAmplification;

        // Check that the position is not in the negatives, if it is, set it to zero so we don't exceed the screen size
        if (posX < 0) {
            posX = 0;
        // Check if the position is over max
        } else if (posX > maxX) {
            posX = maxX;
        }

        if (posY < 0) {
            posY = 0;
        } else if (posY > maxY) {
            posY = maxY;
        }

        if(animation == null){
            RunAnimation();
        }else if(animation.hasEnded()){
            RunAnimation();
        }



/*
        while(!animation.hasEnded()){

        }
        // Set the position
        tennisBallImageView.setX(posX);
        tennisBallImageView.setY(posY);*/

        // Display the position to the user with the textview
        sensorTextView.setText("x: " + posX + "\ny: " + posY);
    }

    private void RunAnimation(){
        // Set the last position for the animation
        float tempLastPosX = posX;
        float templastPosY = posY;
        animation = new TranslateAnimation(lastPosX, posX,
                lastPosY, posY);          //  new TranslateAnimation(xFrom,xTo, yFrom,yTo)
        animation.setDuration(20);         // animation duration
        animation.setFillAfter(true);
        animation.setAnimationListener(new TranslateAnimation.AnimationListener() {

            @Override
            public void onAnimationStart(Animation animation) {
            }

            @Override
            public void onAnimationEnd(Animation animation) {
                // Set the last position for the animation
                lastPosX = tempLastPosX;
                lastPosY = templastPosY;
            }

            @Override
            public void onAnimationRepeat(Animation animation) {

            }
        });
        // Start animation
        tennisBallImageView.startAnimation(animation);
    }


    @Override
    protected void onStop() {
        // Unregister the accelerometer if it's registered
        if (list.size() > 0) {
            sensorManager.unregisterListener(sensorEventListener);
        }
        super.onStop();
    }
}
