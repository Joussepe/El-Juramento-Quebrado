using UnityEngine;

public class CameraScript :MonoBehaviour
{
    public GameObject Caballero;

    void Update()
    {
        Vector3 position = transform.position;
        position.x = Caballero.transform.position.x;
        transform.position = position;
    }
}
