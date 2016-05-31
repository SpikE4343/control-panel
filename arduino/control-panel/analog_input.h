#ifndef analog_input_INCLUDED
#define analog_input_INCLUDED

#define MAX_SAMPLES 1
#define ANALOG_INPUTS 1
#define ANALOG_INPUT_UPDATE_MS 100


typedef struct 
{
  char pin;
  long value;
  int samples[MAX_SAMPLES];
  int sample;
  long avgn;
  long last_avgn;
  long last;
  long nextUpdate;
} AnalogInput;

void set_analog_input_mapping( char input, char pin );
void update_analog_inputs();
void setup_analog_inputs();
#endif



