
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
  // device on the board however, you can leave the next line commented and not bother connecting a wire
  //setupFourMhzTimer(); 

  // Set up the R/W pin
  pinMode(WE, OUTPUT);
  digitalWrite(WE, HIGH);

  // Set up the Data I/O pins
  pinMode(PIN_D0, OUTPUT);
  pinMode(PIN_D1, OUTPUT);
  pinMode(PIN_D2, OUTPUT);
  pinMode(PIN_D3, OUTPUT);
  pinMode(PIN_D4, OUTPUT);
  pinMode(PIN_D5, OUTPUT);
  pinMode(PIN_D6, OUTPUT);
  pinMode(PIN_D7, OUTPUT);

  // Set them all to logic 0
  digitalWrite(PIN_D0, LOW);
  digitalWrite(PIN_D1, LOW);
  digitalWrite(PIN_D2, LOW);
  digitalWrite(PIN_D3, LOW);
  digitalWrite(PIN_D4, LOW);
  digitalWrite(PIN_D5, LOW);
  digitalWrite(PIN_D6, LOW);
  digitalWrite(PIN_D7, LOW);

  // Initialise the SN76489
  InitiliseSoundChip();

  // and the inbound default serial port
  Serial.begin(115200);
}

void setupFourMhzTimer()
{
  // Iv'e no idea how this works :-)  I grabbed it off the arduino forums
  // I made an attempt to understand it, but every tweak I made I killed it!!!
  // it works, it gives 4Mhz, so I'm just leaving it at that.
  pinMode(CLOCK, OUTPUT);
  TCCR2A = ((1 << WGM21) | (1 << COM2A0)); //0x23;
  TCCR2B = (1 << CS20); //0x09;
  
  OCR2A  = 0x02;
  TIMSK2 = 0x00;
  
  OCR2B  = 0x01;
}

void InitiliseSoundChip()
{
  // Do the well known BOOO-BEEP BBC Initialisation sound.
  // Yes, it does actually serve a purpose :-)

  // The low tone that all BBCs play on startup (Before the higher beep all owners are used to) is actually
  // The default tone on channel 2 of the SN76489 sound chip.  It produces this to let you know that it's working
  // and ready to recieve commands, and starts to produce it as soon as power is applied to the device.

  // We allow the default tone to be heard for half a second or s