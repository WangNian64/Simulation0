using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour {

    public Place place1;
    // Use this for initialization
    public void Click()
    {
        string HighBayNum = GameObject.Find("HighBayNum").transform.Find("InputField").GetComponent<InputField>().text;
        string FloorNum = GameObject.Find("FloorNum").transform.Find("InputField").GetComponent<InputField>().text;
        string ColumnNum = GameObject.Find("ColumnNum").transform.Find("InputField").GetComponent<InputField>().text;
        string PlaceNum = GameObject.Find("PlaceNum").transform.Find("InputField").GetComponent<InputField>().text;
        string CargoNum = GameObject.Find("CargoNum").transform.Find("InputField").GetComponent<InputField>().text;
        string EnterTime = GameObject.Find("EnterTime").transform.Find("InputField").GetComponent<InputField>().text;
        string CargoDescription = GameObject.Find("CargoDescription").transform.Find("InputField").GetComponent<InputField>().text;
        bool condition = (HighBayNum != null) && (FloorNum != null) && (ColumnNum != null) && (PlaceNum != null) && (CargoNum != null) && (EnterTime != null) && (CargoDescription != null);
        if (condition)
        {
            int HighBayNum2 = int.Parse(HighBayNum);
            int FloorNum2 = int.Parse(FloorNum);
            int ColumnNum2 = int.Parse(ColumnNum);
            //Place place1;
            switch (PlaceNum)
            {
                case "A":
                    place1 = Place.A;
                    break;
                case "B":
                    place1 = Place.B;
                    break;
                default:
                    GameObject.Find("Notice").GetComponent<Text>().text = "请按要求输入A或B!";
                    break;
            }
            int Num1 = GlobalVariable.KPD.HighBaysNum;
            int Num2 = GlobalVariable.KPD.StorePositions.StoreFloorPositions.Length;
            int Num3 = GlobalVariable.KPD.StorePositions.StoreColumnPositions.Length;
            bool condition1 = (HighBayNum2 > 0 && HighBayNum2 <= Num1) && (FloorNum2 > 0 && FloorNum2 <= Num2) && (ColumnNum2 > 0 && ColumnNum2 < Num3);
            string Sy = "_";
            string CargoName = "Cargo" + Sy + HighBayNum + Sy + FloorNum + Sy + ColumnNum + Sy + PlaceNum;
            if (condition1)
            {
                if (!GlobalVariable.EnterCargosNameList.Contains(CargoName))
                {
                    GameObject Cargo = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/Cargo"));
                    //Cargo.name = CargoName;
                    //CargoMessage CI = new CargoMessage();
                    //CI.Name = Cargo.name;
                    //CI.Size = GlobalVariable.KPD.CargoSize;
                    //CI.Number1 = CargoNum;
                    //CI.Num = 5;
                    //CI.InputTime = EnterTime;
                    //CI.Description = CargoDescription;
                    //CI.PositionInfo.HighBayNum = HighBayNum2;
                    //CI.PositionInfo.ColumnNum = ColumnNum2;
                    //CI.PositionInfo.FloorNum = FloorNum2;
                    //CI.PositionInfo.place = place1;
                    //Cargo.AddComponent<ShowCargoInfo>().Cargomessage = CI;
                    //Cargo.AddComponent<OperatingState>().state = CargoState.WaitIn;
                    Cargo.transform.parent = GlobalVariable.WareHouse.transform;
                    Cargo.transform.localPosition = GlobalVariable.KPD.EnterPosition;
                    //GameObject.Find("Notice").GetComponent<Text>().text = "创建成功！";
                    //GlobalVariable.EnterCargosList.Add(Cargo);
                    //GlobalVariable.EnterCargosNameList.Add(CargoName);
                    //GlobalVariable.TempQueue.Enqueue(Cargo);
                    //GlobalVariable.CargoState state = GlobalVariable.CargoState.Wait;
                    //GlobalVariable.CargoStateList.Add(GlobalVariable.CargoState.Wait);
                    //string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum2.ToString();
                    //BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum2.ToString();
                    //BinName = BinName + "/" + PlaceNum + "Panel/Bin_" + CargoName;
                    //GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[1];
                    //switch (PlaceNum)
                    //{
                    //    case "A":
                    //        GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 0] = StorageBinState.Reserved;
                    //        break;
                    //    case "B":
                    //        GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 1] = StorageBinState.Reserved;
                    //        break;
                    //}

                    //GameObject Item = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/Item"));
                    //Item.name = Cargo.name;
                    //Item.transform.Find("Name").GetComponent<Text>().text = Item.name;
                    //Item.transform.Find("State").GetComponent<Text>().text = "货物状态：" + "等待入库";
                    //Item.transform.parent = GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform;
                    Cargo.name = "Good" + HighBayNum + "_" + FloorNum + "_" + ColumnNum + "_" + PlaceNum;
                    Functions.Connector(Cargo);
                }
                else if(GlobalVariable.EnterCargosNameList.Contains(CargoName))
                {
                    GameObject.Find("Notice").GetComponent<Text>().text = "该货物已存在！";
                }
            }
            
        }
    }
}
