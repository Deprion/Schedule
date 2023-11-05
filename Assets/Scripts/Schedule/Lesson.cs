using System.Collections;
using TMPro;
using UnityEngine;

public class Lesson : MonoBehaviour
{
    [SerializeField] private TMP_Text timeTxt, nameTxt;

    private static WaitForEndOfFrame waitFor = new WaitForEndOfFrame();

    public void Init(string time, string name)
    { 
        timeTxt.text = time;
        nameTxt.text = name;

        StartCoroutine(awaiter());
    }

    private IEnumerator awaiter()
    { 
        yield return waitFor;

        float height = nameTxt.preferredHeight + 30;

        if (string.IsNullOrEmpty(nameTxt.text)) height = 150;

        var rect = GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.rect.height, height);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
