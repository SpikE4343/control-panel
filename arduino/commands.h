// commands.h

#ifndef _COMMANDS_h
#define _COMMANDS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

enum Commands
{
  CMD_GROUP_STATE   = 0,
  CMD_TELEMETRY     = 1,
  CMD_ANALOG_INPUTS = 2,
  CMD_ACTION        = 3,
  CMD_HEARTBEAT     = 4,
  CMD_ANALOG_TELEMETRY = 5,
  CMD_LOG           = 6,
  NUM_COMMANDS
};

void logMsg( const char* msg, int len );

typedef void (*command_handler)(void);

void register_command( int cmd, int size, command_handler handler );
void setup_commands();
void update_commands();

unsigned long now();
void setnow( unsigned long millis );
#endif

