// 
// 
// 

#include "switch_controls.h"
#include "commands.h"

#define switchPauseMs 100
#define mapswitch(id, pin) switches[id] = (Switch){pin, 0, 0 }

static Switch switches[NUM_SWITCH];

void setup_switches()
{
  mapswitch( SWITCH_THROTTLE_TOGGLE,  30 );
  mapswitch( SWITCH_DOCKING_MODE,     32 );
  mapswitch( SWITCH_RCS,              34 );
  mapswitch( SWITCH_SAS,              36 );
  mapswitch( SWITCH_MAP_MODE,         38 );
  mapswitch( SWITCH_BRAKES,           40 );

  mapswitch( SWITCH_TRANS_CTRL,       44 );
  mapswitch( SWITCH_STAGE,            46 );
  mapswitch( SWITCH_LIGHTS,           48 );
  mapswitch( SWITCH_FINE_CTRL,        50 );
  mapswitch( SWITCH_THROTTLE_MOMENT,  28 );

  //mapswitch( SWITCH_STAGE_ARM,        30 );
  mapswitch( SWITCH_GEAR,             42 );

  for( int p=0; p < NUM_SWITCH; ++p )
  {
    pinMode(switches[p].pin, INPUT);
    switches[p].state = digitalRead(switches[p].pin );
  }
}
#include <stdio.h> 

//#define ARDUINO_DEBUG

void update_switches()
{
  for( int p=0; p < NUM_SWITCH; ++p )
  {
    if( switches[p].pin == 0 )
      continue;

    int state = digitalRead( switches[p].pin );

    if( state != switches[p].state && (now() - switches[p].changeTime) > switchPauseMs  )
    {
      switches[p].state = state;
      switches[p].changeTime = now();


      char buffer[] = { 'e', 'R', CMD_GROUP_STATE, 2, p, state };
      Serial.write((uint8_t*)buffer, sizeof(buffer));

      char msg[256];
      int len = sprintf(msg, "switch: %u, state: %u\n", switches[p].pin, state ); 
      logMsg( msg, len );
#ifdef ARDUINO_DEBUG
      Serial.print("[");
      Serial.print((int)switches[p].pin);
      Serial.print(",");
      Serial.print(state);
      Serial.println("]");
#endif
    }
  }

  //Serial.flush();
}

bool get_switch_state(int id )
{
  return switches[id].state == 1;
}

