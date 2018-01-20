using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour
{
    public List<GameObject> Cubes;

    public GameObject Create()
    {
        int count = Cubes.Count;
        int index = Random.Range(0, count);

        GameObject obj = Instantiate(Cubes[index]);
        return obj;
    }
}
