using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;
    private List<ItemDetails> itemList = new List<ItemDetails>();

    /// <summary>
    /// 左侧列表模版样式
    /// </summary>
    private VisualTreeAsset itemRowTemplate;

    /// <summary>
    /// 左侧列表
    /// </summary>
    private ListView itemListView;

    [MenuItem("WRTools/UI-Toolkit/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Settings.ItemEditorPath);
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // 通过绝对路径拿到模版数据
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Settings.ItemRowTemplatePath);

        // 获得左侧的listview容器
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        // 创建面板时加载数据
        LoadDataBase();

        // 生成ListView
        GenerateListView();
    }


    /// <summary>
    /// 拿到数据
    /// </summary>
    private void LoadDataBase()
    {
        var dataArr = AssetDatabase.FindAssets("ItemDataList_SO");
        if (dataArr.Length > 1)
        {
            // 通过Guid找到文件对应路径
            var path = AssetDatabase.GUIDToAssetPath(dataArr[0]);
            // 通过地址拿到文件
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;
        }

        itemList = dataBase.itemDetailsList;

        // 重要步骤！！！如果不标记无法保存数据
        EditorUtility.SetDirty(dataBase);

        // foreach (var i in itemList)
        //     Debug.Log(string.Format("{0}-{1}", i.itemID, i.name));
    }

    /// <summary>
    /// 生成面板左侧列表
    /// </summary>
    private void GenerateListView()
    {
        // copy一份模版文件给makeItem
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            // 保证i必须小于数据列表的个数
            if (i < itemList.Count)
            {
                // if (itemList[i].itemIcon != null)
                if (!ReferenceEquals(itemList[i].itemIcon, null))
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i] == null ? "No Item" : itemList[i].itemName;
            }
        };

        //根据需要高度调整数值
        itemListView.fixedItemHeight = 50;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
    }
}