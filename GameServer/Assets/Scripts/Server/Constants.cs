using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const int TICKS_PER_SEC = 60; // How many ticks per second
    public const float MS_PER_TICK = 1000f / TICKS_PER_SEC; // How many milliseconds per tick
    public const int MAX_PLAYERS = 50; // How many players can be connected at once

    public const float MAX_HEIGHT = 30f;
    public const float MIN_HEIGHT = 0f;
    public const float WATER_HEIGHT = 0.14f;
}
