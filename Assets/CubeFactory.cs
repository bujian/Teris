using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour
{
    public GameObject Cubes;

    public GameObject Create()
    {
        int dataindex = UnityEngine.Random.Range(0, CubeSetData.Instance.Data.Length);

        GameObject obj = Instantiate(Cubes);
        obj.GetComponent<CubeSet>().Randomize(dataindex);

        return obj;
    }
}
