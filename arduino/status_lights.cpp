// 
// 
// 

#include "status_lights.h"
#include "commands.h"

static StatusGroup groups[NUM_STATUS];

void update_pin( char pin )
{
  digitalWrite( pin, groups[pin].state ? HIGH : LOW );
}

void set_status_group_mapping( char status, char pin )
{
  groups[status] =  (StatusGroup){ pin, 0 };
  pinMode(pin, OUTPUT);
  update_pin(pin);
}

void setup_status_groups()
{
  register_command( CMD_GROUP_STATE, 2, &handle_status_group_command );

  // status leds
  set_status_group_mapping( STATUS_RCS, 23 );
  set_status_group_mapping( STATUS_SAS, 39 );
  set_status_group_mapping( STATUS_STAGE, 27 );
  set_status_group_mapping( STATUS_STAGE_ARMED, 29 );
  set_status_group_mapping( STATUS_THROTTLE, 33 );
  set_status_group_mapping( 5, 35 );
  set_status_group_mapping( 6, 37 );
  set_status_group_mapping( 7, 39 );
}

void update_status_groups()
{
  //for( int p=0; p < NUM_STATUS; ++p )
  //{
  //  
  //}
}

void handle_status_group_command()
{
  int id = Serial.read();
  int state = Serial.read();

  if( id < 0 || id >= NUM_STATUS  )
    return;

  groups[id].state = state;
  digitalWrite( groups[id].pin, groups[id].state ? HIGH : LOW );
}



