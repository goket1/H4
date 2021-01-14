//region includes
#include <Arduino.h>
// Needed ESPAsyncWiFiManager and WiFi server
#include <ESP8266WiFi.h>          //https://github.com/esp8266/Arduino
#include <ArduinoOTA.h>
// Needed for ESPAsyncWiFiManager
#include <ESPAsyncWebServer.h>     //Local WebServer used to serve the configuration portal
#include <ESPAsyncWiFiManager.h>          //https://github.com/tzapu/WiFiManager WiFi Configuration Magic
//endregion

//region WiFi server
WiFiServer wifiServer(5000);
//endregion

//region PxMatrix
// This is how many color levels the display shows - the more the slower the update
//#define PxMATRIX_COLOR_DEPTH 4

// Defines the speed of the SPI bus (reducing this may help if you experience noisy images)
//#define PxMATRIX_SPI_FREQUENCY 20000000

// Creates a second buffer for backround drawing (doubles the required RAM)
//#define double_buffer

#include <PxMatrix.h>

// Pins for LED MATRIX
#ifdef ESP32

#define P_LAT 22
#define P_A 19
#define P_B 23
#define P_C 18
#define P_D 5
#define P_E 15
#define P_OE 16
hw_timer_t * timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;

#endif

#ifdef ESP8266

#include <Ticker.h>

Ticker display_ticker;
#define P_LAT 16
#define P_A 5
#define P_B 4
#define P_C 15
#define P_D 12
#define P_E 0
#define P_OE 2

#endif

#define matrix_width 32
#define matrix_height 32

// This defines the 'on' time of the display is us. The larger this number,
// the brighter the display. If too large the ESP will crash
uint8_t display_draw_time = 30; //30-70 is usually fine

//PxMATRIX display(32,16,P_LAT, P_OE,P_A,P_B,P_C);
//PxMATRIX display(64,32,P_LAT, P_OE,P_A,P_B,P_C,P_D);
//PxMATRIX display(64,64,P_LAT, P_OE,P_A,P_B,P_C,P_D,P_E);

// Define the PxMATRIX object with the pins defined above, my panel doesn't use the the PD_E
PxMATRIX display(32, 32, P_LAT, P_OE, P_A, P_B, P_C, P_D);

#ifdef ESP8266

// ISR for display refresh
void display_updater() {
    display.display(display_draw_time);
}

#endif

#ifdef ESP32
void IRAM_ATTR display_updater(){
  // Increment the counter and set the time of ISR
  portENTER_CRITICAL_ISR(&timerMux);
  display.display(display_draw_time);
  portEXIT_CRITICAL_ISR(&timerMux);
}
#endif

void display_update_enable(bool is_enable) {

#ifdef ESP8266
    if (is_enable)
        display_ticker.attach(0.004, display_updater);
    else
        display_ticker.detach();
#endif

#ifdef ESP32
    if (is_enable)
  {
    timer = timerBegin(0, 80, true);
    timerAttachInterrupt(timer, &display_updater, true);
    timerAlarmWrite(timer, 4000, true);
    timerAlarmEnable(timer);
  }
  else
  {
    timerDetachInterrupt(timer);
    timerAlarmDisable(timer);
  }
#endif
}
//endregion

