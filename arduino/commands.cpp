// 
// 
// 

#include "commands.h"
#include "status_lights.h"
#include "telemetry_displays.h"
#include <assert.h>

typedef struct
{
  int size;
  command_handler handler;
} Command;

static Command Commands[NUM_COMMANDS]; 

void register_command( int cmd, int size, command_handler handler )
{
  Commands[ cmd ] = (Command){ size, handler };
}

void setup_commands()
{

}

struct Header
{
  char msgId[2];
  byte type;
  byte size;
};

Header pendingHeader;
bool hasPendingMessage = false;

long lastCheck = 0;
void update_commands()
{ 
  int count = 0;
  while( Serial.available() >= 4  && count < 5)
  {
    if( !hasPendingMessage )
    {
      if( Serial.available() < 4 )
        continue;

      pendingHeader.msgId[0] = Serial.read();
      ++count;
      if( pendingHeader.msgId[0] != 'e' )
      {
        continue;
      }

      pendingHeader.msgId[1] = Serial.read();
      if( pendingHeader.msgId[1] != 'R' )
      {
        continue;
      }

      pendingHeader.type = Serial.read();
      pendingHeader.size = Serial.read();
      hasPendingMessage = true;
    }

    int avail = Serial.available();
    if( hasPendingMessage && avail >= pendingHeader.size )
    {
      ++count;
      hasPendingMessage = false;
      int command = pendingHeader.type;
      if( command < 0 || command >= NUM_COMMANDS )
      {
        continue;
      }

      command_handler handler = Commands[command].handler;
      if( handler == NULL )
      {
        char b[512];
        Serial.readBytes(b, pendingHeader.size);
        /*
        Serial.write( pendingHeader.msgId[0] );
        Serial.write( pendingHeader.msgId[1] );
        Serial.write( pendingHeader.type);
        Serial.write ( pendingHeader.size );
        Serial.write((uint8_t*)b, pendingHeader.size);
        */
        continue;
      }

      handler();

      int diff = avail - Serial.available();
      if( (diff) != pendingHeader.size )
      {
        /*
        Serial.print(command);
        Serial.print(" diff != size: ");
        Serial.print( diff );
        Serial.print(" != ");
        Serial.println(pendingHeader.size);
        */
      }
    }
  }
}

#include <string.h>

void logMsg( const char* msg, int len )
{
  return;
  if( !Serial )
    return;

  char buffer[] = { 'e', 'R', CMD_LOG, (byte)len };
  Serial.write((uint8_t*)buffer, sizeof(buffer));
  Serial.write((uint8_t*)msg,len);
}


