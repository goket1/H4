package com.example.currencyconverter;

import android.app.Application;
import android.content.Context;

import com.android.volley.Cache;
import com.android.volley.Network;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.DiskBasedCache;
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
        volleyQueue = Volley.newRequestQueue(context);
    }

    public JSONObject get() {
        System.out.println("Api connector get called");
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest
                (Request.Method.GET, baseUrl, null,
                        response -> {
                            System.out.println("Got response from API: " + response.toString());

                        }, error -> {
                    System.out.println("Got error from API");
                });
        // Add the request to the RequestQueue.
        this.volleyQueue.add(jsonObjectRequest);
        return null;
    }

    public interface ApiReply {
        void apiReply(JSONObject reply);
    }
}
