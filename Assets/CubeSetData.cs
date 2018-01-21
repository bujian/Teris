using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSetData
{
    static CubeSetData _instance;
    public static CubeSetData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CubeSetData();
            }
            return _instance;
        }
    }

    private CubeSetData()
    {
        Colors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.blue,
            Color.cyan,
            Color.magenta,
            Color.black,
            Color.white,
        };
        

    }

    public Color[] Colors;

    public List<Vect2[]>[] Data = new List<Vect2[]>[]
    {
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,2), new Vect2(1,2),  new Vect2(1,1),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(1,0), new Vect2(1,1),  new Vect2(2,1),  new Vect2(2,2) },
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(1,0),  new Vect2(2,0) },
             new Vect2[] {  new Vect2(0,0), new Vect2(0,1),  new Vect2(1,1),  new Vect2(1,2) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(1,2),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(1,0), new Vect2(1,1),  new Vect2(1,2),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(1,0),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(1,0),  new Vect2(1,2) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(1,2),  new Vect2(2,2) },
             new Vect2[] {  new Vect2(1,1), new Vect2(1,2),  new Vect2(2,1),  new Vect2(2,0) },
             new Vect2[] {  new Vect2(0,0), new Vect2(1,0),  new Vect2(1,1),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(0,2), new Vect2(0,1),  new Vect2(1,1),  new Vect2(1,0) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(1,1), new Vect2(1,2),  new Vect2(2,1),  new Vect2(2,2) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(2,1),  new Vect2(2,2) },
             new Vect2[] {  new Vect2(1,0), new Vect2(1,1),  new Vect2(1,2),  new Vect2(2,0) },
             new Vect2[] {  new Vect2(0,0), new Vect2(0,1),  new Vect2(1,1),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(0,2), new Vect2(1,2),  new Vect2(1,1),  new Vect2(1,0) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,2), new Vect2(0,1),  new Vect2(1,1),  new Vect2(2,1) },
             new Vect2[] {  new Vect2(2,2), new Vect2(1,2),  new Vect2(1,1),  new Vect2(1,0) },
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(2,1),  new Vect2(2,0) },
             new Vect2[] {  new Vect2(0,0), new Vect2(1,0),  new Vect2(1,1),  new Vect2(1,2) },
        },
        new List<Vect2[]>
        {
             new Vect2[] {  new Vect2(0,2), new Vect2(1,2),  new Vect2(2,2),  new Vect2(3,2) },
             new Vect2[] {  new Vect2(2,0), new Vect2(2,1),  new Vect2(2,2),  new Vect2(2,3) },
             new Vect2[] {  new Vect2(0,1), new Vect2(1,1),  new Vect2(2,1),  new Vect2(3,1) },
             new Vect2[] {  new Vect2(1,0), new Vect2(1,1),  new Vect2(1,2),  new Vect2(1,3) },
        },
    };
}
