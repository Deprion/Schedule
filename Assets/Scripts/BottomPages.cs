using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPages : MonoBehaviour
{
    [SerializeField] private GameObject sheetPrefab;
    [SerializeField] private Transform parent;

    private Image prevImg;

    [SerializeField] private Material material;

    public void CreateSheet(string name, int index, bool activeBtn)
    { 
        var obj = Instantiate(sheetPrefab, parent, false);
        obj.GetComponentInChildren<TMP_Text>().text = name;

        obj.GetComponent<Button>().onClick.AddListener(() =>
        ChangeActiveBtn(obj.GetComponent<Image>(), index));

        if (activeBtn) ChangeActiveBtn(obj.GetComponent<Image>(), index);
    }

    private void ChangeActiveBtn(Image img, int index)
    {
        if (prevImg != null) prevImg.material = null;

        img.material = material;
        prevImg = img;

        GameManager.inst.LoadSheet(index);
    }
}
