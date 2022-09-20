﻿using System;
using System.Threading;

// VGM Player written by
// !Shawty!/DS in 2017
// NOTE: This player is designed only to playback BBC Model B VGM Files

namespace VGMPlayer
{
  class Program
  {
    static VgmFile vgmFile = new VgmFile();
    static MicroTimer timer;
    static bool displayRunning = false;

    static void DisplayThread()
    {
      Console.Clear();
      while (displayRunning)
      {
        Console.SetCursorPosition(0, 0);
        Console.Write("BBC Micro VGM Player (Serial Port Arduino Version)");

        Console.SetCursorPosition(0, 2);
        Console.Write("Delay {0}     ", vgmFile.DelayCounter);

        Console.SetCursorPosition(0, 3);
        Console.Write("Last Byte Sent: 0x{0:X}     ", vgmFile.LastByteSent);

        Console.SetCursorPosition(