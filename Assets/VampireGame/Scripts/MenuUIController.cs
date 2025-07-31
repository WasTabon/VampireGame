using DG.Tweening;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menuButtonsPanel;
    [SerializeField] private CanvasGroup _levelsPanel;

    public void OpenLevelsPanel()
    {
        _menuButtonsPanel.DOFade(0f, 0.5f)
            .OnComplete((() =>
            {
                _menuButtonsPanel.gameObject.SetActive(false);
                _levelsPanel.gameObject.SetActive(true);
                _levelsPanel.DOFade(1f, 0.5f);
            }));
    }

    public void CloseLevelsPanel()
    {
        _levelsPanel.DOFade(0f, 0.5f)
            .OnComplete((() =>
            {
                _levelsPanel.gameObject.SetActive(false);
                _menuButtonsPanel.gameObject.SetActive(true);
                _menuButtonsPanel.DOFade(1f, 0.5f);
            }));
    }
}
