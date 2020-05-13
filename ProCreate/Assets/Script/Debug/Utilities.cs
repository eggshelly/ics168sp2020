using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    static Dictionary<Directions, Directions> ReverseDirections = new Dictionary<Directions, Directions>()
    {
        { Directions.forward, Directions.backwards },
        {Directions.backwards, Directions.forward },
        {Directions.left , Directions.right },
        {Directions.right , Directions.left },
        {Directions.f_left, Directions.b_right },
        {Directions.f_right, Directions.b_left },
        { Directions.b_left, Directions.f_right },
        {Directions.b_right, Directions.f_left },
        {Directions.neutral, Directions.neutral }
    };

    static List<char> Vowels = new List<char>{ 'a', 'e', 'i', 'o', 'u' };

    public static bool IsAVowel(char c)
    {
        return Vowels.Contains(c);
    }

    public static Directions GetReverseDirection(Directions dir)
    {
        return ReverseDirections[dir];
    }

    public static void RecursivelySetLayer(Transform trans, LayerMask newLayer)
    {
        trans.gameObject.layer = newLayer;

        foreach (Transform t in trans.transform)
        {
            RecursivelySetLayer(t, newLayer);
        }
    }

}
