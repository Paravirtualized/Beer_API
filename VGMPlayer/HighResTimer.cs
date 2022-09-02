
﻿using System;

// C# High Resolution Timer Class by Ken Loveday
// see: https://www.codeproject.com/Articles/98346/Microsecond-and-Millisecond-NET-Timer
// for more info.

namespace VGMPlayer
{
  /// <summary>
  /// MicroStopwatch class
  /// </summary>
  public class MicroStopwatch : System.Diagnostics.Stopwatch
  {
    readonly double _microSecPerTick =
        1000000D / System.Diagnostics.Stopwatch.Frequency;

    public MicroStopwatch()
    {
      if (!System.Diagnostics.Stopwatch.IsHighResolution)
      {
        throw new Exception("On this system the high-resolution " +
                            "performance counter is not available");
      }
    }

    public long ElapsedMicroseconds
    {
      get
      {
        return (long)(ElapsedTicks * _microSecPerTick);
      }
    }
  }

  /// <summary>
  /// MicroTimer class
  /// </summary>
  public class MicroTimer
  {
    public delegate void MicroTimerElapsedEventHandler(
                         object sender,
                         MicroTimerEventArgs timerEventArgs);
    public event MicroTimerElapsedEventHandler MicroTimerElapsed;

    System.Threading.Thread _threadTimer = null;
    long _ignoreEventIfLateBy = long.MaxValue;
    long _timerIntervalInMicroSec = 0;
    bool _stopTimer = true;

    public MicroTimer()
    {
    }

    public MicroTimer(long timerIntervalInMicroseconds)
    {
      Interval = timerIntervalInMicroseconds;
    }

    public long Interval
    {