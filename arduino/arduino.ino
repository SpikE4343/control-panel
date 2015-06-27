
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
#include "analog_input.h"

#define HEARTBEAT_SEND_MS 1000
#define BAUD  9600

long frame =0;

uint8_t hbMsg[] = { 'e','R', CMD_HEARTBEAT, 4 };

unsigned long nextHeartbeat = 0;
unsigned long ulNow = 0;
unsigned long now() { return ulNow; }
void setnow( unsigned long lnow ) { ulNow = lnow; }


void setup() 
{
  setnow( millis() );

  setup_status_groups();
  setup_telemetry();
  setup_switches();
  setup_analog_telemetry();

  Serial.begin(BAUD);
  while( !Serial ){}
}

void send_heartbeat()
{
  ++frame;

  if( now() < nextHeartbeat)
    return;

  nextHeartbeat = now()+HEARTBEAT_SEND_MS;

  Serial.write( &hbMsg[0], sizeof(hbMsg) );
  Serial.write( (uint8_t*)&frame, sizeof(frame) );
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
