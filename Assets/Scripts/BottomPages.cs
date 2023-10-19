using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPages : MonoBehaviour
{
    [SerializeField] private GameObject sheetPrefab;
    [SerializeField] private Transform parent;

    public void CreateSheet(string name, int index)
    { 
        var obj = Instantiate(sheetPrefab, parent, false);
        obj.GetComponentInChildren<TMP_Text>().text = name;

        obj.GetComponent<Button>().onClick.AddListener(() => GameManager.inst.LoadSheet(index));
    }
}
