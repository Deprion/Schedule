using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private Transform star;

    private float rotSpeed = 150;

    private void Update()
    {
        star.Rotate(Vector3.back, Time.deltaTime * rotSpeed);
    }
}
