
#include "Arduino.h"
#include "commands.h"
#include "telemetry_analog_meter.h"


int values[NUM_METERS];
bool changed[NUM_METERS];
int pins[NUM_METERS] = { 5, 6, 7, 8, 9, 10, 11 }; 
int pin = 2;

void setup_analog_telemetry()
{
  memset(values, 0, sizeof(int)*NUM_METERS);
  memset(changed, 0, sizeof(bool)*NUM_METERS);
 
  register_command( 
    CMD_ANALOG_TELEMETRY, 
    sizeof(byte)*2, 
    &handle_analog_telemetrycommand );

  for( int m = 0; m < NUM_METERS; ++m )
  {
    values[m] = 255;
    pinMode(pins[m], OUTPUT);
  }
}

void handle_analog_telemetrycommand()
{
  byte meter = Serial.read();
  byte value = Serial.read();

  if( meter < 0 || meter > NUM_METERS )
      return;

  values[meter] = value;
}

void update_analog_telemetry()
{
  for( int m = 0; m < NUM_METERS; ++m )
  {
    analogWrite(pins[m], values[m] );
  }
}
