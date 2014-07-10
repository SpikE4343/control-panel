
#if VS_BUILD
#ifndef __AVR_ATmega2560__
#define __AVR_ATmega2560__
#endif
#endif

#include <Arduino.h>

#include "commands.h"
#include <LedControl.h>
#include "status_lights.h"
#include "switch_controls.h"
#include "telemetry_displays.h"
#include "telemetry_analog_meter.h"

long throttle = 0;
#define MAX_SAMPLES 1
int samples[MAX_SAMPLES];
int sample = 0;
long avgn = 0;
long last_avgn = 0;
long last = 0;

unsigned long nextThrottleUpdate = 0;
unsigned long nextHeartbeat = 0;
long frame =0;

unsigned long ulNow = 0;
unsigned long now() { return ulNow; }
void setnow( unsigned long lnow ) { ulNow = lnow; }

void setup() 
{
  setnow( millis() );

  setup_status_groups();
  setup_commands();
  setup_telemetry();
  setup_switches();
  setup_analog_telemetry();
  

  memset(&samples[0], 0, sizeof(samples) );
 
  Serial.begin(9600);
  while( !Serial ){}
}

void send_heartbeat()
{
  ++frame;

  

  if( now() < nextHeartbeat)
    return;

  set_telemetry(9, 2, 0, 3, 0, Serial.available() );
  set_telemetry(8, 3, 0, 8, 0, frame );

  nextHeartbeat = now()+1000;
  
  uint8_t buffer[] = 
  { 
    'e',
    'R',
    CMD_HEARTBEAT,
    4
  };
    
  Serial.write( &buffer[0], sizeof(buffer) );
  Serial.write( (uint8_t*)&frame, sizeof(frame) );
}

void update_analog_inputs()
{
  if( now() < nextThrottleUpdate)
    return;

  nextThrottleUpdate = now()+100;

  int t = analogRead(A1);
  
  samples[sample] = t;

  //for( int i=0; i < MAX_SAMPLES; ++i )
  //  throttle += samples[i];

  // throttle /= MAX_SAMPLES;
  sample = (sample+1) % MAX_SAMPLES;
  last_avgn = avgn;
  avgn = last_avgn + t - (last_avgn/MAX_SAMPLES);
  
  throttle = avgn/(MAX_SAMPLES * 10 );
  //Serial.print( t );
  //Serial.print( "." );
  //Serial.println(sizeof(throttle));

  if( throttle <= 3 )
    throttle = 0;

  if( throttle >= 98 )
    throttle = 100;

  if( throttle != last )
  {
    //set_telemetry(2, throttle);
    last = throttle;
    float ft = throttle;
    uint8_t buffer[] = 
    { 
      'e',
      'R',
      CMD_ANALOG_INPUTS,
      5,
      0 
    };
    
    Serial.write( &buffer[0], sizeof(buffer) );
    Serial.write( (uint8_t*)&ft, sizeof(ft) );
  }
}



void loop()
{
  setnow(millis());
  send_heartbeat();

  update_commands();
  update_switches();
  update_status_groups();
  update_analog_inputs();
  update_telemetry();
  update_analog_telemetry();
  
  
  
}
