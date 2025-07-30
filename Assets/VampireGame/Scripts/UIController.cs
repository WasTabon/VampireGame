using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum UIPanelType
{
    NoKeyPanel,
    GetKeyButton,
    UseKeyButton
}

[Serializable]
public class UIPanelEntry
{
    public UIPanelType panelType;
    public RectTransform panelTransform;
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI _keysText;
    
    [SerializeField] private List<UIPanelEntry> _uiPanels;
    [SerializeField] private float _animationDuration = 0.5f;

    private Dictionary<UIPanelType, UIPanelState> _panelStates = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializePanels();
    }

    private void Update()
    {
        _keysText.text = PlayerController.Instance.keys.ToString();
    }

    private void InitializePanels()
    {
        foreach (var panelEntry in _uiPanels)
        {
            if (panelEntry.panelTransform == null) continue;

            var startPos = panelEntry.panelTransform.anchoredPosition;
            var hiddenPos = startPos + new Vector2(0, -Screen.height);

            panelEntry.panelTransform.anchoredPosition = hiddenPos;

            _panelStates[panelEntry.panelType] = new UIPanelState
            {
                Transform = panelEntry.panelTransform,
                VisiblePosition = startPos,
                HiddenPosition = hiddenPos
            };
        }
    }

    public void ShowPanel(UIPanelType panelType)
    {
        if (!_panelStates.TryGetValue(panelType, out var panel)) return;

        panel.Transform.DOAnchorPos(panel.VisiblePosition, _animationDuration).SetEase(Ease.OutCubic);
    }

    public void HidePanel(UIPanelType panelType)
    {
        if (!_panelStates.TryGetValue(panelType, out var panel)) return;

        panel.Transform.DOAnchorPos(panel.HiddenPosition, _animationDuration).SetEase(Ease.InCubic);
    }

    private class UIPanelState
    {
        public RectTransform Transform;
        public Vector2 VisiblePosition;
        public Vector2 HiddenPosition;
    }
}
