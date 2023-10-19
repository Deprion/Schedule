using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class XLogo : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject yanaLogo;

    [SerializeField] private float speed;
    private float progress = 0;
    private Vector2 defPos = new Vector2(-300, 0);

    private WaitForFixedUpdate waitFor = new WaitForFixedUpdate();

    private bool isShowing = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();

        isShowing = !isShowing;

        StartCoroutine(MoveLogo(isShowing));
    }

    private IEnumerator MoveLogo(bool show)
    {
        progress = 0;

        if (show)
        {
            while (yanaLogo.transform.localPosition.x != 0)
            {
                //logo reveal
                yanaLogo.transform.localPosition = Vector2.Lerp
                    (yanaLogo.transform.localPosition, Vector2.zero, progress);

                // X rotation
                transform.localRotation = Quaternion.Lerp
                    (transform.localRotation, Quaternion.Euler(0, 0, 180), progress);

                progress += Time.deltaTime * speed;

                yield return waitFor;
            }
        }
        else
        {
            while (yanaLogo.transform.localPosition.x != defPos.x)
            {
                // logo hide
                yanaLogo.transform.localPosition = Vector2.Lerp
                    (yanaLogo.transform.localPosition, defPos, progress);

                // X rotation
                transform.localRotation = Quaternion.Lerp
                    (transform.localRotation, Quaternion.identity, progress);

                progress += Time.deltaTime * speed;

                yield return waitFor;
            }
        }
    }
}
