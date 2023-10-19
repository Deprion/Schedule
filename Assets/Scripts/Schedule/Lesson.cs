using TMPro;
using UnityEngine;

public class Lesson : MonoBehaviour
{
    [SerializeField] private TMP_Text timeTxt, nameTxt;

    public void Init(string time, string name)
    { 
        timeTxt.text = time;
        nameTxt.text = name;
    }
}
