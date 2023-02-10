
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// VGM Player written by
// !Shawty!/DS in 2017
// NOTE: This player is designed only to playback BBC Model B VGM Files

namespace VGMPlayer
{
  public class VgmFile
  {
    // This class is based on the 1.50 VGM File format

    private SerialSender _serialSender = new SerialSender();

    private VgmFileHeader _header = new VgmFileHeader();
    private byte[] _chipData;

    // The player process is quite simple.
    // All we care about are VGM Commands 0x61, 0x63, 0x50 & 0x66
    // 61 means set a delay, if we see one of these we set the delay counter to that value, then each time the player is called
    // we decrement that delay until it reaches 0, we do not allow the vgm data reader to progress to the next byte if this
    // delay counter is not 0.  Command 63 is a shortcut for a delay of 1/50th of a second
    // Command 50, means the next byte is to be sent directly to the sound chip, so thats exactly what we do :-)
    // Command 66, means end of song data, so we set the variable to show that it's looped, then reset the song data pointer to 0

    private int _dataPointer = 0; // Current position in our chip data array
    private int _delayCounter = 0; // Counts down to 0, and is set by a delay command in the chip data
    //private int _delayCounterTimer = 0; // Used to control how fast the delay counter counts down
    //private int _delayCounterTimerMax = 500; // Used to set the speed on the delay counter timer. Higher is slower
    // NOTE: You WILL need to fiddle with the above max value to get the correct playback speed, remember the PC you
    // run this in is likley to be orders of magnitude faster than the 16Mhz arduino recieving the data to send to
    // the sound chip, which itself is only clocked at 4Mhz.
    // The serial connection in the Arduino code is set at 115200, and the Arduino's inbound buffer is only 128 bytes
    // so timing will need to be adjusted!!!

    private byte barMax = 32; // Maximum size a bar value can be
    private byte barSpeed = 0; // Counter to track bar speed
    private byte barSpeedMax = 192; // How many itterations of the play loop before we reduce a bar value;

    // These 3 vars are public (That is they can be accessed outside the player class) and are read only.
    public bool SongLooping { get; private set; }
    public int DelayCounter { get { return _delayCounter; } }
    public byte LastByteSent { get; private set; }

    public byte Tone3Volume { get; private set; }
    public byte Tone2Volume { get; private set; }
    public byte Tone1Volume { get; private set; }
    public byte NoiseVolume { get; private set; }

    public byte Tone3Bar { get; private set; }
    public byte Tone2Bar { get; private set; }
    public byte Tone1Bar { get; private set; }

    public void Load(string fileName)
    {
      Stream fileStream = new FileStream(fileName, FileMode.Open);
      BinaryReader reader = new BinaryReader(fileStream);

      LoadHeader(reader);
      LoadChipData(reader);

      fileStream.Close();

      SongLooping = false;
    }

    public void PlayNext()
    {
      // Plays the next available entry in the chip data array, this is designed to be called repeatedly
      // from a loop outside the class, so each call of it will decode one command only.

      // Adjust our tone counters
      if (barSpeed == 0)
      {
        Tone3Bar--; if (Tone3Bar < 1) { Tone3Bar = 1; }; if (Tone3Bar > barMax) { Tone3Bar = barMax; }
        Tone2Bar--; if (Tone2Bar < 1) { Tone2Bar = 1; }; if (Tone2Bar > barMax) { Tone2Bar = barMax; }
        Tone1Bar--; if (Tone1Bar < 1) { Tone1Bar = 1; }; if (Tone1Bar > barMax) { Tone1Bar = barMax; }
        barSpeed = barSpeedMax;
      }
      else
      {
        barSpeed--;
      }

      // If we have a delay set
      if (_delayCounter > 0)
      {
        // THIS WAS THE OLD WAY OF DOING THE TIMING UNTIL I MANAGED TO USE A HIGH RES TIMER
        // IVE NOT TRIED THIS ON ANYTHING OTHER THAN WINDOWS 7 YET, SO MAY HAVE TO Go
        // BACK TO USING THIS ON OTHER PLATFORMS
        // Increment our delay timer 
        //_delayCounterTimer++;
        // And if it's hit our max.....
        //if(_delayCounterTimer > _delayCounterTimerMax)
        //{
          // Decrement the actual delay and reset the timer
          _delayCounter--;
          //_delayCounterTimer = 0;
        //}
        return;
      }

      // if our delayCounter is 0 then we get here, ready to decode the next command

      byte currentDataByte = _chipData[_dataPointer];

      // Now we decode our VGM chip data commands.
      // In the case of the SN76489 in the BBC not all the commands are used, so we decode
      // only the one we are interested in.

      switch (currentDataByte)
      {
        case 0x61:
          // Set a delay counter value, low byte is in the next byte, high in the one following that
          int delayVal = (_chipData[_dataPointer + 2] << 8) + _chipData[_dataPointer + 1];
          _delayCounter = delayVal;
          _dataPointer = _dataPointer + 3; // Increase pointer to next data entry

          //Console.WriteLine("Set Delay {0}", _delayCounter);
          break;

        case 0x63:
          // Short cut for a 1/50th of a second delay
          _delayCounter = 0x7203; // See VGM spec for these values (These are 20ms on a PAL System running at 4Mhz)
          _dataPointer++; // Increase pointer to next data entry;
