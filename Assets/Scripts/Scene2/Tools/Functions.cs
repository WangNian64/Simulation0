using UnityEngine;
using UnityEngine.UI;
using System;
namespace BlackBox.WareHouse.Tools
{
    public class Functions : MonoBehaviour
    { 
        public static void ChangeColor(string CargoName, Varibles.StorageBinState state)
        {
            Varibles.CargoMessage CM = GameObject.Find(CargoName).GetComponent<ShowData.ShowCargoInfo>().Cargomessage;
            int HighBayNum = CM.PositionInfo.HighBayNum; int FloorNum = CM.PositionInfo.FloorNum;
            int ColumnNum = CM.PositionInfo.ColumnNum; Varibles.Place PlaceNum = CM.PositionInfo.place;

            string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum.ToString();
            BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum.ToString();
            BinName = BinName + "/" + PlaceNum.ToString() + "Panel/Bin_" + CargoName;
            switch (PlaceNum)
            {
                case Varibles.Place.A:
                    Varibles.GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 0] = state;
                    break;
                case Varibles.Place.B:
                    Varibles.GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 1] = state;
                    break;
            }
            switch (state)
            {
                case Varibles.StorageBinState.NotStored:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[0];
                    break;
                case Varibles.StorageBinState.Reserved:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[1];
                    break;
                case Varibles.StorageBinState.InStore:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[2];
                    break;
                case Varibles.StorageBinState.Stored:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[3];
                    break;
                case Varibles.StorageBinState.Stay2Exit:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[4];
                    break;
                case Varibles.StorageBinState.OutStore:
                    GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[5];
                    break;
            }
        }

        //接口
        public static void Connector(GameObject Cargo)
        {
            string Name = Cargo.name.Remove(0, 4);
            int i1 = Name.IndexOf("_");
            string HighBayNum = Name.Substring(0, i1); Name = Name.Remove(0, i1 + 1);
            int i2 = Name.IndexOf("_");
            string FloorNum = Name.Substring(0, i2); Name = Name.Remove(0, i2 + 1);
            int i3 = Name.IndexOf("_");
            string ColumnNum = Name.Substring(0, i3);
            string PlaceNum = Name.Remove(0, i3 + 1);
            string CargoName = "Cargo_" + HighBayNum + "_" + FloorNum + "_" + ColumnNum + "_" + PlaceNum;
            int HighBayNum2 = int.Parse(HighBayNum);
            int FloorNum2 = int.Parse(FloorNum);
            int ColumnNum2 = int.Parse(ColumnNum);
            Varibles.Place place1 = Varibles.Place.A;
            Cargo.transform.parent = Varibles.GlobalVariable.WareHouse.transform;
            Cargo.name = CargoName;

            Varibles.CargoMessage CI = new Varibles.CargoMessage();
            CI.Name = Cargo.name;
            CI.Size = Varibles.GlobalVariable.KPD.CargoSize;
            CI.Number1 = "123456789";
            CI.Num = 5;
            CI.InputTime = DateTime.Now.ToLocalTime().ToString();
            CI.Description = "This is a Cargo!";
            CI.PositionInfo.HighBayNum = HighBayNum2;
            CI.PositionInfo.ColumnNum = ColumnNum2;
            CI.PositionInfo.FloorNum = FloorNum2;
            switch (PlaceNum)
            {
                case "A":
                    Varibles.GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 0] = Varibles.StorageBinState.Reserved;
                    place1 = Varibles.Place.A;
                    break;
                case "B":
                    Varibles.GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 1] = Varibles.StorageBinState.Reserved;
                    place1 = Varibles.Place.B;
                    break;
            }
            CI.PositionInfo.place = place1;
            Cargo.AddComponent<ShowData.ShowCargoInfo>().Cargomessage = CI;
            Cargo.AddComponent<Varibles.OperatingState>().state = Varibles.CargoState.WaitIn;


            Varibles.GlobalVariable.EnterCargosNameList.Add(CargoName);
            Varibles.GlobalVariable.TempQueue.Enqueue(Cargo);

            string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum;
            BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum;
            BinName = BinName + "/" + PlaceNum + "Panel/Bin_" + CargoName;
            GameObject.Find(BinName).GetComponent<Image>().color = Varibles.GlobalVariable.BinColor[1];

            GameObject Item = Instantiate((GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/Item"));
            Item.name = Cargo.name;
            Item.transform.Find("Name").GetComponent<Text>().text = Item.name;
            Item.transform.Find("State").GetComponent<Text>().text = "货物状态：" + "等待入库";
            Item.transform.parent = GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform;
        }
    }
}
