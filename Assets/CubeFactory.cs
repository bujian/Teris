using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour
{
    public GameObject Cubes;

    public GameObject Create()
    {
        GameObject obj = Instantiate(Cubes);
        obj.GetComponent<CubeSet>().Randomize();

        return obj;
    }
}
