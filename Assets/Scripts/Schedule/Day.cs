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
        Instantiate(lessonPrefab, transform, false).
            GetComponent<Lesson>().Init(time, name);
    }
}
