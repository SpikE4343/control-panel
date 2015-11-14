#include <stdio.h> 
#include "switch_controls.h"
#include "commands.h"

#define switchPauseMs 100

static Switch switches[NUM_SWITCH];

void set_switch_mapping( char id, char pin )
{
  switches[id] = (Switch){pin, 0, 0 };
  pinMode(switches[id].pin, INPUT);
  switches[id].state = digitalRead(switches[id].pin );
}

void setup_switches()
{
  set_switch_mapping( SWITCH_THROTTLE_MOMENT,  28 );
  set_switch_mapping( SWITCH_THROTTLE_TOGGLE,  30 );
  set_switch_mapping( SWITCH_DOCKING_MODE,     32 );
  set_switch_mapping( SWITCH_RCS,              34 );
  set_switch_mapping( SWITCH_SAS,              36 );
  set_switch_mapping( SWITCH_MAP_MODE,         38 );
  set_switch_mapping( SWITCH_BRAKES,           40 );
  set_switch_mapping( SWITCH_GEAR,             42 );
  set_switch_mapping( SWITCH_TRANS_CTRL,       44 );
  set_switch_mapping( SWITCH_STAGE,            46 );
  set_switch_mapping( SWITCH_LIGHTS,           48 );
  set_switch_mapping( SWITCH_FINE_CTRL,        52 );
}

void update_switch( char id )
{
  Switch& sw = switches[id];
  if( sw.pin == 0 )
    return;

  // too soon
  if( (now() - sw.changeTime) < switchPauseMs )
    return;

  int state = digitalRead( sw.pin );

  // no change
  if( state == sw.state )
    return;

  sw.state = state;
  sw.changeTime = now();
  char gsMsg[] = { 'e', 'R', CMD_GROUP_STATE, 2, id, state };

  Serial.write((uint8_t*)gsMsg, sizeof(gsMsg));
}

void update_switches()
{
  for( int p=0; p < NUM_SWITCH; ++p )
  {
    update_switch(p);
  }
}

bool get_switch_state(int id )
{
  return switches[id].state == 1;
}




