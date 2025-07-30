using UnityEngine;

public class WinController : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            UIController.Instance.ShowWinPanel();
        }
    }
}
