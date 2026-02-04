using UnityEngine;

public class LevelListUI : MonoBehaviour
{
    public LevelItemUI itemPrefab;
    public Transform contentParent;

    private LevelItemUI selectedItem;

    public int SelectedLevelIndex { get; private set; } = -1;

    void Start()
    {
        CreateItem("Level 1", 1);
        CreateItem("Level 2", 2);
        CreateItem("Level 3", 3);
    }

    void CreateItem(string name, int index)
    {
        int highScore = HighScoreManager.GetHighScore(index);

        var item = Instantiate(itemPrefab, contentParent);
        item.Setup(name, highScore, index, this);
    }


    public void SelectLevel(LevelItemUI item)
    {
        if (selectedItem != null)
            selectedItem.SetSelected(false);

        selectedItem = item;
        selectedItem.SetSelected(true);
        SelectedLevelIndex = item.levelIndex;
    }

    public bool HasSelection()
    {
        return SelectedLevelIndex > 0;
    }
    public int GetSelectedLevelIndex()
    {
        return selectedItem != null ? selectedItem.levelIndex : -1;
    }

}
