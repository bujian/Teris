using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Container : MonoBehaviour
{
    int Width = 10;
    int Height = 20;
    int Height_Exp = 4;

    /// <summary>
    /// 位置状态 true为空  false为满
    /// </summary>
    bool[][] CubeEmptyStates;
    public Transform LineGroup;

    public Transform FrameGroup;

    public void InitContainer(int width, int height)
    {
        ClearLineModels();

        Width = width;
        Height = height;
        height += Height_Exp;
        CubeEmptyStates = new bool[width][];
        for (int i = 0; i < CubeEmptyStates.Length; i++)
        {
            CubeEmptyStates[i] = new bool[height];
            for (int j = 0; j < CubeEmptyStates[i].Length; j++)
            {
                CubeEmptyStates[i][j] = true;
            }
        }

        for (int i = 0; i < height; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(LineGroup);
        }

        InitFrame(Width, Height, 1);
    }

    void InitFrame(int width, int height, float cubeSideLen)
    {
        Vector3 ori = Vector3.zero - new Vector3(cubeSideLen / 2 , cubeSideLen / 2);
        float fWidth = width * cubeSideLen;
        float fHeight = height * cubeSideLen;

        Transform down = FrameGroup.GetChild(0);
        Transform left = FrameGroup.GetChild(1);
        Transform right = FrameGroup.GetChild(2);

        float leftW = left.localScale.x;
        float downH = down.localScale.y;
        fWidth += leftW * 2;
        down.localScale = GetNewScale(down.localScale, fWidth, Pos.x);
        left.localScale = GetNewScale(left.localScale, fHeight, Pos.y);
        right.localScale = GetNewScale(right.localScale, fHeight, Pos.y);

        left.localPosition = ori - new Vector3(leftW / 2, 0, 0) + new Vector3(0, fHeight / 2, 0);
        right.localPosition = left.localPosition + new Vector3(cubeSideLen * width + leftW, 0, 0);
        down.localPosition = ori + new Vector3(fWidth /2 - leftW, 0, 0) - new Vector3(0, downH / 2, 0);
    }

    Vector3 GetNewScale(Vector3 pos, float v, Pos p)
    {
        switch (p)
        {
            case Pos.x:
                {
                    pos.x = v;
                }
                break;
            case Pos.y:
                {
                    pos.y = v;
                }
                break;
            case Pos.z:
                {
                    pos.z = v;
                }
                break;
        }
        return pos;
    }

    void ClearLineModels()
    {
        for (int i = 0; i < LineGroup.childCount; i++)
        {
            Destroy(LineGroup.GetChild(i).gameObject);
        }
    }

    public bool CanMove(CubeSet cube, Direction d)
    {
        bool ret = true;
        Predicate<Vect2> judgecon = null;
        switch (d)
        {
            case Direction.Left:
                {
                    judgecon = bor => bor.XPos - 1 >= 0 && CubeEmptyStates[bor.XPos - 1][bor.YPos];
                }
                break;
            case Direction.Right:
                {
                    judgecon = bor => bor.XPos + 1 < Width && CubeEmptyStates[bor.XPos + 1][bor.YPos];
                }
                break;
            case Direction.Down:
                {
                    judgecon = bor => bor.YPos - 1 >= 0 && CubeEmptyStates[bor.XPos][bor.YPos - 1];
                }
                break;
        }

        Vect2[] borders = cube.GetBorder(d);
        for (int i = 0; i < borders.Length; i++)
        {
            Vect2 bor = borders[i];
            //没有碰壁且有空位
            if (!judgecon(bor))
            {
                ret = false;
                break;
            }
        }
        return ret;
    }

    public bool CanRotate(CubeSet cube)
    {
        bool ret = true;
        Vect2[] nextRange = cube.GetRotRange();
        for (int i = 0; i < nextRange.Length; i++)
        {
            Vect2 ran = nextRange[i];
            if (CubeEmptyStates.OutOfRange(ran.XPos, ran.YPos) || !CubeEmptyStates[ran.XPos][ran.YPos])
            {
                ret = false;
                break;
            }
        }
        return ret;
    }
    public bool IsGameOver(CubeSet cube)
    {
        bool ret = false;
        Vect2[] bors = cube.GetBorder(Direction.Left);
        for (int i = 0; i < bors.Length; i++)
        {
            int line = bors[i].YPos;
            if (line >= Height)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    public void RemoveLineCubes(CubeSet cube)
    {
        Vect2[] linepoints = cube.GetBorder(Direction.Left);
        int startIndex = -1;
        int downStep = 0;
        //消除同行
        for (int i = 0; i < linepoints.Length; i++)
        {
            int line = linepoints[i].YPos;
            if (CheckifLineFilled(line))
            {
                RemoveSingleLine(line);
                if (startIndex < line) startIndex = line;
                downStep++;
            }
        }
        startIndex++;
        //下移
        if (downStep > 0)
        {
            int lineCount = CubeEmptyStates[0].Length;
            for (int i = startIndex; i < lineCount; i++)
            {
                print(i);
                MoveDownAfterMove(i, downStep);
            }
        }
    }
    /// <summary>
    /// 消掉一行，清楚状态，上面下移，状态再改便
    /// </summary>
    /// <param name="line"></param>
    void RemoveSingleLine(int line)
    {
        Transform group = LineGroup.GetChild(line);
        for (int i = 0; i < group.childCount; i++)
        {
            Destroy(group.GetChild(i).gameObject);
        }

        ClearLineStates(line);
    }

    void ClearLineStates(int line)
    {
        for (int i = 0; i < CubeEmptyStates.Length; i++)
        {
            CubeEmptyStates[i][line] = true;
        }
    }

    bool CheckifLineFilled(int line)
    {
        bool ret = true;
        for (int i = 0; i < CubeEmptyStates.Length; i++)
        {
            if (CubeEmptyStates[i][line])
            {
                ret = false;
                break;
            }
        }

        return ret;
    }

    /// <summary>
    /// 单行下移
    /// </summary>
    /// <param name="startLine"></param>
    /// <param name="downSteps"></param>
    void MoveDownAfterMove(int startLine, int downSteps)
    {
        int sLine = startLine;
        int dLine = startLine - downSteps;
        for (int i = 0; i < CubeEmptyStates.Length; i++)
        {
            CubeEmptyStates[i][dLine] = CubeEmptyStates[i][sLine];
            CubeEmptyStates[i][sLine] = true;
        }

        Transform sGroup = LineGroup.GetChild(sLine);
        Transform dGroup = LineGroup.GetChild(dLine);
        List<Transform> cList = new List<Transform>();
        for (int i = 0; i < sGroup.childCount; i++)
        {
            cList.Add(sGroup.GetChild(i));
        }

        cList.ForEach(c =>
        {
            var tag = c.GetComponent<SingleCubeTag>();
            tag.transform.SetParent(dGroup);
            tag.MoveDown(downSteps);
        });

    }

    public void TransferCubes(CubeSet cube)
    {
        try
        {
            List<Transform> children = cube.LetChildrenGo();
            children.ForEach(child =>
            {
                var tag = child.GetComponent<SingleCubeTag>().Pos;
                child.SetParent(LineGroup.GetChild(tag.YPos));
            });
            Destroy(cube.gameObject);
        }
        catch (Exception ex)
        {
            print(ex.Message);
            throw;
        }

    }



    public void SetStates(Vect2[] pos, bool state)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            var p = pos[i];
            CubeEmptyStates[p.XPos][p.YPos] = state;
        }
    }


}

public enum Direction
{
    Left,
    Right,
    Down,
}

public enum Pos
{
    x,
    y,
    z
}
