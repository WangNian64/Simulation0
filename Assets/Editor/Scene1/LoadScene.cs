using UnityEditor;
using UnityEngine;

public class LoadScene : EditorWindow
{
    private static LoadScene window;
    string RootName = "Warehouse_Final_Material";
    string Name = "WarehouseScene";

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
        RootName = EditorGUILayout.TextField("资源文件夹名称：", RootName);
        Name = EditorGUILayout.TextField("模型名称：", Name);
        
        if (GUILayout.Button("载入", GUILayout.Height(20)))
        {
            string path = GlobalVariable.RootName+"/Simulation/";
            LoadOn(path, Name);

        }
        
    }

    public void LoadOn(string path,string Name) {
        string path0 = path + Name;
        GameObject obj = (GameObject)Resources.Load(path0);//载入模型
        GameObject OBJ = Instantiate(obj);
        OBJ.name = obj.name;
        string path1 = path + "MainInterface";
        GameObject MainInterface = Instantiate((GameObject)Resources.Load(path1));
        MainInterface.name = "MainInterface";
        string path2 = path + "ProcessInterface";
        GameObject ProcessInterface = Instantiate((GameObject)Resources.Load(path2));
        ProcessInterface.name = "ProcessInterface";
        string path3 = path + "StorageStateInterface";
        GameObject StorageStateInterface = Instantiate((GameObject)Resources.Load(path3));
        StorageStateInterface.name = "StorageStateInterface";
        OBJ.AddComponent<Main1>();
        
    }

}
