using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform objetivo;

    


    // Update is called once per frame
    void Update()
    {
        transform.position = objetivo.position;
    }
}
