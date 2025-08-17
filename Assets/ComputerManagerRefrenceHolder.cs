using UnityEngine;

public class ComputerManagerRefrenceHolder : MonoBehaviour
{

    public computer computerScript; 
    public GameObject[] computerWindows; 

    private void Start()
    {
        if (ComputerManager.Current != null)
        {
            ComputerManager.Current.UpdateReferences(computerScript, computerWindows);
        }

    }
}