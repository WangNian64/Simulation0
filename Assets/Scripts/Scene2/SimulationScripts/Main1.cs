using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class Main1 : MonoBehaviour
{

    public float Speed;
    private bool[] finish1;
    private bool[] finish2;
    public bool AccomplishValue;
    private string CargoName;
    private GameObject[] Cargo;
    private GameObject[] Piler;
    private int PilerNums;
    private int PilerNum;
    private float Times1;
    private bool[] KeyFrame;
    // Use this for initialization
    void Start()
    {
        //摄像机参数设置
        GameObject.Find("Main Camera").AddComponent<InitCamera>(); Debug.Log(0);
        GameObject.Find("Main Camera").AddComponent<FiCameraControl>();Debug.Log(1);
        //设置资源文件夹名
        GlobalVariable.RootName = "Warehouse_Final_Material";
        //参数计算
        Speed = 1.5f;
        //全局变量初始化操作
        #region 
        GlobalVariable.KPD = this.GetComponent<ShowKeyPositionData>().KeyPositionsData;
        GlobalVariable.WareHouse = this.gameObject;
        int CargosNum = GlobalVariable.KPD.HighBaysNum * GlobalVariable.KPD.StorePositions.StoreColumnPositions.Length * GlobalVariable.KPD.StorePositions.StoreFloorPositions.Length * 2;
        GlobalVariable.TempQueue = new Queue<GameObject>();
        int HighBayNum = GlobalVariable.KPD.HighBaysNum; int ColumnNum = GlobalVariable.KPD.StorePositions.StoreColumnPositions.Length;
        int FloorNum = GlobalVariable.KPD.StorePositions.StoreFloorPositions.Length; int PlaceNum = GlobalVariable.KPD.StorePositions.StorePlacePosition.Length;
        GlobalVariable.BinState = new StorageBinState[HighBayNum, FloorNum, ColumnNum, PlaceNum];
        //GlobalVariable.EnterCargosList = new List<GameObject>();
        GlobalVariable.EnterCargosNameList = new List<string>();
        //GlobalVariable.ExitCargosList = new List<GameObject>();
        GlobalVariable.TempQueue = new Queue<GameObject>();
        GlobalVariable.BinColor = new Color[6];
        GlobalVariable.BinColor[0] = new Color(192f / 255f, 192f / 255f, 192f / 255f, 255f / 255f);
        GlobalVariable.BinColor[1] = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        GlobalVariable.BinColor[2] = new Color(255f / 255f, 128f / 255f, 0f / 255f, 255f / 255f);
        GlobalVariable.BinColor[3] = new Color(255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
        GlobalVariable.BinColor[4] = new Color(128f / 255f, 42f / 255f, 0f / 42f, 255f / 255f);
        GlobalVariable.BinColor[5] = new Color(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        //GlobalVariable.CargosState = new bool[4, 8];

        //GlobalVariable.CargoStateList = new List<GlobalVariable.CargoState>();
        PilerNums = (this.GetComponent<ShowKeyPositionData>().KeyPositionsData.HighBaysNum + 1) / 2;
        GlobalVariable.PilersState = new State[PilerNums];
        GlobalVariable.UnidirectionalConveyorStates = new State[PilerNums + 4, 2];
        GlobalVariable.BidirectionalConveyorStates = new State[PilerNums, 3, 2];
        GlobalVariable.ConveyorDirections = new Direction[PilerNums];
        GlobalVariable.LiftTransferStates = new State[PilerNums + 1];
        GlobalVariable.ConveyorQueue = new Queue<GameObject>[PilerNums];
        GlobalVariable.PilerQueue = new Queue<GameObject>[PilerNums];
        GlobalVariable.Wait = new WaitState[PilerNums];
        GlobalVariable.EnterQueue = new Queue<GameObject>[PilerNums];
        GlobalVariable.ExitQueue = new Queue<GameObject>[PilerNums];
        GlobalVariable.PilerBodyPartPositions = new Vector3[PilerNums];
        #endregion
        //局部变量初始化操作
        #region
        Cargo = new GameObject[PilerNums];
        Piler = new GameObject[PilerNums];
        finish1 = new bool[PilerNums];
        finish2 = new bool[PilerNums];
        KeyFrame = new bool[PilerNums];
        for (int i = 0; i < PilerNums; i++)
        {
            KeyFrame[i] = false;
            finish1[i] = true;
            string PilerName = GlobalVariable.WareHouse.name + "/PilerGroup/Piler" + (i + 1).ToString();
            Piler[i] = GameObject.Find(PilerName);
            Piler[i].AddComponent<PilerProperty>().PilerState = State.Off;
            Piler[i].GetComponent<PilerProperty>().PilerNum = i + 1;
            Piler[i].GetComponent<PilerProperty>().WorkState = Direction.Enter;
            GlobalVariable.ConveyorQueue[i] = new Queue<GameObject>();
            GlobalVariable.PilerQueue[i] = new Queue<GameObject>();
            GlobalVariable.Wait[i] = WaitState.None;
            GlobalVariable.EnterQueue[i] = new Queue<GameObject>();
            GlobalVariable.ExitQueue[i] = new Queue<GameObject>();
            GlobalVariable.PilersState[i] = State.Off;
            GlobalVariable.UnidirectionalConveyorStates[i, 0] = State.Off; GlobalVariable.UnidirectionalConveyorStates[i, 1] = State.Off;
            GlobalVariable.ConveyorDirections[i] = Direction.Enter;
            GlobalVariable.LiftTransferStates[i] = State.Off;
            GlobalVariable.BidirectionalConveyorStates[i, 0, 0] = State.Off; GlobalVariable.BidirectionalConveyorStates[i, 0, 1] = State.Off;
            GlobalVariable.BidirectionalConveyorStates[i, 1, 0] = State.Off; GlobalVariable.BidirectionalConveyorStates[i, 1, 1] = State.Off;
            GlobalVariable.BidirectionalConveyorStates[i, 2, 0] = State.Off; GlobalVariable.BidirectionalConveyorStates[i, 2, 1] = State.Off;
            GlobalVariable.PilerBodyPartPositions[i] = GameObject.Find(this.name +"/PilerGroup").transform.Find("Piler" + (i + 1).ToString()).transform.Find("BodyPart").transform.localPosition;
        }
        GlobalVariable.UnidirectionalConveyorStates[PilerNums, 0] = State.Off; GlobalVariable.UnidirectionalConveyorStates[PilerNums, 1] = State.Off;
        GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 0] = State.Off; GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 1] = State.Off;
        GlobalVariable.UnidirectionalConveyorStates[PilerNums + 2, 0] = State.Off; GlobalVariable.UnidirectionalConveyorStates[PilerNums + 2, 1] = State.Off;
        GlobalVariable.UnidirectionalConveyorStates[PilerNums + 3, 0] = State.Off; GlobalVariable.UnidirectionalConveyorStates[PilerNums + 3, 1] = State.Off;
        GlobalVariable.LiftTransferStates[PilerNums] = State.Off;
        #endregion

        #region
        GameObject MainInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/MainInterface"));
        MainInterface.name = "MainInterface";
        GameObject ProcessInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/ProcessInterface"));
        ProcessInterface.name = "ProcessInterface";
        GameObject StorageStateInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/StorageStateInterface"));
        StorageStateInterface.name = "StorageStateInterface";
        GameObject EventSystem = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/EventSystem"));
        EventSystem.name = "EventSystem";
        #endregion

        GlobalVariable.UnidirectionalPositions = new Vector3[PilerNums + 4, 2];
        GlobalVariable.BidirectionalPositions = new Vector3[PilerNums, 3, 2];//每个输送线，6个关键点
        GlobalVariable.LiftTransferPositions = new Vector3[PilerNums + 1, 2];
        //GlobalVariable.LiftTransferPositions关键点坐标
        #region
        for (int i = 0; i <= PilerNums; i++)
        {
            GlobalVariable.LiftTransferPositions[i, 0] = GlobalVariable.KPD.EnterPosition;
            GlobalVariable.LiftTransferPositions[i, 0].x = GlobalVariable.KPD.EnterPosition.x - (i + 1) * GlobalVariable.KPD.ConveyorLengths[1]
                - i * GlobalVariable.KPD.ConveyorWidth - GlobalVariable.KPD.ConveyorWidth / 2;
            GlobalVariable.LiftTransferPositions[i, 1] = GlobalVariable.LiftTransferPositions[i, 0];
            GlobalVariable.LiftTransferPositions[i, 1].y = GlobalVariable.KPD.HighValues[0];
        }
        #endregion

        //GlobalVariable.UnidirectionalPositions关键点的坐标
        #region
        //GlobalVariable.UnidirectionalPositions[0] = GlobalVariable.KPD.EnterPosition;
        for (int i = 0; i <= PilerNums; i++)
        {
            GlobalVariable.UnidirectionalPositions[i, 0] = GlobalVariable.KPD.EnterPosition;
            GlobalVariable.UnidirectionalPositions[i, 0].x = GlobalVariable.KPD.EnterPosition.x - GlobalVariable.KPD.CargoSize.x / 2
                - i * (GlobalVariable.KPD.ConveyorLengths[1] + GlobalVariable.KPD.ConveyorWidth);
            GlobalVariable.UnidirectionalPositions[i, 1] = GlobalVariable.UnidirectionalPositions[i, 0];
            GlobalVariable.UnidirectionalPositions[i, 1].x = GlobalVariable.UnidirectionalPositions[i, 0].x
                - (GlobalVariable.KPD.ConveyorLengths[1] - GlobalVariable.KPD.CargoSize.x);
        }
        GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0] = GlobalVariable.LiftTransferPositions[PilerNums, 1];
        GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z = GlobalVariable.UnidirectionalPositions[PilerNums, 1].z
            + (GlobalVariable.KPD.ConveyorWidth + GlobalVariable.KPD.CargoSize.z) / 2 + 0.2f;
        GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1] = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1].z = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z
            + (GlobalVariable.KPD.ConveyorLengths[0] - GlobalVariable.KPD.CargoSize.z - 0.4f);
        GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0] = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0].z = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z + GlobalVariable.KPD.ConveyorLengths[0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1] = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1];
        GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1].z = GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1].z + GlobalVariable.KPD.ConveyorLengths[0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 3, 0] = GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 3, 0].z = GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0].z + GlobalVariable.KPD.ConveyorLengths[0];
        GlobalVariable.UnidirectionalPositions[PilerNums + 3, 1] = GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1];
        GlobalVariable.UnidirectionalPositions[PilerNums + 3, 1].z = GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1].z + GlobalVariable.KPD.ConveyorLengths[0];
        #endregion
        //GlobalVariable.BidirectionalPositions关键点坐标
        #region
        for (int i = 0; i < PilerNums; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GlobalVariable.BidirectionalPositions[i, j, 0] = GlobalVariable.LiftTransferPositions[i, 1];
                GlobalVariable.BidirectionalPositions[i, j, 0].z = GlobalVariable.LiftTransferPositions[i, 1].z
                    - GlobalVariable.KPD.ConveyorWidth / 2 - GlobalVariable.KPD.CargoSize.z / 2 - j * GlobalVariable.KPD.ConveyorLengths[0]-0.2F;
                GlobalVariable.BidirectionalPositions[i, j, 1] = GlobalVariable.BidirectionalPositions[i, j, 0];
                GlobalVariable.BidirectionalPositions[i, j, 1].z = GlobalVariable.BidirectionalPositions[i, j, 0].z
                    - (GlobalVariable.KPD.ConveyorLengths[0] - GlobalVariable.KPD.CargoSize.z-0.4F);
            }
        }

        #endregion
        GameObject Eventsystem = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName + "/Simulation/EventSystem"));
        Eventsystem.name = "EventSystem";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Times1 = Time.time;
        if (GlobalVariable.TempQueue.Count > 0)//检测货物运输需要用到的堆垛机
        {
            PilerNum = (GlobalVariable.TempQueue.Peek().GetComponent<ShowCargoInfo>().Cargomessage.PositionInfo.HighBayNum + 1) / 2;
        }

        for (int i = 0; i < PilerNums; i++)
        {
            //入库货物情况
            if (PilerNum == i + 1 && finish1[i] == true && GlobalVariable.TempQueue.Count > 0)
            {
                Cargo[i] = GlobalVariable.TempQueue.Peek();
                if (Cargo[i].GetComponent<OperatingState>().state == CargoState.WaitIn)
                {
                    //GlobalVariable.CargosState[i, 0] = true;
                    KeyFrame[i] = true;
                    Cargo[i].AddComponent<CargoEnter>().enabled = false;
                    Cargo[i].GetComponent<CargoEnter>().Speed = Speed;
                    Cargo[i].GetComponent<OperatingState>().state = CargoState.Enter;
                    GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(Cargo[i].name).transform.Find("State").GetComponent<Text>().text = "货物状态：" + "正在入库";//修改进程中该货物状态信息
                    Functions.ChangeColor(Cargo[i].name, StorageBinState.InStore);//修改该货物对应仓位的状态表示颜色
                    finish1[i] = false;
                    GlobalVariable.ConveyorQueue[i].Enqueue(Cargo[i]);
                    GlobalVariable.EnterQueue[i].Enqueue(Cargo[i]);
                    
                }
            }
            //入库货物入库动画
            if (GlobalVariable.EnterQueue[i].Count > 0)
            {
                foreach (GameObject cargo in GlobalVariable.EnterQueue[i])
                {
                    cargo.GetComponent<CargoEnter>().enabled = true;
                    //出队
                    if (GlobalVariable.UnidirectionalConveyorStates[0, 0] == State.On && finish1[i] == false)
                    {
                        GlobalVariable.TempQueue.Dequeue();
                        finish1[i] = true;
                    }
                }

            }
            //出库货物出库动画
            if (GlobalVariable.ExitQueue[i].Count > 0)
            {
                foreach (GameObject cargo in GlobalVariable.ExitQueue[i])
                {
                    cargo.GetComponent<CargoExit>().enabled = true;
                    //Debug.Log(0);
                }
            }
        }
        //堆垛机闲置时
        foreach (GameObject piler in Piler)
        {
            int Num = piler.GetComponent<PilerProperty>().PilerNum - 1;//堆垛机编号
            //添加动画脚本
            if (GlobalVariable.PilersState[Num] == State.Off && GlobalVariable.ConveyorQueue[Num].Count > 0)
            {
                GameObject cargo0 = GlobalVariable.ConveyorQueue[Num].Peek();
                //添加入库动画脚本
                if (cargo0.GetComponent<OperatingState>().state == CargoState.Enter && GlobalVariable.Wait[Num] == WaitState.WaitEnter)
                {
                    piler.AddComponent<PilerOfEnter>().enabled = false;
                    piler.GetComponent<PilerOfEnter>().Speed = Speed;
                    piler.GetComponent<PilerOfEnter>().Cargo = cargo0;
                    piler.GetComponent<PilerOfEnter>().PilerNum = Num;
                    GlobalVariable.PilersState[Num] = State.On;
                    piler.GetComponent<PilerProperty>().PilerState = State.On;
                    piler.GetComponent<PilerProperty>().WorkState = Direction.Enter;
                }
                //添加出库动画脚本
                if (cargo0.GetComponent<OperatingState>().state == CargoState.WaitOut && GlobalVariable.Wait[Num] == WaitState.None)
                {
                    piler.AddComponent<PilerOfExit>().enabled = false;
                    piler.GetComponent<PilerOfExit>().Speed = Speed;
                    piler.GetComponent<PilerOfExit>().Cargo = GlobalVariable.ConveyorQueue[Num].Peek();
                    piler.GetComponent<PilerOfExit>().PilerNum = Num;
                    GlobalVariable.PilersState[Num] = State.On;
                    piler.GetComponent<PilerProperty>().PilerState = State.On;
                    piler.GetComponent<PilerProperty>().WorkState = Direction.Exit;
                    GlobalVariable.BidirectionalConveyorStates[Num, 2, 1] = State.On;
                }
            }
            //执行动画脚本
            if (GlobalVariable.PilersState[Num] == State.On)
            {
                //入库脚本
                if (piler.GetComponent<PilerProperty>().WorkState == Direction.Enter)
                {
                    piler.GetComponent<PilerOfEnter>().enabled = true;
                }
                //出库脚本
                if (piler.GetComponent<PilerProperty>().WorkState == Direction.Exit)
                {
                    piler.GetComponent<PilerOfExit>().enabled = true;
                }
            }
        }
    }

    

}
