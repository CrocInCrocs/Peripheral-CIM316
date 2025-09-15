using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpCameraFollow : MonoBehaviour
{
    public FPController playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        playerCamera = PeripheralGameManager.Current.returnFPController();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (playerCamera == null)
        {
            playerCamera = PeripheralGameManager.Current.returnFPController();
        }
        Vector3 direction = transform.position - playerCamera.transform.position;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
       
    }
}