void setup() {
    //region serial
    Serial.begin(115200);
    Serial.println("Booting");
    //endregion

    //region WiFi server
    wifiServer.begin();
    //endregion

    //region ESPAsyncWiFiManager
    AsyncWebServer server(80);
    DNSServer dns;

    //first parameter is name of access point, second is the password
    AsyncWiFiManager wifiManager(&server, &dns);

    wifiManager.autoConnect("LedMatrixDisplay", "ILikeRGB");
    //endregion

    //region Over the air update
    ArduinoOTA.onStart([]() {
        String type;
        if (ArduinoOTA.getCommand() == U_FLASH) {
            type = "sketch";
        } else { // U_FS
            type = "filesystem";
        }

        // NOTE: if updating FS this would be the place to unmount FS using FS.end()
        Serial.println("Start updating " + type);
    });
    ArduinoOTA.onEnd([]() {
        Serial.println("\nEnd");
    });
    ArduinoOTA.onProgress([](unsigned int progress, unsigned int total) {
        Serial.printf("Progress: %u%%\r", (progress / (total / 100)));
    });
    ArduinoOTA.onError([](ota_error_t error) {
        Serial.printf("Error[%u]: ", error);
        if (error == OTA_AUTH_ERROR) {
            Serial.println("Auth Failed");
        } else if (error == OTA_BEGIN_ERROR) {
            Serial.println("Begin Failed");
        } else if (error == OTA_CONNECT_ERROR) {
            Serial.println("Connect Failed");
        } else if (error == OTA_RECEIVE_ERROR) {
            Serial.println("Receive Failed");
        } else if (error == OTA_END_ERROR) {
            Serial.println("End Failed");
        }
    });
    ArduinoOTA.begin();
    Serial.println("Ready");
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
    //endregion

    //region PxMatrix
    // Define your display layout here, e.g. 1/8 step, and optional SPI pins begin(row_pattern, CLK, MOSI, MISO, SS)
    display.begin(8);
    //display.begin(8, 14, 13, 12, 4);

    // Define multiplex implemention here {BINARY, STRAIGHT} (default is BINARY)
    //display.setMuxPattern(BINARY);

    // Set the multiplex pattern {LINE, ZIGZAG,ZZAGG, ZAGGIZ, WZAGZIG, VZAG, ZAGZIG} (default is LINE)
    display.setScanPattern(WZAGZIG);

    // Rotate display
    //display.setRotate(true);

    // Flip display
    //display.setFlip(true);

    // Control the minimum color values that result in an active pixel
    //display.setColorOffset(5, 5,5);

    // Set the multiplex implemention {BINARY, STRAIGHT} (default is BINARY)
    //display.setMuxPattern(BINARY);

    // Set the color order {RRGGBB, RRBBGG, GGRRBB, GGBBRR, BBRRGG, BBGGRR} (default is RRGGBB)
    display.setColorOrder(BBGGRR);

    // Set the time in microseconds that we pause after selecting each mux channel
    // (May help if some rows are missing / the mux chip is too slow)
    //display.setMuxDelay(0,1,0,0,0);

    // Set the number of panels that make up the display area width (default is 1)
    //display.setPanelsWidth(2);

    // Set the brightness of the panels (default is 255)
    display.setBrightness(50);

    // Set driver chip type
    //display.setDriverChip(FM6124);

    display_update_enable(true);

    display.fillScreen(display.color565(0, 0, 0));
    display.clearDisplay();
    display.showBuffer();
    //endregion
}

String splitStringGetIndexValue(String data, char separator, int index) {
    int found = 0;
    int strIndex[] = {0, -1};
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) {
        if (data.charAt(i) == separator || i == maxIndex) {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i + 1 : i;
        }
    }

    return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}


void loop() {
    //region Over the air update
    ArduinoOTA.handle();
    //endregion

    //region WiFi server
    WiFiClient client = wifiServer.available();

    if (client) {
        Serial.println("Client connected");
        while (client.connected()) {
            String packet = "";
            while (client.connected()) {
                while (client.available() > 0) {
                    char c = client.read();
                    if (c != '|') {
                        packet += c;
                    } else {
                        /*
                        Serial.println(packet);
                        Serial.println("0 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 0));
                        Serial.println("1 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 1));
                        Serial.println("2 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 2));
                        Serial.println("3 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 3));
                        Serial.println("4 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 4));
                        Serial.println("5 ");
                        Serial.println(splitStringGetIndexValue(packet, ':', 5));
                        */
                        switch (splitStringGetIndexValue(packet, ':', 0).toInt()) {
                            case 0:
                                Serial.println("Updating");
                                //region Over the air update
                                display.clearDisplay();
                                display.fillScreen(display.color565(25, 0, 0));
                                display.setCursor(0, 1);
                                display.print("updating...");
                                display.showBuffer();
                                while (true) {
                                    ArduinoOTA.handle();
                                }
                                //endregion
                            case 1:
                                Serial.println("Color pixel");
                                display.drawPixel(splitStringGetIndexValue(packet, ':', 1).toInt(),
                                                  splitStringGetIndexValue(packet, ':', 2).toInt(),
                                                  display.color565(splitStringGetIndexValue(packet, ':', 3).toInt(),
                                                                   splitStringGetIndexValue(packet, ':', 4).toInt(),
                                                                   splitStringGetIndexValue(packet, ':', 5).toInt()));
                                display.showBuffer();
                                break;
                            case 2:
                                display.clearDisplay();
                                display.fillScreen(display.color565(0, 0, 0));
                                display.showBuffer();

                            default:
                                // statements
                                break;
                        }
                        packet = "";
                    }
                    //Serial.print(c)
                }
            }
        }
        client.stop();
        Serial.println("Client disconnected");
    }
    //endregion

    //region PxMatrix

/*
    for (int i = 0; i < matrix_width; i++){
        for (int j = 0; j < matrix_height; j++){
            display.drawPixel(i, j, display.color565(255, 255, 255));
            display.showBuffer();
            delay(5);
        }
    }
    */
/*
    display.setTextWrap(false);

    display.fillScreen(display.color565(0, 0, 32));

    display.setTextColor(display.color565(255, 255, 255));
    display.setCursor(1, 1);
    display.print("Hello");
    display.setCursor(1, 11);
    display.print("World");

    display.showBuffer();

    delay(5000);
    display.clearDisplay();
    display.setTextColor(display.color565(0, 255, 0));

    display.setCursor(5,14);
    display.print("Test");
    display.showBuffer();
    delay(5000);*/
    //endregion
}