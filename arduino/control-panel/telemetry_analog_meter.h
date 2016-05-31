#ifndef _TELEMETRY_ANALOG_METER_h
#define _TELEMETRY_ANALOG_METER_h

enum MeterId
{
  METER_FUEL_LIQUID=0,
  METER_FUEL_OXIDIZER=1,
  METER_FUEL_MONO=2,
  METER_FUEL_ELECTRIC=3,
  METER_GEES=4,
  METER_PSI=5,
  METER_VERT_SPEED=6,

  NUM_METERS
};

void set_analog_meter_mapping( char meter, char pin );

void setup_analog_telemetry();
void handle_analog_telemetrycommand();
void update_analog_telemetry();

#endif



