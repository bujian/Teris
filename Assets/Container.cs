using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    int Width = 10;
    int Height = 20;

    int Height_Exp = 4;

    /// <summary>
    /// 位置状态 true为空  false为满
    /// </summary>
    bool[][] CubeEmptyStates;
    Vector3 OriginPos;
    public CubeFactory Factory;

    CubeSet CurCube;

    public Transform LineGroup;

    bool _gameover = false;
    private void Awake()
    {
        int width = Width;
        int height = Height + Height_Exp;
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
    }

    private void Start()
    {
        Create();
        //StartCoroutine(StartAutoMove(0.5f));
    }

    private IEnumerator StartAutoMove(float v)
    {
        while (!_gameover)
        {
            yield return new WaitForSeconds(v);
            CubeMove(CurCube, Direction.Down);
        }
        print("Stop Move");
    }

    private void Update()
    {
        if (!_gameover)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CubeMove(CurCube, Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CubeMove(CurCube, Direction.Right);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                CubeMove(CurCube, Direction.Down);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CubeRotate(CurCube);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < CubeEmptyStates.Length; i++)
                {
                    string s = i + ": ";
                    for (int j = 0; j < CubeEmptyStates[i].Length; j++)
                    {
                        s += CubeEmptyStates[i][j] ? 1 : 0;
                    }
                    print(s);
                }
            }
        }
    }

    void ClearStates()
    {
        for (int i = 0; i < CubeEmptyStates.Length; i++)
        {
            for (int j = 0; j < CubeEmptyStates[i].Length; j++)
            {
                CubeEmptyStates[i][j] = true;
            }
        }
    }


    bool CanMove(CubeSet cube ,Direction d)
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

    bool CanRotate(CubeSet cube)
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

    private void CubeMove(CubeSet cube ,Direction dir)
    {
        if (CanMove(cube, dir))
        {
            SetStates(cube.GetCurCubeState(), true);
            cube.MoveTo(dir);
            SetStates(cube.GetCurCubeState(), false);
        }
        else
        {
            if (dir == Direction.Down)
            {
                TransferCubes();
                RemoveLineCubes();
                if (IsGameOver())
                {
                    _gameover = true;
                    print("Game Over");
                }
                else
                {
                    Create();
                    print("Next");
                }
            }
        }
    }

    bool IsGameOver()
    {
        bool ret = false;
        Vect2[] bors = CurCube.GetBorder(Direction.Left);
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

    private void RemoveLineCubes()
    {
        Vect2[] linepoints = CurCube.GetBorder(Direction.Left);
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

    private void TransferCubes()
    {
        List<Transform> children = CurCube.LetChildrenGo();
        children.ForEach(child =>
        {
            var tag = child.GetComponent<SingleCubeTag>().Pos;
            child.SetParent(LineGroup.GetChild(tag.YPos));
        });
        Destroy(CurCube.gameObject);
    }

    void CubeRotate(CubeSet cube)
    {
        if (CanRotate(cube))
        {
            SetStates(cube.GetCurCubeState(), true);
            cube.Rotate();
            SetStates(cube.GetCurCubeState(), false);
        }
        else
        {

        }
    }

    void SetStates(Vect2[] pos, bool state)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            var p = pos[i];
            CubeEmptyStates[p.XPos][p.YPos] = state;
        }
    }

    void Create()
    {
        var obj = Factory.Create();
        CurCube = obj.GetComponent<CubeSet>();
        CurCube.transform.parent = this.transform;

        int startX = Width / 2 - 2;
        int startY =  Height;
        Vect2 startPos = new Vect2(startX, startY);
        CurCube.SetPos(startPos, OriginPos);
        SetStates(CurCube.GetCurCubeState(), false);
    }
}

public enum Direction
{
    Left,
    Right,
    Down,
}
