using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BlackBox.WareHouse.ControlUnit.UIControl
{
    public class CargoExitButton : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CargoExit()
        {

            string CargoName = this.transform.parent.transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text;
            GameObject Cargo = GameObject.Find(CargoName);

            Varibles.CargoMessage CM = Cargo.GetComponent<ShowData.ShowCargoInfo>().Cargomessage;
            int HighBayNum = CM.PositionInfo.HighBayNum; int FloorNum = CM.PositionInfo.FloorNum;
            int ColumnNum = CM.PositionInfo.ColumnNum; Varibles.Place PlaceNum = CM.PositionInfo.place;
            Varibles.StorageBinState state = Varibles.StorageBinState.InStore;
            //int NumofPlace;
            switch (PlaceNum)
            {
                case Varibles.Place.A:
                    state = Varibles.GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 0];// = GlobalVariable.StorageBinState.Stay2Exit;
                                                                                                    //NumofPlace = 0;
                    break;
                case Varibles.Place.B:
                    state = Varibles.GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 1];// = GlobalVariable.StorageBinState.Stay2Exit;
                                                                                                    //NumofPlace = 1;
                    break;
            }
            //Debug.Log(state.ToString());

            if (state == Varibles.StorageBinState.Stored)
            {
                string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum.ToString();
                BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum.ToString();
                BinName = BinName + "/" + PlaceNum + "Panel/Bin_" + CargoName;
                //GlobalVariable.ExitCargosList.Add(Cargo);//出库列表增加该货物
                //GlobalVariable.TempQueue.Enqueue(Cargo);//临时队列增加该货物
                Varibles.GlobalVariable.ConveyorQueue[(HighBayNum + 1) / 2 - 1].Enqueue(Cargo);//出库货物加入队列
                                                                                               //GlobalVariable.ExitQueue[(HighBayNum + 1) / 2 - 1].Enqueue(Cargo);
                Varibles.GlobalVariable.ConveyorDirections[(HighBayNum + 1) / 2 - 1] = Varibles.Direction.Exit;//输送线方向改为Exit
                GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[4];
                Cargo.GetComponent<Varibles.OperatingState>().state = Varibles.CargoState.WaitOut;

                GameObject Item = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/Item"));
                Item.name = Cargo.name;
                Item.transform.Find("Name").GetComponent<Text>().text = Item.name;
                Item.transform.Find("State").GetComponent<Text>().text = "货物状态：" + "等待出库";
                Item.transform.parent = GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform;
                Varibles.GlobalVariable.ConveyorDirections[HighBayNum] = Varibles.Direction.Exit;
                Debug.Log("该货物即将出库！");
            }
            else if (state == Varibles.StorageBinState.Stay2Exit)
            {
                Debug.Log("该货物已经准备出库！");
            }
            else if (state == Varibles.StorageBinState.OutStore)
            {
                Debug.Log("该货物正在出库！");
            }
            else if (state == Varibles.StorageBinState.NotStored)
            {
                Debug.Log("该货物已经出库！");
            }
        }
    }
}

