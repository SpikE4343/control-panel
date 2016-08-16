
#include <Arduino.h>
#include "analog_input.h"
#include "commands.h"
#include "telemetry_displays.h"

AnalogInput inputs[ANALOG_INPUTS];

uint8_t aiMsg[] = { 'e', 'R', CMD_ANALOG_INPUTS, 5, 0 };

void set_analog_input_mapping( char input, char pin )
{
  inputs[input] = (AnalogInput){pin, 0, 0, 0, 0, 0, 0, 0 };
  memset( inputs[input].samples, 0, sizeof( int ) * MAX_SAMPLES); 
  //inputs[input].pin = pi;n
}

void setup_analog_inputs()
{
  for( int i=0; i < ANALOG_INPUTS; ++i )
  {
    set_analog_input_mapping( i, i+1 );
  }
}



void update_analog_input(int id)
{
  AnalogInput& input = inputs[id];

  int t = analogRead(input.pin);

  input.avgn = input.avgn + 0.25f* (t - input.avgn);
  input.value = (((input.avgn / 1024.0f) - 0.03f)/0.93f) * 100;

  //  set_telemetry( 9, 3, 4, 4, 0, input.pin);
  //  set_telemetry( 6, 3, 0, 4, 0, t);
  //  set_telemetry( 7, 4, 0, 4, 0, input.value);
  //  set_telemetry( 8, 4, 4, 4, 0, input.avgn);

  if( input.value >= 100 )
    input.value = 100;

  if( now() < input.nextUpdate)
    return;

  input.nextUpdate = now() + ANALOG_INPUT_UPDATE_MS;

  if( input.value != input.last )
  {
    input.last = input.value;
    float ft = input. value;

    aiMsg[4] = id;
    Serial.write( &aiMsg[0], sizeof(aiMsg) );
    Serial.write( (uint8_t*)&ft, sizeof(ft) );
  }
}

void update_analog_inputs()
{
  for( int i=0; i < ANALOG_INPUTS; ++i )
  {
    update_analog_input(i);
  }
}



