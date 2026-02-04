using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelItemUI : MonoBehaviour, IPointerClickHandler
{
    public Image bg;
    public TMP_Text levelName;
    public TMP_Text bestScore;

    public int levelIndex;
    private LevelListUI owner;

    public void Setup(string name, int score, int index, LevelListUI list)
    {
        levelName.text = name;
        bestScore.text = "BEST: " + score;

        levelIndex = index;  
        owner = list;

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        Color c = bg.color;
        c.a = selected ? 1f : 0f;
        bg.color = c;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        owner.SelectLevel(this);
    }
}
