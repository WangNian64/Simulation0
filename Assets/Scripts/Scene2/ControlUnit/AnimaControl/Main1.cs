using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
namespace BlackBox.WareHouse.ControlUnit.AnimaControl
{
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
            GameObject.Find("Main Camera").AddComponent<CameraControl.InitCamera>(); Debug.Log(0);
            GameObject.Find("Main Camera").AddComponent<CameraControl.FiCameraControl>(); Debug.Log(1);
            //设置资源文件夹名
            Varibles.GlobalVariable.RootName = "Warehouse_Material";
            //参数计算
            Speed = 1.5f;
            //全局变量初始化操作
            #region 
            Varibles.GlobalVariable.KPD = this.GetComponent<ShowData.ShowKeyPositionData>().KeyPositionsData;
            Varibles.GlobalVariable.WareHouse = this.gameObject;
            int CargosNum = Varibles.GlobalVariable.KPD.HighBaysNum * Varibles.GlobalVariable.KPD.StorePositions.StoreColumnPositions.Length * Varibles.GlobalVariable.KPD.StorePositions.StoreFloorPositions.Length * 2;
            Varibles.GlobalVariable.TempQueue = new Queue<GameObject>();
            int HighBayNum = Varibles.GlobalVariable.KPD.HighBaysNum; int ColumnNum = Varibles.GlobalVariable.KPD.StorePositions.StoreColumnPositions.Length;
            int FloorNum = Varibles.GlobalVariable.KPD.StorePositions.StoreFloorPositions.Length; int PlaceNum = Varibles.GlobalVariable.KPD.StorePositions.StorePlacePosition.Length;
            Varibles.GlobalVariable.BinState = new Varibles.StorageBinState[HighBayNum, FloorNum, ColumnNum, PlaceNum];
            //GlobalVariable.EnterCargosList = new List<GameObject>();
            Varibles.GlobalVariable.EnterCargosNameList = new List<string>();
            //GlobalVariable.ExitCargosList = new List<GameObject>();
            Varibles.GlobalVariable.TempQueue = new Queue<GameObject>();
            Varibles.GlobalVariable.BinColor = new Color[6];
            Varibles.GlobalVariable.BinColor[0] = new Color(192f / 255f, 192f / 255f, 192f / 255f, 255f / 255f);
            Varibles.GlobalVariable.BinColor[1] = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
            Varibles.GlobalVariable.BinColor[2] = new Color(255f / 255f, 128f / 255f, 0f / 255f, 255f / 255f);
            Varibles.GlobalVariable.BinColor[3] = new Color(255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
            Varibles.GlobalVariable.BinColor[4] = new Color(128f / 255f, 42f / 255f, 0f / 42f, 255f / 255f);
            Varibles.GlobalVariable.BinColor[5] = new Color(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
            //GlobalVariable.CargosState = new bool[4, 8];

            //GlobalVariable.CargoStateList = new List<GlobalVariable.CargoState>();
            PilerNums = (this.GetComponent<ShowData.ShowKeyPositionData>().KeyPositionsData.HighBaysNum + 1) / 2;
            Varibles.GlobalVariable.PilersState = new Varibles.State[PilerNums];
            Varibles.GlobalVariable.UnidirectionalConveyorStates = new Varibles.State[PilerNums + 4, 2];
            Varibles.GlobalVariable.BidirectionalConveyorStates = new Varibles.State[PilerNums, 3, 2];
            Varibles.GlobalVariable.ConveyorDirections = new Varibles.Direction[PilerNums];
            Varibles.GlobalVariable.LiftTransferStates = new Varibles.State[PilerNums + 1];
            Varibles.GlobalVariable.ConveyorQueue = new Queue<GameObject>[PilerNums];
            Varibles.GlobalVariable.PilerQueue = new Queue<GameObject>[PilerNums];
            Varibles.GlobalVariable.Wait = new Varibles.WaitState[PilerNums];
            Varibles.GlobalVariable.EnterQueue = new Queue<GameObject>[PilerNums];
            Varibles.GlobalVariable.ExitQueue = new Queue<GameObject>[PilerNums];
            Varibles.GlobalVariable.PilerBodyPartPositions = new Vector3[PilerNums];
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
                string PilerName = Varibles.GlobalVariable.WareHouse.name + "/PilerGroup/Piler" + (i + 1).ToString();
                Piler[i] = GameObject.Find(PilerName);
                Piler[i].AddComponent<Varibles.PilerProperty>().PilerState = Varibles.State.Off;
                Piler[i].GetComponent<Varibles.PilerProperty>().PilerNum = i + 1;
                Piler[i].GetComponent<Varibles.PilerProperty>().WorkState = Varibles.Direction.Enter;
                Varibles.GlobalVariable.ConveyorQueue[i] = new Queue<GameObject>();
                Varibles.GlobalVariable.PilerQueue[i] = new Queue<GameObject>();
                Varibles.GlobalVariable.Wait[i] = Varibles.WaitState.None;
                Varibles.GlobalVariable.EnterQueue[i] = new Queue<GameObject>();
                Varibles.GlobalVariable.ExitQueue[i] = new Queue<GameObject>();
                Varibles.GlobalVariable.PilersState[i] = Varibles.State.Off;
                Varibles.GlobalVariable.UnidirectionalConveyorStates[i, 0] = Varibles.State.Off; Varibles.GlobalVariable.UnidirectionalConveyorStates[i, 1] = Varibles.State.Off;
                Varibles.GlobalVariable.ConveyorDirections[i] = Varibles.Direction.Enter;
                Varibles.GlobalVariable.LiftTransferStates[i] = Varibles.State.Off;
                Varibles.GlobalVariable.BidirectionalConveyorStates[i, 0, 0] = Varibles.State.Off; Varibles.GlobalVariable.BidirectionalConveyorStates[i, 0, 1] = Varibles.State.Off;
                Varibles.GlobalVariable.BidirectionalConveyorStates[i, 1, 0] = Varibles.State.Off; Varibles.GlobalVariable.BidirectionalConveyorStates[i, 1, 1] = Varibles.State.Off;
                Varibles.GlobalVariable.BidirectionalConveyorStates[i, 2, 0] = Varibles.State.Off; Varibles.GlobalVariable.BidirectionalConveyorStates[i, 2, 1] = Varibles.State.Off;
                Varibles.GlobalVariable.PilerBodyPartPositions[i] = GameObject.Find(this.name + "/PilerGroup").transform.Find("Piler" + (i + 1).ToString()).transform.Find("BodyPart").transform.localPosition;
            }
            Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums, 0] = Varibles.State.Off; Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums, 1] = Varibles.State.Off;
            Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 0] = Varibles.State.Off; Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 1, 1] = Varibles.State.Off;
            Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 2, 0] = Varibles.State.Off; Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 2, 1] = Varibles.State.Off;
            Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 3, 0] = Varibles.State.Off; Varibles.GlobalVariable.UnidirectionalConveyorStates[PilerNums + 3, 1] = Varibles.State.Off;
            Varibles.GlobalVariable.LiftTransferStates[PilerNums] = Varibles.State.Off;
            #endregion

            #region
            GameObject MainInterface = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/MainInterface"));
            MainInterface.name = "MainInterface";
            GameObject ProcessInterface = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/ProcessInterface"));
            ProcessInterface.name = "ProcessInterface";
            GameObject StorageStateInterface = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/StorageStateInterface"));
            StorageStateInterface.name = "StorageStateInterface";
            GameObject EventSystem = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/EventSystem"));
            EventSystem.name = "EventSystem";
            #endregion

            Varibles.GlobalVariable.UnidirectionalPositions = new Vector3[PilerNums + 4, 2];
            Varibles.GlobalVariable.BidirectionalPositions = new Vector3[PilerNums, 3, 2];//每个输送线，6个关键点
            Varibles.GlobalVariable.LiftTransferPositions = new Vector3[PilerNums + 1, 2];
            //GlobalVariable.LiftTransferPositions关键点坐标
            #region
            for (int i = 0; i <= PilerNums; i++)
            {
                Varibles.GlobalVariable.LiftTransferPositions[i, 0] = Varibles.GlobalVariable.KPD.EnterPosition;
                Varibles.GlobalVariable.LiftTransferPositions[i, 0].x = Varibles.GlobalVariable.KPD.EnterPosition.x - (i + 1) * Varibles.GlobalVariable.KPD.ConveyorLengths[1]
                    - i * Varibles.GlobalVariable.KPD.ConveyorWidth - Varibles.GlobalVariable.KPD.ConveyorWidth / 2;
                Varibles.GlobalVariable.LiftTransferPositions[i, 1] = Varibles.GlobalVariable.LiftTransferPositions[i, 0];
                Varibles.GlobalVariable.LiftTransferPositions[i, 1].y = Varibles.GlobalVariable.KPD.HighValues[0];
            }
            #endregion

            //GlobalVariable.UnidirectionalPositions关键点的坐标
            #region
            //GlobalVariable.UnidirectionalPositions[0] = GlobalVariable.KPD.EnterPosition;
            for (int i = 0; i <= PilerNums; i++)
            {
                Varibles.GlobalVariable.UnidirectionalPositions[i, 0] = Varibles.GlobalVariable.KPD.EnterPosition;
                Varibles.GlobalVariable.UnidirectionalPositions[i, 0].x = Varibles.GlobalVariable.KPD.EnterPosition.x - Varibles.GlobalVariable.KPD.CargoSize.x / 2
                    - i * (Varibles.GlobalVariable.KPD.ConveyorLengths[1] + Varibles.GlobalVariable.KPD.ConveyorWidth);
                Varibles.GlobalVariable.UnidirectionalPositions[i, 1] = Varibles.GlobalVariable.UnidirectionalPositions[i, 0];
                Varibles.GlobalVariable.UnidirectionalPositions[i, 1].x = Varibles.GlobalVariable.UnidirectionalPositions[i, 0].x
                    - (Varibles.GlobalVariable.KPD.ConveyorLengths[1] - Varibles.GlobalVariable.KPD.CargoSize.x);
            }
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0] = Varibles.GlobalVariable.LiftTransferPositions[PilerNums, 1];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums, 1].z
                + (Varibles.GlobalVariable.KPD.ConveyorWidth + Varibles.GlobalVariable.KPD.CargoSize.z) / 2 + 0.2f;
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1] = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z
                + (Varibles.GlobalVariable.KPD.ConveyorLengths[0] - Varibles.GlobalVariable.KPD.CargoSize.z - 0.4f);
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0] = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 0].z + Varibles.GlobalVariable.KPD.ConveyorLengths[0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1] = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 1, 1].z + Varibles.GlobalVariable.KPD.ConveyorLengths[0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 3, 0] = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 3, 0].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 0].z + Varibles.GlobalVariable.KPD.ConveyorLengths[0];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 3, 1] = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1];
            Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 3, 1].z = Varibles.GlobalVariable.UnidirectionalPositions[PilerNums + 2, 1].z + Varibles.GlobalVariable.KPD.ConveyorLengths[0];
            #endregion
            //GlobalVariable.BidirectionalPositions关键点坐标
            #region
            for (int i = 0; i < PilerNums; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Varibles.GlobalVariable.BidirectionalPositions[i, j, 0] = Varibles.GlobalVariable.LiftTransferPositions[i, 1];
                    Varibles.GlobalVariable.BidirectionalPositions[i, j, 0].z = Varibles.GlobalVariable.LiftTransferPositions[i, 1].z
                        - Varibles.GlobalVariable.KPD.ConveyorWidth / 2 - Varibles.GlobalVariable.KPD.CargoSize.z / 2 - j * Varibles.GlobalVariable.KPD.ConveyorLengths[0] - 0.2F;
                    Varibles.GlobalVariable.BidirectionalPositions[i, j, 1] = Varibles.GlobalVariable.BidirectionalPositions[i, j, 0];
                    Varibles.GlobalVariable.BidirectionalPositions[i, j, 1].z = Varibles.GlobalVariable.BidirectionalPositions[i, j, 0].z
                        - (Varibles.GlobalVariable.KPD.ConveyorLengths[0] - Varibles.GlobalVariable.KPD.CargoSize.z - 0.4F);
                }
            }

            #endregion
            GameObject Eventsystem = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/EventSystem"));
            Eventsystem.name = "EventSystem";
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Times1 = Time.time;
            if (Varibles.GlobalVariable.TempQueue.Count > 0)//检测货物运输需要用到的堆垛机
            {
                PilerNum = (Varibles.GlobalVariable.TempQueue.Peek().GetComponent<ShowData.ShowCargoInfo>().Cargomessage.PositionInfo.HighBayNum + 1) / 2;
            }

            for (int i = 0; i < PilerNums; i++)
            {
                //入库货物情况
                if (PilerNum == i + 1 && finish1[i] == true && Varibles.GlobalVariable.TempQueue.Count > 0)
                {
                    Cargo[i] = Varibles.GlobalVariable.TempQueue.Peek();
                    if (Cargo[i].GetComponent<Varibles.OperatingState>().state == Varibles.CargoState.WaitIn)
                    {
                        //GlobalVariable.CargosState[i, 0] = true;
                        KeyFrame[i] = true;
                        Cargo[i].AddComponent<CargoEnter>().enabled = false;
                        Cargo[i].GetComponent<CargoEnter>().Speed = Speed;
                        Cargo[i].GetComponent<Varibles.OperatingState>().state = Varibles.CargoState.Enter;
                        GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(Cargo[i].name).transform.Find("State").GetComponent<Text>().text = "货物状态：" + "正在入库";//修改进程中该货物状态信息
                        Tools.Functions.ChangeColor(Cargo[i].name, Varibles.StorageBinState.InStore);//修改该货物对应仓位的状态表示颜色
                        finish1[i] = false;
                        Varibles.GlobalVariable.ConveyorQueue[i].Enqueue(Cargo[i]);
                        Varibles.GlobalVariable.EnterQueue[i].Enqueue(Cargo[i]);

                    }
                }
                //入库货物入库动画
                if (Varibles.GlobalVariable.EnterQueue[i].Count > 0)
                {
                    foreach (GameObject cargo in Varibles.GlobalVariable.EnterQueue[i])
                    {
                        cargo.GetComponent<CargoEnter>().enabled = true;
                        //出队
                        if (Varibles.GlobalVariable.UnidirectionalConveyorStates[0, 0] == Varibles.State.On && finish1[i] == false)
                        {
                            Varibles.GlobalVariable.TempQueue.Dequeue();
                            finish1[i] = true;
                        }
                    }

                }
                //出库货物出库动画
                if (Varibles.GlobalVariable.ExitQueue[i].Count > 0)
                {
                    foreach (GameObject cargo in Varibles.GlobalVariable.ExitQueue[i])
                    {
                        cargo.GetComponent<CargoExit>().enabled = true;
                        //Debug.Log(0);
                    }
                }
            }
            //堆垛机闲置时
            foreach (GameObject piler in Piler)
            {
                int Num = piler.GetComponent<Varibles.PilerProperty>().PilerNum - 1;//堆垛机编号
                                                                           //添加动画脚本
                if (Varibles.GlobalVariable.PilersState[Num] == Varibles.State.Off && Varibles.GlobalVariable.ConveyorQueue[Num].Count > 0)
                {
                    GameObject cargo0 = Varibles.GlobalVariable.ConveyorQueue[Num].Peek();
                    //添加入库动画脚本
                    if (cargo0.GetComponent<Varibles.OperatingState>().state == Varibles.CargoState.Enter && Varibles.GlobalVariable.Wait[Num] == Varibles.WaitState.WaitEnter)
                    {
                        piler.AddComponent<PilerOfEnter>().enabled = false;
                        piler.GetComponent<PilerOfEnter>().Speed = Speed;
                        piler.GetComponent<PilerOfEnter>().Cargo = cargo0;
                        piler.GetComponent<PilerOfEnter>().PilerNum = Num;
                        Varibles.GlobalVariable.PilersState[Num] = Varibles.State.On;
                        piler.GetComponent<Varibles.PilerProperty>().PilerState = Varibles.State.On;
                        piler.GetComponent<Varibles.PilerProperty>().WorkState = Varibles.Direction.Enter;
                    }
                    //添加出库动画脚本
                    if (cargo0.GetComponent<Varibles.OperatingState>().state == Varibles.CargoState.WaitOut && Varibles.GlobalVariable.Wait[Num] == Varibles.WaitState.None)
                    {
                        piler.AddComponent<PilerOfExit>().enabled = false;
                        piler.GetComponent<PilerOfExit>().Speed = Speed;
                        piler.GetComponent<PilerOfExit>().Cargo = Varibles.GlobalVariable.ConveyorQueue[Num].Peek();
                        piler.GetComponent<PilerOfExit>().PilerNum = Num;
                        Varibles.GlobalVariable.PilersState[Num] = Varibles.State.On;
                        piler.GetComponent<Varibles.PilerProperty>().PilerState = Varibles.State.On;
                        piler.GetComponent<Varibles.PilerProperty>().WorkState = Varibles.Direction.Exit;
                        Varibles.GlobalVariable.BidirectionalConveyorStates[Num, 2, 1] = Varibles.State.On;
                    }
                }
                //执行动画脚本
                if (Varibles.GlobalVariable.PilersState[Num] == Varibles.State.On)
                {
                    //入库脚本
                    if (piler.GetComponent<Varibles.PilerProperty>().WorkState == Varibles.Direction.Enter)
                    {
                        piler.GetComponent<PilerOfEnter>().enabled = true;
                    }
                    //出库脚本
                    if (piler.GetComponent<Varibles.PilerProperty>().WorkState == Varibles.Direction.Exit)
                    {
                        piler.GetComponent<PilerOfExit>().enabled = true;
                    }
                }
            }
        }
    }
}

