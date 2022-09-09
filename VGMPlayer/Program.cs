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
 