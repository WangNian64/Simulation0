using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilerOfEnter : MonoBehaviour {

    private GameObject OBJ1;
    private GameObject OBJ2;
    private GameObject PilerBodyPart;
    private GameObject PilerUpPart;
    private GameObject PilerForks;
    private GameObject PilerFork1;
    public GameObject Cargo;
    //public string CargoName;//运输货物对象名称

    public bool Accomplish;
    public float Speed;

    //private KeyPositionsData KPD; //场景关键数据
    private CargoMessage CM; //货物信息

    private Vector3[] BodyPartPositions;//堆垛机主体部分关键坐标
    private Vector3[] UpPartPositions;//堆垛机上下移动部分关键坐标
    private Vector3[] ForksPositions;//堆垛机叉组移动关键坐标
    private Vector3[] Fork1Positions;//堆垛机叉1移动关键坐标
    private Vector3 TargetPosition;//货物最终位置
    private Vector3 ForkSize;//货叉尺寸
    public bool[] finish;//判断条件
    private int PlaceNum;
    public int PilerNum;
    private float high;//货物座空隙高度
    private Vector3 RotationAngle;
    // Use this for initialization
    void Start () {
        //this.GetComponent<OperatingState>().state = State.On;//堆垛机工作状态为on
        OBJ1 = GlobalVariable.WareHouse;
        OBJ2 = OBJ1.transform.Find("Cargos").gameObject;
        //KPD = GameObject.Find("WarehouseScene").GetComponent<ShowKeyPositionData>().KeyPositionsData;
        CM = Cargo.GetComponent<ShowCargoInfo>().Cargomessage;
        RotationAngle = new Vector3(0, 0, 0) - OBJ1.transform.eulerAngles;
        //设计移动速度
        Speed = 0.8f;
        GlobalVariable.PilersState[PilerNum] = State.On;
        int Tempi1 = CM.PositionInfo.HighBayNum; int Tempi2 = (Tempi1 + 1) / 2;
        high = 0.12f;
        if (CM.PositionInfo.place == 0)
        {
            PlaceNum = 0;
        }
        else
        {
            PlaceNum = 1;
        }
        finish = new bool[7];
        finish[0] = false; finish[1] = false; finish[2] = false; finish[3] = false; finish[4] = false; finish[5] = false; finish[6] = false;
        #region  堆垛机动画设计
        //获取相关对象
        //Cargo = GameObject.Find(CargoName);//对应的货物对象
        Vector3 CargoPosition = Cargo.transform.position;
        string PilerName = this.name;// + "/PilerGroup/Piler" + Tempi2.ToString();
        //GameObject Piler = GameObject.Find(PilerName);
        Vector3 PilerPosition = this.transform.position;//堆垛机的坐标
        string HighBayName = OBJ1.name + "/HighBayGroup/HighBayGroup/HighBay" + Tempi1.ToString();
        GameObject HighBay = GameObject.Find(HighBayName);
        Vector3 HighBayPosition = HighBay.transform.position;//高架库坐标
        string BodyPartName = PilerName + "/BodyPart";
        PilerBodyPart = GameObject.Find(BodyPartName);//堆垛机主体
        Vector3 PilerBodyPartPosition = PilerBodyPart.transform.localPosition;//堆垛机主体局部坐标
        //Debug.Log(PilerBodyPartPosition);
        string UpPartName = BodyPartName + "/UpPart";
        PilerUpPart = GameObject.Find(UpPartName);//堆垛机上下移动部分
        Vector3 PilerUpPartPosition = PilerUpPart.transform.localPosition;//堆垛机上下移动部分局部坐标
        string ForksName = UpPartName + "/Forks"; PilerForks = GameObject.Find(ForksName);

        Vector3 ForksSize = new Vector3(0, 0, 0);
        MyClass.MeshSize(PilerForks, ref ForksSize);
        Vector3 ForksPosition = PilerForks.transform.localPosition;
        string Fork1Name = ForksName + "/Fork1"; PilerFork1 = GameObject.Find(Fork1Name);
        Vector3 Fork1Position = PilerFork1.transform.localPosition;

        //堆垛机主体移动关键位置坐标
        string MainShelfName = OBJ1.name + "/HighBayGroup/HighBayGroup/HighBay" + CM.PositionInfo.HighBayNum.ToString() + "/MainShelf";
        MainShelfName = MainShelfName + "_" + CM.PositionInfo.FloorNum.ToString() + "_" + CM.PositionInfo.ColumnNum.ToString();
        GameObject MainShelf = GameObject.Find(MainShelfName);
        Vector3 MainShelfPosition = MainShelf.transform.position;
        TargetPosition = MainShelfPosition;
        TargetPosition.z = TargetPosition.z + GlobalVariable.KPD.StorePositions.StorePlacePosition[PlaceNum];
        BodyPartPositions = new Vector3[5];//堆垛机BodyPart目标位置0
        //Vector3 position = Quaternion.Euler(RotationAngle) * (CargoPosition - PilerForks.transform.position);
        float TempValue1 = (Quaternion.Euler(RotationAngle) * (CargoPosition - PilerForks.transform.position)).z;
        //float tempvalue1 = MainShelfPosition.z - KeyPositions[3].z;
        BodyPartPositions[0] = PilerBodyPart.transform.localPosition;//Body初始位置
        BodyPartPositions[1] = BodyPartPositions[0];
        BodyPartPositions[1].z = BodyPartPositions[0].z + TempValue1;//Body关键位置1
        BodyPartPositions[2] = BodyPartPositions[1];
        float TempValue1_1 = (Quaternion.Euler(RotationAngle) * (MainShelfPosition- CargoPosition)).z + GlobalVariable.KPD.StorePositions.StorePlacePosition[PlaceNum];
        BodyPartPositions[2].z = BodyPartPositions[1].z + TempValue1_1;
        //上下移动部分关键位置坐标
        UpPartPositions = new Vector3[7];
        float TempValue2 = (Quaternion.Euler(RotationAngle) * (CargoPosition - PilerForks.transform.position)).y + ForksSize.y / 2;
        UpPartPositions[0] = PilerUpPart.transform.localPosition;//堆垛机UpPart目标位置0
        UpPartPositions[1] = UpPartPositions[0]; UpPartPositions[1].y = UpPartPositions[0].y + TempValue2;//堆垛机UpPart目标位置1
        UpPartPositions[2] = UpPartPositions[1];
        UpPartPositions[2].y = UpPartPositions[1].y + high - ForksSize.y;//堆垛机UpPart目标位置2
        //Debug.Log(high - TempValue3);
        UpPartPositions[3] = UpPartPositions[2];
        UpPartPositions[3].y = UpPartPositions[2].y + 0.2f;//堆垛机UpPart目标位置3
        UpPartPositions[4] = UpPartPositions[3];
        float TempValue2_1 = (Quaternion.Euler(RotationAngle) * (MainShelfPosition - CargoPosition)).y + high - ForksSize.y + 0.2f;
        UpPartPositions[4].y = UpPartPositions[1].y + TempValue2_1;//堆垛机UpPart目标位置4
        UpPartPositions[5] = UpPartPositions[4];
        UpPartPositions[5].y = UpPartPositions[1].y + TempValue2_1 - 0.2f;//堆垛机UpPart目标位置6
        UpPartPositions[6] = UpPartPositions[5];
        UpPartPositions[6].y = UpPartPositions[5].y - (high - ForksSize.y);//堆垛机UpPart目标位置6

        //货叉关键坐标
        float TempValue3 = 2 * ForksSize.x / 3;//Debug.Log(TempValue3);
        ForksPositions = new Vector3[3];
        ForksPositions[0] = PilerForks.transform.localPosition;//叉组位置0
        ForksPositions[1] = ForksPositions[0];
        ForksPositions[1].x = ForksPositions[1].x + TempValue3;//叉组位置1

        Fork1Positions = new Vector3[3];
        float TempValue4 = (Quaternion.Euler(RotationAngle) * (CargoPosition - PilerPosition)).x - TempValue3;
        Fork1Positions[0] = PilerFork1.transform.localPosition;//叉1位置0
        Fork1Positions[1] = Fork1Positions[0];
        Fork1Positions[1].x = Fork1Positions[1].x + TempValue4;//叉1位置1
        float TempValue4_1 = Mathf.Abs((Quaternion.Euler(RotationAngle) * (HighBayPosition - PilerPosition)).x);
        switch (Tempi1 % 2)
        {
            case 1:
                ForksPositions[2] = ForksPositions[0]; ForksPositions[2].x = ForksPositions[0].x + TempValue3;//叉组位置2
                Fork1Positions[2] = Fork1Positions[0]; Fork1Positions[2].x = Fork1Positions[0].x + TempValue4_1 - TempValue3;//叉1位置2
                break;
            case 0:
                ForksPositions[2] = ForksPositions[0]; ForksPositions[2].x = ForksPositions[0].x - TempValue3;
                Fork1Positions[2] = Fork1Positions[0]; Fork1Positions[2].x = Fork1Positions[0].x - TempValue4_1 + TempValue3;
                break;
        }


        #endregion
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (finish[0] == false)
        {
            PilerBodyPart.transform.localPosition = Vector3.MoveTowards(PilerBodyPart.transform.localPosition, BodyPartPositions[1], Speed * Time.deltaTime);
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[1], Speed * Time.deltaTime);
            if (PilerBodyPart.transform.localPosition == BodyPartPositions[1])
            {
                if (PilerUpPart.transform.localPosition == UpPartPositions[1])
                {
                    PilerForks.transform.localPosition = Vector3.MoveTowards(PilerForks.transform.localPosition, ForksPositions[1], Speed * Time.deltaTime);
                    if (PilerForks.transform.localPosition == ForksPositions[1])
                    {
                        PilerFork1.transform.localPosition = Vector3.MoveTowards(PilerFork1.transform.localPosition, Fork1Positions[1], Speed * Time.deltaTime);
                        if (PilerFork1.transform.localPosition == Fork1Positions[1])
                        {
                            finish[0] = true;
                        }
                    }
                }
            }
        }

        if (finish[0] == true && finish[1] == false)
        {
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[2], Speed * Time.deltaTime);
            if (PilerUpPart.transform.localPosition == UpPartPositions[2])
            {
                Cargo.transform.parent = PilerFork1.transform;
                finish[1] = true;
            }
        }

        if (finish[1]==true && finish[2]==false)
        {
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[3], Speed * Time.deltaTime);
            if (PilerUpPart.transform.localPosition == UpPartPositions[3])
            {
                PilerFork1.transform.localPosition = Vector3.MoveTowards(PilerFork1.transform.localPosition, Fork1Positions[0], Speed * Time.deltaTime);
                if (PilerFork1.transform.localPosition == Fork1Positions[0])
                {
                    PilerForks.transform.localPosition = Vector3.MoveTowards(PilerForks.transform.localPosition, ForksPositions[0], Speed * Time.deltaTime);
                    if (PilerForks.transform.localPosition == ForksPositions[0])
                    {
                        finish[2] = true;
                        GlobalVariable.Wait[PilerNum] = WaitState.None;
                        GlobalVariable.BidirectionalConveyorStates[PilerNum, 2, 1] = State.Off;
                        GlobalVariable.ConveyorQueue[PilerNum].Dequeue();
                        GlobalVariable.EnterQueue[PilerNum].Dequeue();
                        DestroyImmediate(Cargo.GetComponent<CargoEnter>());
                    }
                }
            }
        }
        
        if (finish[2] == true && finish[3] == false)
        {
            PilerBodyPart.transform.localPosition = Vector3.MoveTowards(PilerBodyPart.transform.localPosition, BodyPartPositions[2], Speed * Time.deltaTime);
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[4], Speed * Time.deltaTime);
            if (PilerBodyPart.transform.localPosition == BodyPartPositions[2] && PilerUpPart.transform.localPosition == UpPartPositions[4])
            {
                PilerForks.transform.localPosition = Vector3.MoveTowards(PilerForks.transform.localPosition, ForksPositions[2], Speed * Time.deltaTime);
                if (PilerForks.transform.localPosition == ForksPositions[2])
                {
                    PilerFork1.transform.localPosition = Vector3.MoveTowards(PilerFork1.transform.localPosition, Fork1Positions[2], Speed * Time.deltaTime);
                    if (PilerFork1.transform.localPosition == Fork1Positions[2])
                    {
                        finish[3] = true;
                    }
                }
            }
        }

        if (finish[3] == true && finish[4] == false)
        {
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[5], Speed * Time.deltaTime);
            if (PilerUpPart.transform.localPosition == UpPartPositions[5])
            {
                Cargo.transform.parent = OBJ2.transform;
                Cargo.GetComponent<OperatingState>().state = CargoState.Stored;
                GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(Cargo.name).transform.Find("State").GetComponent<Text>().text = "货物状态：" + "完成入库";//修改该货物进程面板中的状态信息
                Functions.ChangeColor(Cargo.name, StorageBinState.Stored);//修改状态面板中该货物对应仓位的颜色表示
                finish[4] = true;
            }
        }


        if (finish[4] == true && finish[5]==false)
        {
            PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[6], Speed * Time.deltaTime);
            if (PilerUpPart.transform.localPosition == UpPartPositions[6])
            {
                PilerFork1.transform.localPosition = Vector3.MoveTowards(PilerFork1.transform.localPosition, Fork1Positions[0], Speed * Time.deltaTime);
                if (PilerFork1.transform.localPosition == Fork1Positions[0])
                {
                    PilerForks.transform.localPosition = Vector3.MoveTowards(PilerForks.transform.localPosition, ForksPositions[0], Speed * Time.deltaTime);
                    if (PilerForks.transform.localPosition == ForksPositions[0])
                    {
                        finish[5] = true;
                        //原地结束
                        Accomplish = true;
                        finish[6] = true;
                        //GlobalVariable.CargosState[PilerNum, 5] = true;
                        GlobalVariable.PilersState[PilerNum] = State.Off;
                        DestroyImmediate(GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(Cargo.name).gameObject);
                        this.GetComponent<PilerProperty>().PilerState = State.Off;
                        DestroyImmediate(this.GetComponent<PilerOfEnter>());
                    }
                }
            }
        }
        
        //if (finish[5] == true && finish[6] == false)
        //{
        //    PilerBodyPart.transform.localPosition = Vector3.MoveTowards(PilerBodyPart.transform.localPosition, BodyPartPositions[1], Speed * Time.deltaTime);
        //    PilerUpPart.transform.localPosition = Vector3.MoveTowards(PilerUpPart.transform.localPosition, UpPartPositions[1], Speed * Time.deltaTime);
        //    if (PilerBodyPart.transform.localPosition == BodyPartPositions[1] && PilerUpPart.transform.localPosition == UpPartPositions[1])
        //    {
        //        Accomplish = true;
        //        finish[6] = true;
        //        //GlobalVariable.CargosState[PilerNum, 5] = true;
        //        GlobalVariable.PilersState[PilerNum] = State.Off;
        //        DestroyImmediate(GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform.Find(Cargo.name).gameObject);
        //        this.GetComponent<PilerProperty>().PilerState = State.Off;
        //        DestroyImmediate(this.GetComponent<PilerOfEnter>());
        //    }
        //}
	}
}
