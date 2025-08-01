using DG.Tweening;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public static MenuUIController Instance;
    
    [SerializeField] private CanvasGroup _menuButtonsPanel;
    [SerializeField] private CanvasGroup _levelsPanel;
    [SerializeField] private CanvasGroup _shopPanel;

    private void Awake()
    {
        Instance = this;
    }

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

    public void CloseMenuButton()
    {
        _menuButtonsPanel.DOFade(0f, 0.5f)
            .OnComplete((() =>
            {
                _menuButtonsPanel.gameObject.SetActive(false);
            }));
    }
    
    public void OpenShopPanel()
    {
        _shopPanel.DOFade(0f, 0f);
        _shopPanel.DOFade(1f, 0.5f)
            .OnStart((() =>
            {
                _shopPanel.gameObject.SetActive(true);
            }));
    }

    public void CloseShopPanel()
    {
        _shopPanel.DOFade(0f, 0.5f)
            .OnStart((() =>
            {
                _shopPanel.gameObject.SetActive(false);
            }));
    }

    public Tween CloseMenuButtonTween()
    {
        Tween tween = _menuButtonsPanel.DOFade(0f, 0.5f)
            .OnComplete(() =>
            {
                _menuButtonsPanel.gameObject.SetActive(false);
            });

        return tween;
    }
    public Tween OpenMenuButtonTween()
    {
        Tween tween = _menuButtonsPanel.DOFade(1f, 0.5f)
            .OnStart(() =>
            {
                _menuButtonsPanel.gameObject.SetActive(true);
            });

        return tween;
    }
    public Tween CloseShopPanelTween()
    {
        Tween tween = _shopPanel.DOFade(0f, 0.5f);
        
        DOVirtual.DelayedCall(0.5f, () =>
        {
            _shopPanel.gameObject.SetActive(false);
        });

        return tween;
    }
}
