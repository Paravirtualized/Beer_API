
#define CLOCK  10
#define WE     2

#define PIN_D0 29
#define PIN_D1 28
#define PIN_D2 27
#define PIN_D3 26
#define PIN_D4 25
#define PIN_D5 24
#define PIN_D6 23
#define PIN_D7 22

int dataPtr = 0;
int delayCounter = 0;
int delayCounterTimer = 0;
int delayCounterTimerMax = 4;

void setup() {

  // This is only used if you want to attempt to let the Arduino generate the 4Mhz clock
  // that the SN76489 requires to operate.  NOTE: Beacuse this uses a custom PWM to generate it
  // It MUST use pin 10 as in the clock define above.  If you have a hardware 4mhz clock
  // device on the board however, you can leave the 