using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vect2
{
    public int XPos;
    public int YPos;

    public Vect2(int x, int y)
    {
        XPos = x;
        YPos = y;
    }

    public static Vect2 operator + (Vect2 v1, Vect2 v2)
    {
        if (v1 == null || v2 == null) throw new System.Exception("value is null");
        return new Vect2(v1.XPos + v2.XPos, v1.YPos + v2.YPos);
    }


    //public static bool operator == (Vect2 v1, Vect2 v2)
    //{
    //    return v1.XPos == v2.XPos && v1.YPos == v2.YPos;
    //}

    //public static bool operator != (Vect2 v1, Vect2 v2)
    //{
    //    return v1.XPos != v2.XPos || v1.YPos != v2.YPos;
    //}

    public Vect2 Clone()
    {
        return new Vect2(this.XPos, this.YPos);
    }

    public override bool Equals(object obj)
    {
        if (obj != null && obj is Vect2)
        {
            Vect2 v2 = (Vect2)obj;
            return this.XPos == v2.XPos && this.YPos == v2.YPos;
        }
        return false;
    }

}

public static class SomeClass
{
    public static bool OutOfRange(this bool[][] array, int x, int y)
    {
        bool ret = true;
        if (0 <= x && x < array.Length)
        {
            if (0 <= y && y < array[x].Length)
            {
                ret = false;
            }
        }
        return ret;
    }
}
