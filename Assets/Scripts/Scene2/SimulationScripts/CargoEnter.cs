using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoEnter : MonoBehaviour
{

    //private KeyPositionsData KPD; //场景关键数据
    //public GameObject OBJ1;
    public float Speed; //移动速度
    public int PilerNum;
    private CargoMessage CM; //货物信息
    private Vector3[] KeyPositions;//货物运动关键点坐标
    private GameObject LiftTransfer;
    private GameObject LiftPart;
    public bool AllRight;
    private Vector3 RotationAngle;
    private int KeyStep;
    private int KeysNum;
    private bool[] StepState;
    private bool[,] UnidirectionState;//单向输送线可行性状态
    private bool[,] LiftTransferState;//顶升移载机可行性状态
    private bool[,] BidirectionState;//双向输送线可行性状态
    private bool[,] UnidirectionFinish;//是否到达关键点
    private bool[,] LiftTransferFinish;//顶升移载机关键点
    private bool[,] BidirectionFinish;//双向输送线关键点到达判断
    private bool KeyFrame1;//阶段1
    private bool KeyFrame2;//阶段2

    void Start()
    {
        //获取主要参数
        //KPD = GameObject.Find("WarehouseScene").GetComponent<ShowKeyPositionData>().KeyPositionsData; //获取场景关键信息
        CM = this.GetComponent<ShowCargoInfo>().Cargomessage;
        RotationAngle = GlobalVariable.WareHouse.transform.eulerAngles;
        //Speed = 0.8f;
        int Tempi1 = CM.PositionInfo.HighBayNum; int Tempi2 = (Tempi1 + 1) / 2;
        //顶升移栽对象
        string LiftTransferName = GlobalVariable.WareHouse.name + "/LiftTransferGroup/LiftTransfer" + Tempi2.ToString();
        LiftTransfer = GameObject.Find(LiftTransferName);
        string LiftPartName = LiftTransferName + "/LiftPart";
        LiftPart = GameObject.Find(LiftPartName);
        //货箱关键点状态
        int PilerNums = (GlobalVariable.KPD.HighBaysNum + 1) / 2; //Debug.Log(PilerNums);
        PilerNum = Tempi2 - 1;
        UnidirectionState = new bool[PilerNums, 2];
        UnidirectionFinish = new bool[PilerNums, 2];
        LiftTransferState = new bool[PilerNums, 2];
        LiftTransferFinish = new bool[PilerNums, 3];
        BidirectionState = new bool[3, 2];
        BidirectionFinish = new bool[3, 2];
        for (int i = 0; i < PilerNums; i++)
        {
            UnidirectionState[i, 0] = false; UnidirectionState[i, 1] = false;
            LiftTransferState[i, 0] = false; UnidirectionState[i, 1] = false;
            UnidirectionFinish[i, 0] = false; UnidirectionFinish[i, 1] = false;
            LiftTransferFinish[i, 0] = false; LiftTransferFinish[i, 1] = false; LiftTransferFinish[i, 2] = false;
        }
        for (int i = 0; i < 3; i++)
        {
            BidirectionState[i, 0] = false; BidirectionState[i, 1] = false;
            BidirectionFinish[i, 0] = false; BidirectionFinish[i, 1] = false;
        }
        AllRight = false;
        KeyStep = 0;
        KeysNum = 0;
        KeyFrame1 = false;
        KeyFrame2 = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //货物到达UniConveyor0位置
        if (UnidirectionFinish[0, 0] == false)
        {
            //访问未知情况
            if (GlobalVariable.UnidirectionalConveyorStates[0, 0] == State.Off && UnidirectionState[0, 0] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[0, 0] = State.On;
                UnidirectionState[0, 0] = true;
            }
            //移动到指定位置
            if (UnidirectionState[0, 0] == true)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[0, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[0, 0])
                {
                    UnidirectionFinish[0, 0] = true;
                    KeyFrame1 = true;
                }
            }
        }
        //阶段1过程
        if (KeyFrame1 == true && KeyFrame2 == false)
        {
            LiftTransfer2UniConveyor(KeyStep);
            UniConveyor0_1(KeyStep);
            UniConveyor2LiftTransfer(KeyStep);
        }
        if (KeyFrame2 == true && AllRight == false)
        {
            LiftTransferEnter();
            BiConveyor1_0(KeysNum);
            BiConveyor0_1(KeysNum);
        }
        
    }
    //UniConveyor上0-1位置移动
    public void UniConveyor0_1(int i)
    {
        if (UnidirectionFinish[i, 0] == true && UnidirectionFinish[i, 1] == false)
        {
            //访问1位置可行性
            if (GlobalVariable.UnidirectionalConveyorStates[i, 1] == State.Off && UnidirectionState[i, 1] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.On;
                UnidirectionState[i, 1] = true;
                GlobalVariable.UnidirectionalConveyorStates[i, 0] = State.Off;
            }
            //0-1位置移动
            if (UnidirectionState[i, 1] == true && UnidirectionFinish[i, 1] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[i, 1], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[i, 1])
                {
                    UnidirectionFinish[i, 1] = true;
                }
            }
        }  
    }
    //UniConveyor过渡到顶升移载机
    public void UniConveyor2LiftTransfer(int i)
    {
        if (UnidirectionFinish[i, 1] == true && LiftTransferFinish[i, 0] == false)
        {
            //访问顶升移载机
            if (i != PilerNum)
            {
                //访问顶升移载机
                if (GlobalVariable.LiftTransferStates[i] == State.Off && LiftTransferState[i, 0] == false)
                {
                    GlobalVariable.LiftTransferStates[i] = State.On;
                    LiftTransferState[i, 0] = true;
                    GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.Off;
                }
                //过渡
                if (LiftTransferState[i, 0] == true && LiftTransferFinish[i, 0] == false)
                {
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.LiftTransferPositions[i, 0], Speed * Time.deltaTime);
                    if (this.transform.localPosition == GlobalVariable.LiftTransferPositions[i, 0])
                    {
                        LiftTransferFinish[i, 0] = true;
                    }
                }
            }
            if (i == PilerNum)
            {
                //访问顶升移载机
                if (GlobalVariable.ConveyorDirections[PilerNum] == Direction.Enter)
                {
                    if (GlobalVariable.LiftTransferStates[i] == State.Off && LiftTransferState[i, 0] == false)
                    {
                        GlobalVariable.LiftTransferStates[i] = State.On;
                        LiftTransferState[i, 0] = true;
                        GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.Off;
                    }
                }
                //过渡
                if (LiftTransferState[i, 0] == true && LiftTransferFinish[i, 0] == false)
                {
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.LiftTransferPositions[i, 0], Speed * Time.deltaTime);
                    if (this.transform.localPosition == GlobalVariable.LiftTransferPositions[i, 0])
                    {
                        LiftTransferFinish[i, 0] = true;
                        KeyFrame2 = true;
                        this.transform.parent = LiftPart.transform;
                        LiftTransfer.AddComponent<LiftTransferMove>().enabled = false;
                        LiftTransfer.GetComponent<LiftTransferMove>().Speed = Speed / 30;
                        LiftTransfer.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.up;
                    }
                }
            }
        }
    }
    //顶升移载机过渡到UniConveyor
    public void LiftTransfer2UniConveyor(int i)
    {
        if (LiftTransferFinish[i, 0] == true && UnidirectionFinish[i + 1, 0] == false)
        {
            //访问UniConveyor
            if (GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] == State.Off && UnidirectionState[i + 1, 0] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] = State.On;
                UnidirectionState[i + 1, 0] = true;
            }
            //过渡
            if (UnidirectionState[i + 1, 0] == true && UnidirectionFinish[i + 1, 0] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[i + 1, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[i + 1, 0])
                {
                    UnidirectionFinish[i + 1, 0] = true;
                    GlobalVariable.LiftTransferStates[i] = State.Off;
                    KeyStep = KeyStep + 1;
                }
            }
        }
    }
    //顶升移载机工作
    public void LiftTransferEnter()
    {
        if (LiftTransferFinish[PilerNum, 0] == true && LiftTransferFinish[PilerNum, 2] == false)
        {
            //顶升抬起货物
            if (LiftTransferFinish[PilerNum, 1] == false)
            {
                LiftTransfer.GetComponent<LiftTransferMove>().enabled = true;
                if (LiftTransfer.GetComponent<LiftTransferMove>().Finish1 == true)
                {
                    LiftTransferFinish[PilerNum, 1] = true;
                    this.transform.parent = GlobalVariable.WareHouse.transform;
                }
            }
            //货物离开顶升移栽机
            if (LiftTransferFinish[PilerNum, 1] == true && BidirectionFinish[0, 0] == false)
            {
                //访问BiConveyor
                if (GlobalVariable.BidirectionalConveyorStates[PilerNum, 0, 0] == State.Off && BidirectionState[0, 0] == false)
                {
                    GlobalVariable.BidirectionalConveyorStates[PilerNum, 0, 0] = State.On;
                    BidirectionState[0, 0] = true;
                }
                //货物过渡
                if (BidirectionState[0, 0] == true && BidirectionFinish[0, 0] == false)
                {
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.BidirectionalPositions[PilerNum, 0, 0], Speed * Time.deltaTime);
                    if (this.transform.localPosition == GlobalVariable.BidirectionalPositions[PilerNum, 0, 0])
                    {
                        BidirectionFinish[0, 0] = true;
                        LiftTransfer.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.down;
                        KeysNum = 0;
                    }
                }
            }
            //顶升降下
            if (BidirectionFinish[0, 0] == true)
            {
                LiftTransfer.GetComponent<LiftTransferMove>().enabled = true;
                if (LiftTransfer.GetComponent<LiftTransferMove>().Finish2 == true)
                {
                    LiftTransferFinish[PilerNum, 2] = true;
                    GlobalVariable.LiftTransferStates[PilerNum] = State.Off;
                    DestroyImmediate(LiftTransfer.GetComponent<LiftTransferMove>());
                }
            }
        }
    }
    //BiConveyor0-1位置移动
    public void BiConveyor0_1(int j)
    {
        if (BidirectionFinish[j, 0] == true && BidirectionFinish[j, 1] == false)
        {
            //访问1位置
            if (GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 1] == State.Off && BidirectionState[j, 1] == false)
            {
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 1] = State.On;
                BidirectionState[j, 1] = true;
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 0] = State.Off;
            }
            //0-1位置移动
            if (BidirectionState[j, 1] == true && BidirectionFinish[j, 1] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.BidirectionalPositions[PilerNum, j, 1], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.BidirectionalPositions[PilerNum, j, 1])
                {
                    BidirectionFinish[j, 1] = true;
                    if (j == 2)
                    {
                        AllRight = true;
                        GlobalVariable.Wait[PilerNum] = WaitState.WaitEnter;
                    }
                }
            }
        }
    }
    //BiConveyor之间过渡
    public void BiConveyor1_0(int j)
    {
        if (BidirectionFinish[j, 1] == true && BidirectionFinish[j + 1, 0] == false)
        {
            //访问0位置
            if (GlobalVariable.BidirectionalConveyorStates[PilerNum, j + 1, 0] == State.Off && BidirectionState[j + 1, 0] == false)
            {
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j + 1, 0] = State.On;
                BidirectionState[j + 1, 0] = true;
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 1] = State.Off;
            }
            //过渡
            if (BidirectionState[j + 1, 0] == true && BidirectionFinish[j + 1, 0] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.BidirectionalPositions[PilerNum, j + 1, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.BidirectionalPositions[PilerNum, j + 1, 0])
                {
                    BidirectionFinish[j + 1, 0] = true;
                    KeysNum = KeysNum + 1;
                }
            }
        }
    }
}
