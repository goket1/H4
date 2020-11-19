package com.example.currencyconverter;

import android.app.Application;
import android.content.Context;

import com.android.volley.Cache;
import com.android.volley.Network;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.BasicNetwork;
import com.android.volley.toolbox.DiskBasedCache;
import com.android.volley.toolbox.HurlStack;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONObject;

import java.util.ArrayList;

public class ApiConnector {

    private String baseUrl = "http://data.fixer.io/api/latest?access_key=c0238192f1d3ffdf4d5b75c6fab57547";
    private ArrayList<ApiReply> listeners = new ArrayList<>();
    private Context context;
    private RequestQueue volleyQueue;

    public ApiConnector(final Context context) {
        this.context = context;
        RequestQueue requestQueue;

    // Instantiate the cache
            Cache cache = new DiskBasedCache(context.getCacheDir(), 1024 * 1024); // 1MB cap

    // Set up the network to use HttpURLConnection as the HTTP client.
            Network network = new BasicNetwork(new HurlStack());

    // Instantiate the RequestQueue with the cache and network.
            requestQueue = new RequestQueue(cache, network);

    // Start the queue
            requestQueue.start();

    }

    public JSONObject get() {
        System.out.println("Api connector get called");


        String url ="http://www.example.com";

        // Formulate the request and handle the response.
        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        System.out.println("Got response from API: " + response.toString());
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        System.out.println("Got an error from the API: " + error.getMessage());
                    }
                });

        // Add the request to the RequestQueue.
        volleyQueue.add(stringRequest);
        return null;
    }

    public interface ApiReply {
        void apiReply(JSONObject reply);
    }
}
