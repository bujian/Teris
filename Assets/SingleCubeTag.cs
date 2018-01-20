using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCubeTag : MonoBehaviour
{
    public Vect2 Pos;
    public float SideLength = 1;

    public void UpdateActPos(Vect2 pos ,Vector3 ori = new Vector3())
    {
        this.Pos = pos;
        transform.localPosition = new Vector3(pos.XPos * SideLength, pos.YPos * SideLength) + ori;
    }

    public void MoveDown(int step)
    {
        transform.localPosition -= new Vector3(0, step * SideLength);
    }
}
