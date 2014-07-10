// 
// 
// 

#include "telemetry_displays.h"
#include "commands.h"
#include <LedControl.h>
#include <assert.h>

static LedControl ledControl = LedControl(LED_DATA_PIN, 
                           LED_CLOCK_PIN, 
                           LED_CS_PIN, 
                           NUM_LED_DISPLAYS);

static TelemetryCmd cmds[MAX_TELEMETRY_ITEMS];
static bool telemetryChanged = true;

void setLong( TelemetryCmd& cmd )
{
  //Serial.println("setLong");
  int end = cmd.startDigit + cmd.maxDigits;
  unsigned long num = cmd.value;

  for( int d = cmd.startDigit; d < end; ++d )
  {
    

    int digit = num%10;
    bool decimalPoint = (d - cmd.startDigit) == cmd.precision;

    if( num <= 0 && d > cmd.startDigit)
    {
      ledControl.setChar(cmd.display, d, ' ', false);
    }
    else 
    {
      ledControl.setDigit(cmd.display, d, digit, decimalPoint);
    }

    num /= 10;
  }
}

void displayTelemetry( int id )
{
  if( id < 0 || id >= MAX_TELEMETRY_ITEMS )
    return;
    
  setLong( cmds[id] );
}

void setup_telemetry()
{
  // id = 1 byte
  // type = 1 byte
  // precision = 1 byte
  // number = 4 bytes

  //( sizeof(TelemetryCmd) == 6 );

  register_command( CMD_TELEMETRY, sizeof(TelemetryCmd)+1, &handle_telemetry_command );

  for( int d=0; d < NUM_LED_DISPLAYS; ++d )
  {
    ledControl.shutdown(d, false );
    ledControl.setIntensity(d,0);
    ledControl.clearDisplay(d);
    set_telemetry( d, d, 0, 8, 0, d);
  }
}

void handle_telemetry_command()
{  
  int id = Serial.read();

  if( id >= MAX_TELEMETRY_ITEMS )
  {
    //Serial.println( "invalid command" );
    char bf[256];
    Serial.readBytes( bf, 8 );
  }

  TelemetryCmd& cmd = cmds[id];
  cmd.read( Serial );
  telemetryChanged = true;
}

void set_telemetry(int id, int display, int start, int maxDigits, int precision, long value )
{  
  TelemetryCmd& cmd = cmds[id];
  cmd.display = display;
  cmd.precision = precision;
  cmd.maxDigits = maxDigits;
  cmd.startDigit = start;
  cmd.value = value;
  cmd.changed = true;
  telemetryChanged = true;
}

void update_telemetry()
{
  if( !telemetryChanged )
    return;

  telemetryChanged = false;
  for( int c = 0; c < MAX_TELEMETRY_ITEMS; ++c )
  {
    TelemetryCmd& cmd = cmds[c];
    if( cmd.display == 255 || !cmd.changed)
      continue;

    cmd.changed = false;
    setLong(cmd);
  }
}
