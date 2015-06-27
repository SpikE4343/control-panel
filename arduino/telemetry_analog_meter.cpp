
#include "Arduino.h"
#include "commands.h"
#include "telemetry_analog_meter.h"


char values[NUM_METERS];
bool changed[NUM_METERS];
char pins[NUM_METERS] = { 5, 6, 7, 8, 9, 10, 11 }; 
char pin = 2;

void set_analog_meter_mapping( char meter, char pin )
{
  pins[meter] = pin;
  values[meter] = 0;
  pinMode(pins[meter], OUTPUT);
}

void setup_analog_telemetry()
{
  memset(values, 0, sizeof(int)*NUM_METERS);
  memset(changed, 0, sizeof(bool)*NUM_METERS);

  register_command( 
    CMD_ANALOG_TELEMETRY, 
    sizeof(byte)*2, 
    &handle_analog_telemetrycommand );

  set_analog_meter_mapping( 0, 5 );
  set_analog_meter_mapping( 1, 6 );
  set_analog_meter_mapping( 2, 7 );
  set_analog_meter_mapping( 3, 8 );
  set_analog_meter_mapping( 4, 9 );
  set_analog_meter_mapping( 5, 10 );
  set_analog_meter_mapping( 6, 11 );
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
