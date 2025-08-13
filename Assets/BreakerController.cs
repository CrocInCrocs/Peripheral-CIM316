using UnityEngine;

public class BreakerController : MonoBehaviour
{
    public GameObject[] circuits;
    public void TurnOffBreaker()
    {
        foreach (var circuit in circuits)
        {
            circuit.SetActive(false);
        }
    }
    public void TurnOnBreaker()
    {
        foreach (var circuit in circuits)
        {
            circuit.SetActive(true);
        }
    }
}
