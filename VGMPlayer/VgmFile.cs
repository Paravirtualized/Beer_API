
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