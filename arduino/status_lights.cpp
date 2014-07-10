// 
// 
// 

#include "status_lights.h"
#include "commands.h"

static StatusGroup groups[NUM_STATUS];

void setup_status_groups()
{
  register_command( CMD_GROUP_STATE, 2, &handle_status_group_command );

  // status leds
  groups[STATUS_RCS]          = (StatusGroup){ 23, 1 }; // 0, 0
  groups[STATUS_SAS]          = (StatusGroup){ 25, 1 }; // 1, 0
  groups[STATUS_STAGE]        = (StatusGroup){ 27, 1 }; // 1, 2
  groups[STATUS_STAGE_ARMED]  = (StatusGroup){ 29, 1 }; // 1, 3
  groups[STATUS_THROTTLE]     = (StatusGroup){ 33, 1 }; // 0, 2
  groups[5]                   = (StatusGroup){ 35, 1 }; // 0, 3
  groups[6]                   = (StatusGroup){ 37, 1 }; // 0, 0
  groups[7]                   = (StatusGroup){ 39, 1 }; // 0, 1
     
  for( int p=0; p < NUM_STATUS; ++p )
  {
    pinMode(groups[p].pin, OUTPUT);
    digitalWrite( groups[p].pin, groups[p].state ? HIGH : LOW );
  }
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