using TMPro;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private GameObject lessonPrefab;
    [SerializeField] private TMP_Text label;

    public void Label(string name)
    { 
        label.text = name;
    }

    public void AddLesson(string time, string name)
    {
        var obj = Instantiate(lessonPrefab, transform, false);

        obj.GetComponent<Lesson>().Init(time, name);

        obj.transform.SetSiblingIndex(1);
    }
}
