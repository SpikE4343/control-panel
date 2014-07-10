// telemetry_displays.h

#ifndef _TELEMETRY_DISPLAYS_h
#define _TELEMETRY_DISPLAYS_h

#include "LedControl.h"

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#define NUM_LED_DISPLAYS 5
#define MAX_TELEMETRY_ITEMS 10

// 22 22 24 24 26 26
// 24 26 22 26 22 24
// 26 24 26 22 24 22

//  9  9 10 10 11 11
// 10 11  9 11 10  9
// 11 10 11  9  9 10

// Led display connection pins
#define LED_CS_PIN    4
#define LED_CLOCK_PIN 3
#define LED_DATA_PIN  2


enum TelemetryDataType
{
  TDT_LONG = 0,
  TDT_ULONG = 1,
  TDT_FLOAT = 2
};

class TelemetryCmd
{
public:
  TelemetryCmd() 
    : precision(2)
    , startDigit(0)
    , maxDigits(8)
    , value(0)
    , display(255)
    , changed(true)
  {}
  byte display;
  byte precision;
  byte startDigit;
  byte maxDigits;
  long value;
  bool changed;

  void read( Stream& stream )
  {
    display = stream.read();
    precision = stream.read();
    startDigit = stream.read();
    maxDigits = stream.read();
    long ov = value;
    stream.readBytes( (char*)&value, 4 );
    changed = ov != value;
  }
};

void displayTelemetry( int id );
void set_telemetry(int id, int display, int start, int maxDigits, int precision, long value );

void setup_telemetry();
void handle_telemetry_command();
void update_telemetry();

#endif

