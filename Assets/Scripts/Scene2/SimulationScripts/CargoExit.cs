using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoExit : MonoBehaviour
{

    public float Speed; //移动速度
    public int PilerNum;
    private CargoMessage CM; //货物信息
    private Vector3[] KeyPositions;//货物运动关键点坐标
    private GameObject LiftTransfer;
    private GameObject LiftTransfer1;
    private GameObject LiftPart;
    private GameObject LiftPart1;
    public bool AllRight;
    private Vector3 RotationAngle;
    private int KeyStep;
    private int KeysNum;
    private int PilerNums;
    private bool[,] UnidirectionState;//单向输送线可行性状态
    private bool[,] LiftTransferState;//顶升移载机可行性状态
    private bool[,] BidirectionState;//双向输送线可行性状态
    private bool[,] UnidirectionFinish;//是否到达关键点
    private bool[,] LiftTransferFinish;//顶升移载机关键点
    private bool[,] BidirectionFinish;//双向输送线关键点到达判断
    private bool KeyFrame;//判断是否为指定顶升移载机
    private bool KeyFrame2;//判断是否在横向输送线上
    void Start()
    {
        //获取主要参数
        //KPD = GameObject.Find("WarehouseScene").GetComponent<ShowKeyPositionData>().KeyPositionsData; //获取场景关键信息
        CM = this.GetComponent<ShowCargoInfo>().Cargomessage;
        RotationAngle = GlobalVariable.WareHouse.transform.eulerAngles;
        //Speed = 0.8f;
        int Tempi1 = CM.PositionInfo.HighBayNum; int Tempi2 = (Tempi1 + 1) / 2;
        PilerNums = (GlobalVariable.KPD.HighBaysNum + 1) / 2; //Debug.Log(PilerNums);
        PilerNum = Tempi2 - 1;
        //顶升移栽对象
        string LiftTransferName = GlobalVariable.WareHouse.name + "/LiftTransferGroup/LiftTransfer" + Tempi2.ToString();
        LiftTransfer = GameObject.Find(LiftTransferName);
        string LiftPartName = LiftTransferName + "/LiftPart";
        LiftPart = GameObject.Find(LiftPartName);
        string LiftTransferName1 = GlobalVariable.WareHouse.name + "/LiftTransferGroup/LiftTransfer" + (PilerNums+1).ToString();
        LiftTransfer1 = GameObject.Find(LiftTransferName1);
        string LiftPartName1 = LiftTransferName1 + "/LiftPart";
        LiftPart1 = GameObject.Find(LiftPartName1);
        //货箱关键点状态

        UnidirectionState = new bool[PilerNums + 4, 2];
        UnidirectionFinish = new bool[PilerNums + 4, 2];
        LiftTransferState = new bool[PilerNums + 1, 2];
        LiftTransferFinish = new bool[PilerNums + 1, 3];
        BidirectionState = new bool[3, 2];
        BidirectionFinish = new bool[3, 2];
        for (int i = 0; i <= PilerNums; i++)
        {
            UnidirectionState[i, 0] = false; UnidirectionState[i, 1] = false;
            LiftTransferState[i, 0] = false; UnidirectionState[i, 1] = false;
            UnidirectionFinish[i, 0] = false; UnidirectionFinish[i, 1] = false;
            LiftTransferFinish[i, 0] = false; LiftTransferFinish[i, 1] = false; LiftTransferFinish[i, 2] = false;
        }
        UnidirectionState[PilerNums + 1, 0] = false; UnidirectionState[PilerNums + 1, 1] = false;
        UnidirectionState[PilerNums + 2, 0] = false; UnidirectionState[PilerNums + 2, 1] = false;
        UnidirectionState[PilerNums + 3, 0] = false; UnidirectionState[PilerNums + 3, 1] = false;
        UnidirectionFinish[PilerNums + 1, 0] = false; UnidirectionFinish[PilerNums + 1, 1] = false;
        UnidirectionFinish[PilerNums + 2, 0] = false; UnidirectionFinish[PilerNums + 2, 1] = false;
        UnidirectionFinish[PilerNums + 3, 0] = false; UnidirectionFinish[PilerNums + 3, 1] = false;
        for (int i = 0; i < 3; i++)
        {
            BidirectionState[i, 0] = false; BidirectionState[i, 1] = false;
            BidirectionFinish[i, 0] = false; BidirectionFinish[i, 1] = false;
        }
        AllRight = false;
        KeyStep = 0;
        KeysNum = 2;
        KeyFrame = true;
        KeyFrame2 = true;
        BidirectionFinish[KeysNum, 1] = true;
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        //出库线路
        if (KeyFrame == true)
        {
            BiConveyor0_1(KeysNum);
            BiConveyor1_0(KeysNum);
        }
        //出库后线路
        if (KeyFrame == false && KeyFrame2 == true)
        {
            //顶升移载机运输货物出库
            LiftTransferExit();
            //货物从顶升移载机过渡到UniConveyor
            LiftTransfer2UniConveyor(KeyStep);
            //货物在UniConveyor上运输
            UniConveyor0_1(KeyStep);
            //货物从UniConveyor过渡到顶升移载机
            UniConveyor2LiftTransfer(KeyStep);
        }
        //转向出库操作
        if (KeyFrame2 == false && AllRight == false)
        {
            //顶升移栽机工作
            WorkLiftTransfer();
            //UniConveyor之间过渡运输货物
            UniConveyor1_0(KeyStep);
            //UniConveyor运输货物
            UniConveyor0_1(KeyStep);
        }
    }


    //BiditectionConveyor上1-0移动
    public void BiConveyor1_0(int j)
    {
        if (BidirectionFinish[j, 1] == true && BidirectionFinish[j, 0] == false)
        {
            //访问0位置可行情况
            if (GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 0] == State.Off && BidirectionState[j, 0] == false)
            {
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 0] = State.On;
                BidirectionState[j, 0] = true;
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 1] = State.Off;
                if (j == 2)
                {
                    GlobalVariable.Wait[PilerNum] = WaitState.None;
                }
            }
            //1位置移动到0位置
            if (BidirectionState[j, 0] == true && BidirectionFinish[j, 0] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.BidirectionalPositions[PilerNum, j, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.BidirectionalPositions[PilerNum, j, 0])
                {
                    BidirectionFinish[j, 0] = true;
                    if (j == 0)
                    {
                        KeyFrame = false;
                        KeyFrame2 = true;
                    }
                }
            }
        } 
    }
    //BidirectionConveyor0位置过渡到下一个BidirectionConveyor1位置
    public void BiConveyor0_1(int j)
    {
        if (BidirectionFinish[j, 0] == true && BidirectionFinish[j - 1, 1] == false)
        {
            //访问下一个设备1位置可行情况
            if (GlobalVariable.BidirectionalConveyorStates[PilerNum, j - 1, 1] == State.Off && BidirectionState[j - 1, 1] == false)
            {
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j - 1, 1] = State.On;
                BidirectionState[j - 1, 1] = true;
                GlobalVariable.BidirectionalConveyorStates[PilerNum, j, 0] = State.Off;
            }
            //进行过渡
            if (BidirectionState[j - 1, 1] == true && BidirectionFinish[j - 1, 1] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.BidirectionalPositions[PilerNum, j - 1, 1], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.BidirectionalPositions[PilerNum, j - 1, 1])
                {
                    BidirectionFinish[j - 1, 1] = true;
                    KeysNum = KeysNum - 1;
                }
            }
        }
    }
    //顶升移载机出库工作
    public void LiftTransferExit()
    {
        if (BidirectionFinish[0, 0] == true && LiftTransferFinish[PilerNum, 0] == false)
        {
            //访问输送机可行情况
            if (GlobalVariable.LiftTransferStates[PilerNum] == State.Off && LiftTransferState[PilerNum, 0] == false)
            {
                GlobalVariable.LiftTransferStates[PilerNum] = State.On;
                LiftTransferState[PilerNum, 0] = true;
                LiftTransfer.AddComponent<LiftTransferMove>().enabled = false;
                LiftTransfer.GetComponent<LiftTransferMove>().Speed = Speed / 30;
                LiftTransfer.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.up;
            }
            //顶升移载机抬起
            if (LiftTransferState[PilerNum, 0] == true && LiftTransferFinish[PilerNum, 2] == false)
            {
                LiftTransfer.GetComponent<LiftTransferMove>().enabled = true;
                if (LiftTransfer.GetComponent<LiftTransferMove>().Finish1 == true)
                {
                    LiftTransferFinish[PilerNum, 2] = true;
                    GlobalVariable.BidirectionalConveyorStates[PilerNum, 0, 0] = State.Off;
                }
            }
            //货物过渡到顶升移载机上
            if (LiftTransferFinish[PilerNum, 2] == true && LiftTransferFinish[PilerNum, 1] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.LiftTransferPositions[PilerNum, 1], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.LiftTransferPositions[PilerNum, 1])
                {
                    LiftTransferFinish[PilerNum, 1] = true;
                    this.transform.parent = LiftPart.transform;
                    LiftTransfer.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.down;
                }
            }
            //顶升移载机降下
            if (LiftTransferFinish[PilerNum, 1] == true && LiftTransferFinish[PilerNum, 0] == false)
            {
                LiftTransfer.GetComponent<LiftTransferMove>().enabled = true;
                if (LiftTransfer.GetComponent<LiftTransferMove>().Finish2 == true)
                {
                    LiftTransferFinish[PilerNum, 0] = true;
                    this.transform.parent = GlobalVariable.WareHouse.transform;
                    DestroyImmediate(LiftTransfer.GetComponent<LiftTransferMove>());
                    KeyStep = PilerNum;
                    if (GlobalVariable.BidirectionalConveyorStates[PilerNum, 0, 0] == State.Off && GlobalVariable.BidirectionalConveyorStates[PilerNum, 0, 1] == State.Off
                        && GlobalVariable.BidirectionalConveyorStates[PilerNum, 1, 0] == State.Off && GlobalVariable.BidirectionalConveyorStates[PilerNum, 1, 1] == State.Off
                        && GlobalVariable.BidirectionalConveyorStates[PilerNum, 2, 0] == State.Off && GlobalVariable.BidirectionalConveyorStates[PilerNum, 1, 1] == State.Off)
                    {
                        if (GlobalVariable.ConveyorQueue[PilerNum].Count == 0 
                            ||(GlobalVariable.ConveyorQueue[PilerNum].Count>0 && GlobalVariable.ConveyorQueue[PilerNum].Peek().GetComponent<OperatingState>().state==CargoState.Enter))
                        {
                            GlobalVariable.ConveyorDirections[PilerNum] = Direction.Enter;
                        }
                    }
                }
            }
        }
        
    }
    //UnidirectionConveyor上0-1移动
    public void UniConveyor0_1(int i)
    {
        if (UnidirectionFinish[i, 0] == true && UnidirectionFinish[i, 1] == false)
        {
            //访问1位置可行情况
            if (GlobalVariable.UnidirectionalConveyorStates[i, 1] == State.Off && UnidirectionState[i, 1] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.On;
                UnidirectionState[i, 1] = true;
                GlobalVariable.UnidirectionalConveyorStates[i, 0] = State.Off;
            }
            //0位置移动到1位置
            if (UnidirectionState[i, 1] == true && UnidirectionFinish[i, 1] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[i, 1], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[i, 1])
                {
                    UnidirectionFinish[i, 1] = true;
                    if (i == (PilerNums + 3))
                    {
                        AllRight = true;
                        GlobalVariable.UnidirectionalConveyorStates[KeyStep, 1] = State.Off;
                        DestroyImmediate(GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(this.name).gameObject);
                        GlobalVariable.ExitQueue[PilerNum].Dequeue();
                        DestroyImmediate(this.GetComponent<CargoExit>());
                    }
                }
            }
        }  
    }
    //UnidirectionConveyor1位置过渡到顶升移载机
    public void UniConveyor2LiftTransfer(int i)
    {
        if (UnidirectionFinish[i, 1] == true && LiftTransferFinish[i, 0] == false)
        {
            //访问LiftTransfer可通行情况
            if (GlobalVariable.LiftTransferStates[i] == State.Off && LiftTransferState[i, 0] == false)
            {
                GlobalVariable.LiftTransferStates[i] = State.On;
                LiftTransferState[i, 0] = true;
                GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.Off;
            }
            //UniConveyor1位置过渡到LIftTransfer
            if (LiftTransferState[i, 0] == true && LiftTransferFinish[i, 0] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.LiftTransferPositions[i, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.LiftTransferPositions[i, 0])
                {
                    LiftTransferFinish[i, 0] = true;
                    if (i == PilerNums)
                    {
                        KeyFrame2 = false;
                        KeyStep = PilerNums + 1;
                        LiftTransfer1.AddComponent<LiftTransferMove>().enabled = false;
                        LiftTransfer1.GetComponent<LiftTransferMove>().Speed = Speed / 3;
                        LiftTransfer1.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.up;
                        this.transform.parent = LiftPart1.transform;
                    }
                }
            }
        }   
    }
    //LiftTransfer过渡到UnidirectionConveyor0位置
    public void LiftTransfer2UniConveyor(int i)
    {
        if (LiftTransferFinish[i, 0] == true && UnidirectionFinish[i + 1, 0] == false)
        {
            //访问UniConveyor0位置可行情况
            if (GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] == State.Off && UnidirectionState[i + 1, 0] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] = State.On;
                UnidirectionState[i + 1, 0] = true;
            }
            //LiftTransfer过渡到UniConveyor0位置
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
    //LiftTransfer工作机
    public void WorkLiftTransfer()
    {
        if (LiftTransferFinish[PilerNums, 0] == true && LiftTransferFinish[PilerNums, 2] == false)
        {
            //顶升移载机抬起
            if (LiftTransferFinish[PilerNums, 0] == true && LiftTransferFinish[PilerNums, 1] == false)
            {
                LiftTransfer1.GetComponent<LiftTransferMove>().enabled = true;
                if (LiftTransfer1.GetComponent<LiftTransferMove>().Finish1 == true)
                {
                    LiftTransferFinish[PilerNums, 1] = true;
                }
            }
            //货物过渡到顶升移载机上
            if (LiftTransferFinish[PilerNums, 1] == true && LiftTransferFinish[PilerNums, 2] == false)
            {
                //访问
                if (GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 0] == State.Off && UnidirectionState[PilerNums + 1, 0] == false)
                {
                    GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 0] = State.On;
                    UnidirectionState[PilerNums + 1, 0] = true;
                    this.transform.parent = GlobalVariable.WareHouse.transform;
                }
                //过渡
                if (UnidirectionState[PilerNums + 1, 0] == true && UnidirectionFinish[PilerNums + 1, 0] == false)
                {
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0], Speed * Time.deltaTime);
                    if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0])
                    {
                        UnidirectionFinish[PilerNums + 1, 0] = true;
                        LiftTransfer1.GetComponent<LiftTransferMove>().pattern = LiftTransferMove.Pattern.down;
                        KeyStep = PilerNums + 1;
                    }
                }
                //顶升移载机降下
                if (UnidirectionFinish[PilerNums + 1, 0] == true)
                {
                    LiftTransfer1.GetComponent<LiftTransferMove>().enabled = true;
                    if (LiftTransfer1.GetComponent<LiftTransferMove>().Finish2 == true)
                    {
                        LiftTransferFinish[PilerNums, 2] = true;
                        DestroyImmediate(LiftTransfer1.GetComponent<LiftTransferMove>());
                        GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(this.name).transform.Find("State").GetComponent<Text>().text = "货物状态：" + "完成出库";//修改进程中该货物状态信息
                        Functions.ChangeColor(this.name, StorageBinState.NotStored);//修改该货物对应仓位的状态表示颜色
                        this.GetComponent<OperatingState>().state = CargoState.Exit;
                        GlobalVariable.LiftTransferStates[PilerNums] = State.Off;
                        GlobalVariable.EnterCargosNameList.Remove(this.name);
                    }
                }
            }
        }
    }
    //UnidirectionConveyor过渡到下一个UnidirectionConveyor
    public void UniConveyor1_0(int i)
    {
        
        if (UnidirectionFinish[i, 1] == true && UnidirectionFinish[i + 1, 0] == false)
        {
            //访问下一个UniConveyor0位置可行情况
            if (GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] == State.Off && UnidirectionState[i + 1, 0] == false)
            {
                GlobalVariable.UnidirectionalConveyorStates[i + 1, 0] = State.On;
                UnidirectionState[i + 1, 0] = true;
                GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.Off;
            }
            //过渡移动
            if (UnidirectionState[i + 1, 0] == true && UnidirectionFinish[i + 1, 0] == false)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, GlobalVariable.UnidirectionalPositions[i + 1, 0], Speed * Time.deltaTime);
                if (this.transform.localPosition == GlobalVariable.UnidirectionalPositions[i + 1, 0])
                {
                    UnidirectionFinish[i + 1, 0] = true;
                    KeyStep = KeyStep + 1;
                    
                }
            }
        }
    }
    
}