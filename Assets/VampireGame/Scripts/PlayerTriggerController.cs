using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Door"))
        {
            if (coll.gameObject.TryGetComponent(out Door door))
                if (door.isOpened == false)
                {
                    PlayerController.Instance.door = coll.gameObject;
                    if (PlayerController.Instance.keys > 0)
                    {
                        UIController.Instance.ShowPanel(UIPanelType.UseKeyButton);
                    }
                    else
                    {
                        UIController.Instance.ShowPanel(UIPanelType.NoKeyPanel);
                    }
                }
        }
        else if (coll.gameObject.CompareTag("Chest"))
        {
            PlayerController.Instance.chest = coll.gameObject;
            UIController.Instance.ShowPanel(UIPanelType.GetKeyButton);
        }
    }
    
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("Door"))
        {
            if (coll.gameObject.TryGetComponent(out Door door))
                if (door.isOpened == false)
                {
                    PlayerController.Instance.door = null;
                    if (PlayerController.Instance.keys > 0)
                    {
                        UIController.Instance.HidePanel(UIPanelType.UseKeyButton);
                    }
                    else
                    {
                        UIController.Instance.HidePanel(UIPanelType.NoKeyPanel);
                    }
                }
        }
        else if (coll.gameObject.CompareTag("Chest"))
        {
            PlayerController.Instance.chest = null;
            UIController.Instance.HidePanel(UIPanelType.GetKeyButton);
        }
    }
}
