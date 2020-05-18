
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

  // This is only used if you want to attempt to let the Arduino generat