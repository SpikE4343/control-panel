// status_lights.h

#ifndef _STATUS_LIGHTS_h
#define _STATUS_LIGHTS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

enum Status
{
  STATUS_RCS=0,
  STATUS_SAS=1,
  STATUS_STAGE=2,
  STATUS_STAGE_ARMED=3,
  STATUS_THROTTLE=4,
  
  NUM_STATUS = 8
};

typedef struct
{
  char pin;
  char state;
} StatusGroup;

void setup_status_groups();
void update_status_groups();

void handle_status_group_command();

void set_status_group_mapping( char status, char pin );


#endif




