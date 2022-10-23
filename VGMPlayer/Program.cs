using System;
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

        Console.SetCursorPosition(0, 4);
        Console.Write("Tone 3 Volume: {0}".PadRight(60, ' '), vgmFile.Tone3Volume);

        Console.SetCursorPosition(0, 5);
        Console.Write("Tone 2 Volume: {0}".PadRight(60, ' '), vgmFile.Tone2Volume);

        Console.SetCursorPosition(0, 6);
        Console.Write("Tone 1 Volume: {0}".PadRight(60, ' '), vgmFile.Tone1Volume);

        Console.SetCursorPosition(0, 7);
        Console.Write(" Noise Volume: {0}".PadRight(60, ' '), vgmFile.NoiseVolume);

        Console.SetCursorPosition(0, 9);
        Console.WriteLine("3 {0}".PadRight(60, ' '), new String('*', vgmFile.Tone3Bar));

        Console.SetCursorPosition(0, 10);
        Console.WriteLine("2 {0}".PadRight(60, ' '), new String('*', vgmFile.Tone2Bar));

        Con