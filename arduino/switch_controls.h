// switch_controls.h

#ifndef _SWITCH_CONTROLS_h
#define _SWITCH_CONTROLS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

enum SwitchId
{
  SWITCH_RCS=0,
  SWITCH_SAS=1,
  SWITCH_STAGE=2,
  SWITCH_STAGE_ARM=3,
  SWITCH_DOCKING_MODE=4,
  SWITCH_MAP_MODE=5,
  SWITCH_THROTTLE_TOGGLE=6,
  SWITCH_BRAKES=7,
  SWITCH_GEAR=8,
  SWITCH_LIGHTS=9,
  SWITCH_TRANS_CTRL=10,
  SWITCH_FINE_CTRL=11,
  SWITCH_THROTTLE_MOMENT=12,
  NUM_SWITCH
};

typedef struct
{
  char pin;
  char state;
  unsigned long changeTime;
} Switch;

void set_switch_mapping( char id, char pin );
void setup_switches();
void update_switches();
bool get_switch_state(int id );

#endif




