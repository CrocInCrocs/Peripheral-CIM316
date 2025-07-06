using Unity.Mathematics;
using UnityEngine;

public class PopUpCameraFollow : MonoBehaviour
{
    public GameObject _object;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Vector3 direction = transform.position - _object.transform.position;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
       
    }
}
