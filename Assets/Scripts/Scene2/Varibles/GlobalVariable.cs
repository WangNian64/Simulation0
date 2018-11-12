using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackBox.WareHouse.Varibles
{
    public enum WaitState { None = 0, WaitEnter = 1, WaitExit = 2 }
    public enum State { On = 1, Off = 0 }
    public enum Direction { Enter = 1, Exit = 0 }
    public enum CargoState
    {
        WaitIn = 0, Enter = 1, Stored = 2, WaitOut = 3, Exit = 4
    }
    public enum StorageBinState
    {
        NotStored = 0, Reserved = 1, InStore = 2, Stored = 3, Stay2Exit = 4, OutStore = 5
    }

    public class GlobalVariable
    {
        public static KeyPositionsData KPD;
        public static GameObject WareHouse;//仓库对象
        public static State[] PilersState;//堆垛机状态
                                          //public static List<GameObject> EnterCargosList;//入库货物对象列表
        public static List<string> EnterCargosNameList;//入库货物对象名称列表
                                                       //public static List<GameObject> ExitCargosList;//出库货物对象列表
        public static Queue<GameObject>[] ConveyorQueue;//排队队列
        public static Queue<GameObject>[] EnterQueue;//入库队列
        public static Queue<GameObject>[] ExitQueue;//出库队列
        public static WaitState[] Wait;//到达指示
        public static Queue<GameObject>[] PilerQueue;//堆垛机货物队列
        public static State[,] UnidirectionalConveyorStates;//单向输送机工作状态
        public static State[,,] BidirectionalConveyorStates;//双向输送机工作状态
        public static Direction[] ConveyorDirections;//双向输送机运输方向
        public static State[] LiftTransferStates;//顶升移栽机工作状态
        public static Vector3[,] UnidirectionalPositions;//单向输送线关键点坐标
        public static Vector3[,,] BidirectionalPositions;//双向输送线关键点坐标
        public static Vector3[,] LiftTransferPositions;//堆垛机关键点坐标
        public static Vector3[] PilerBodyPartPositions;//堆垛机bodyPart初始位置
        public static Queue<GameObject> TempQueue;//出入库货物队列
        public static bool FollowState;//相机是否跟随对象
        public static GameObject FollowPlayer;//跟随对象
        public static StorageBinState[,,,] BinState;//所有货位的状态
        public static Color[] BinColor;//显示面板中Bin的颜色
        public static string RootName;// = GameObject.Find("RootChild").transform.parent.name;//资源文件夹名称
    }
}

