using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSet : MonoBehaviour
{
    /// <summary>
    /// 方块模型组合
    /// </summary>
    public List<Transform> CubeModels;
    /// <summary>
    /// 旋转状态
    /// </summary>
    int _state;

    int MaxStateCount;
    /// <summary>
    /// 区域内的小方块相对偏移量
    /// </summary>
    List<Vect2[]> cubes_offset;
    /// <summary>
    /// 实际左下角点位
    /// </summary>
    Vector3 _ori_act_pos;
    /// <summary>
    /// 左下角点位
    /// </summary>
    Vect2 _ori_pos;

    public GameObject CopyObj;

    public int Step = 1;

    public void Randomize(int cKind, int sKind)
    {
        cubes_offset = CubeSetData.Instance.Data[cKind];
        Color color = CubeSetData.Instance.Colors[cKind];

        MaxStateCount = cubes_offset.Count;
        _state = sKind;
        SetData(cubes_offset, color);
    }

    void SetData(List<Vect2[]> data, Color color)
    {
        cubes_offset = data;

        for (int i = 0; i < cubes_offset[0].Length; i++)
        {
            var obj = Instantiate(CopyObj);
            obj.GetComponent<Renderer>().material.color = color;
            obj.AddComponent<SingleCubeTag>();
            obj.transform.parent = this.transform;
            CubeModels.Add(obj.transform);
        }
    }

    public void Rotate()
    {
        _state = GetNextState(_state);
        PosChanged(_state, _ori_pos,_ori_act_pos);
    }

    public void SetPos(Vect2 oriPos, Vector3 actOriPos)
    {
        _ori_act_pos = actOriPos;
        _ori_pos = oriPos;

        PosChanged(_state, _ori_pos, _ori_act_pos);
    }

    void PosChanged(int state, Vect2 oriPos, Vector3 actOriPos)
    {
        Vector3[] actPos = new Vector3[cubes_offset[0].Length];
        Vect2[] offset = GetStateOffset(state);

        for (int i = 0; i < actPos.Length; i++)
        {
            CubeModels[i].localPosition = actPos[i];
            var tag = CubeModels[i].GetComponent<SingleCubeTag>();
            tag.UpdateActPos(offset[i], actOriPos);
        }

    }

    /// <summary>
    /// 找到下一个旋转的点（抠掉和当前的重复点，因为当前的点不可能 有其他东西）
    /// </summary>
    /// <returns></returns>
    public Vect2[] GetRotRange()
    {
        List<Vect2> vects = new List<Vect2>();
        Vect2[] curOffset = GetStateOffset(_state);
        Vect2[] nexOffset = GetStateOffset(GetNextState(_state));

        for (int i = 0; i < nexOffset.Length; i++)
        {
            vects.Add(nexOffset[i]);
        }

        for (int i = 0; i < curOffset.Length; i++)
        {
            Vect2 offset = curOffset[i];
            var ret = vects.Find(v => v.Equals(offset));
            if (ret != null)
            {
                vects.Remove(ret);
            }
        }

        return vects.ToArray();
    }

    int GetNextState(int state)
    {
        state++;
        if (state >= MaxStateCount)
        {
            state = 0;
        }
        return state;
    }

    Vect2[] GetStateOffset(int state)
    {
        Vect2[] offset = cubes_offset[state];
        Vect2[] actOffset = new Vect2[offset.Length];
        for (int i = 0; i < actOffset.Length; i++)
        {
            actOffset[i] = _ori_pos + offset[i];
        }
        return actOffset;
    }

    public Vect2[] GetBorder(Direction bord)
    {
        List<Vect2> vects = new List<Vect2>();
        Vect2[] curOffset = GetStateOffset(_state);
        for (int i = 0; i < curOffset.Length; i++)
        {
            Vect2 off = curOffset[i].Clone();
            Predicate<Vect2> condition = null;
            Action<Vect2> replace = null;
            switch (bord)
            {
                case Direction.Left:
                    {
                        condition = v => v.YPos == off.YPos;
                        replace = lve => { if (off.XPos < lve.XPos) lve.XPos = off.XPos; };
                    }
                    break;
                case Direction.Right:
                    {
                        condition = v => v.YPos == off.YPos;
                        replace = lve => { if (off.XPos > lve.XPos) lve.XPos = off.XPos; };
                    }
                    break;
                case Direction.Down:
                    {
                        condition = v => v.XPos == off.XPos;
                        replace = lve => { if (off.YPos < lve.YPos) lve.YPos = off.YPos; };
                    }
                    break;
            }

            var lvec = vects.Find(condition);
            if (lvec == null)
            {
                vects.Add(off);
            }
            else
            {
                replace(lvec);
            }

        }
    
        return vects.ToArray();
    }

    public void MoveTo(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                {
                    _ori_pos.XPos -= 1;
                }
                break;
            case Direction.Right:
                {
                    _ori_pos.XPos += 1;
                }
                break;
            case Direction.Down:
                {
                    _ori_pos.YPos -= 1;
                }
                break;
        }
        PosChanged(_state, _ori_pos, _ori_act_pos);
        //print(_ori_pos.XPos + ", " + _ori_pos.YPos);
    }

    public Vect2[] GetCurCubeState()
    {
        return GetStateOffset(_state);
    }

    public List<Transform> LetChildrenGo()
    {
        return CubeModels;
    }
}
