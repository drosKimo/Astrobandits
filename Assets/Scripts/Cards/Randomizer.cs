using UnityEngine;

public class Randomizer
{
    System.Random rand;
    public int color()
    {
        rand = new System.Random();

        return rand.Next(0, 3);
    }
}