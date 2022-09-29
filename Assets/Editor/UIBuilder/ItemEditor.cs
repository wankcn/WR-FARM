using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

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

    /// <summary>
    /// 拿到右侧的滑动列表ScrollView
    /// </summary>
    private ScrollView itemDetailsSection;

    /// <summary>
    /// 当前选中的物品
    /// </summary>
    private ItemDetails activeItem;

    /// <summary>
    /// 默认预览图片
    /// </summary>
    private Sprite defaultIcon;

    /// <summary>
    /// 修改显示图片 style-background中的Img
    /// </summary>
    private VisualElement iconPreview;


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

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Settings.ItemEditorPath);
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // 通过绝对路径拿到模版数据
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Settings.ItemRowTemplatePath);
        // 获取默认Icon图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>(Settings.DefaultIconInItemEditor);

        // 获得左侧的listview容器
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        // 获得右侧的ScrollView容器
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        // 找到面板中设置的icon
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

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

        // 根据需要高度调整数值
        itemListView.fixedItemHeight = 50;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        // 拿到当前物品数据
        itemListView.onSelectionChange += OnListSelectionChange;

        // 右侧信息面板不可见
        itemDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails) selectedItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true; // 选择物品之后显示信息
    }

    private void GetItemDetails()
    {
        // 标记为dirty，方便保存数据更改数据和撤销数据
        itemDetailsSection.MarkDirtyRepaint();

        // ID
        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(e =>
        {
            activeItem.itemID = e.newValue;
        });

        // 名字
        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(e =>
        {
            activeItem.itemName = e.newValue;
            // 修改名字后重新构建一次
            itemListView.Rebuild();
        });

        // 设置显示图片 VisualElement中类型为texture
        iconPreview.style.backgroundImage =
            activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(e =>
        {
            Sprite newIcon = e.newValue as Sprite;
            activeItem.itemIcon = newIcon;

            // 修改预览的图片 没有图片时显示默认图片
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });
    }

    private void GetItemDetailss()
    {
        // iconPreview.style.backgroundImage =
        //     activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        // itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        // itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        // {
        //     Sprite newIcon = evt.newValue as Sprite;
        //     activeItem.itemIcon = newIcon;
        //
        //     iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
        //     itemListView.Rebuild();
        // });

        //其他所有变量的绑定
        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldSprite = (Sprite) evt.newValue;
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType) evt.newValue;
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedup").value = activeItem.canPickedup;
        itemDetailsSection.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}