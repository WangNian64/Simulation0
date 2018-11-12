using UnityEditor;
using UnityEngine;
namespace BlackBox.WareHouse.CreateUnit.CreateScene
{
    public class LoadScene : EditorWindow
    {
        private static LoadScene window;
        string ResRootName = "Warehouse_Material";
        string SceneName = "WarehouseScene";

        [MenuItem("CustomObject/LoadScene", priority = 0)]
        static void Warehouse_Tool()
        {
            Rect window_position1 = new Rect(500, 1000, 300, 400);
            //获取当前窗口实例
            window = EditorWindow.GetWindowWithRect<LoadScene>(window_position1);
            //显示窗口
            window.Show();
        }

        private void OnGUI()
        {
            ResRootName = EditorGUILayout.TextField("资源文件夹名称：", ResRootName);
            SceneName = EditorGUILayout.TextField("模型名称：", SceneName);

            if (GUILayout.Button("载入", GUILayout.Height(20)))
            {
                string path = ResRootName + "/Simulation/";
                LoadOn(path, SceneName);
            }

        }

        public void LoadOn(string path, string Name)
        {
            string fullPath = path + Name;
            Debug.Log(fullPath);
            GameObject SceneObj = (GameObject)Resources.Load(fullPath);//载入模型
            GameObject SceneObj1 = Instantiate(SceneObj);
            SceneObj1.name = SceneObj.name;
            string path1 = path + "MainInterface";
            GameObject MainInterface = Instantiate((GameObject)Resources.Load(path1));
            MainInterface.name = "MainInterface";
            string path2 = path + "ProcessInterface";
            GameObject ProcessInterface = Instantiate((GameObject)Resources.Load(path2));
            ProcessInterface.name = "ProcessInterface";
            string path3 = path + "StorageStateInterface";
            GameObject StorageStateInterface = Instantiate((GameObject)Resources.Load(path3));
            StorageStateInterface.name = "StorageStateInterface";
            SceneObj1.AddComponent<ControlUnit.AnimaControl.Main1>();
        }
    }

}
