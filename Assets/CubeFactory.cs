using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour
{
    public GameObject Cubes;

    List<RamdonGroup> _indexes_to_create = new List<RamdonGroup>();

    public GameObject Create()
    {
        RamdonGroup index = GetNextValue();
        return Create(index);
    }

    public GameObject Create(RamdonGroup group)
    {
        GameObject obj = Instantiate(Cubes);
        obj.GetComponent<CubeSet>().Randomize(group.CubeIndex, group.ShapeIndex);

        return obj;
    }

    RamdonGroup GetNextValue()
    {
        RamdonGroup index = null;
        if (_indexes_to_create.Count > 0)
        {
            index = _indexes_to_create[0];
            _indexes_to_create.RemoveAt(0);

            RamdonGroup newIndex = GetDataRandomValue();
            _indexes_to_create.Add(newIndex);
        }
        else
        {
            index = GetDataRandomValue();
        }

        return index;
    }

    RamdonGroup GetDataRandomValue()
    {
        RamdonGroup group = new RamdonGroup();
        group.CubeIndex = UnityEngine.Random.Range(0, CubeSetData.Instance.Data.Length);
        group.ShapeIndex = UnityEngine.Random.Range(0, CubeSetData.Instance.Data[group.CubeIndex].Count);
        return group;
    }

    public void InitFactory(int length)
    {
        for (int i = 0; i < length; i++)
        {
            RamdonGroup dataindex = GetDataRandomValue();
            _indexes_to_create.Add(dataindex);
        }
    }

    public List<GameObject> PreCreate(int length)
    {
        List<GameObject> Objs = new List<GameObject>();
        for (int i = 0; i < length && i < _indexes_to_create.Count; i++)
        {
            Objs.Add(Create(_indexes_to_create[i]));
        }
        return Objs;
    }

    public class RamdonGroup
    {
        public int CubeIndex;
        public int ShapeIndex;
    }
}
