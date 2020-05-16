using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    static Dictionary<Directions, Directions> ReverseDirection = new Dictionary<Directions, Directions>()
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

    static Dictionary<Directions, List<Directions>> ReverseDirections = new Dictionary<Directions, List<Directions>>()
    {
        { Directions.forward, new List<Directions>{Directions.backwards } },
        {Directions.backwards, new List<Directions>{Directions.forward } },
        {Directions.left , new List<Directions>{Directions.right } },
        {Directions.right , new List<Directions>{Directions.left } },
        {Directions.f_left, new List<Directions>{Directions.backwards, Directions.right, Directions.b_right }},
        {Directions.f_right, new List<Directions>{Directions.backwards, Directions.left, Directions.b_left }},
        { Directions.b_left, new List<Directions>{Directions.forward, Directions.right, Directions.f_right } },
        {Directions.b_right, new List<Directions>{Directions.forward, Directions.left, Directions.f_left}  },
        {Directions.neutral,new List<Directions>{Directions.neutral} }
    };

    static List<char> Vowels = new List<char>{ 'a', 'e', 'i', 'o', 'u' };

    public static bool IsAVowel(char c)
    {
        return Vowels.Contains(c);
    }

    public static Directions GetReverseDirection(Directions dir)
    {
        return ReverseDirection[dir];
    }

    public static bool IsAReverseDirection(Directions objDir1, Directions objDir2)
    {
        return ReverseDirections[objDir2].Contains(objDir1);
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
