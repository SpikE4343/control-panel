
#include <Arduino.h>
#include "analog_input.h"
#include "commands.h"

AnalogInput inputs[ANALOG_INPUTS];

uint8_t aiMsg[] = { 'e', 'R', CMD_ANALOG_INPUTS, 5, 0 };

void set_analog_input_mapping( char input, char pin )
{
  inputs[input] = (AnalogInput){pin, 0, 0, 0, 0, 0, 0, 0 };
}

void setup_analog_inputs()
{
  for( int i=0; i < ANALOG_INPUTS; ++i )
  {
    set_analog_input_mapping( i, i );
  }
}



void update_analog_input(char id)
{
  AnalogInput& input = inputs[id];

  int t = analogRead(input.pin);

  input.samples[input.sample] = t;

  input.sample = (input.sample+1) % MAX_SAMPLES;
  input.last_avgn = input.avgn;
  input.avgn = input.last_avgn + t - (input.last_avgn/MAX_SAMPLES);

  input.value = input.avgn/(MAX_SAMPLES * 10 );

  if( input.value <= 3 )
    input.value = 0;

  if( input.value >= 98 )
    input.value = 100;

  if( now() < input.nextUpdate)
    return;

  input.nextUpdate = now() + ANALOG_INPUT_UPDATE_MS;

  if( input.value != input.last )
  {
    input.last = input.value;
    float ft = input.value;

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
