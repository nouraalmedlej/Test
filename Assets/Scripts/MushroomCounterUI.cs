using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MushroomCounterUI : MonoBehaviour
{
    [SerializeField] TMP_Text tmpText;     // assign if using TextMeshPro
    [SerializeField] Text legacyText;      // assign if using Legacy Text
    [SerializeField] Slider progressSlider;

    int lastShownTotal = -1;

    void Awake()
    {
        if (!tmpText) tmpText = GetComponentInChildren<TMP_Text>(true);
        if (!legacyText) legacyText = GetComponentInChildren<Text>(true);
        if (!progressSlider) progressSlider = GetComponentInChildren<Slider>(true);
    }

    void Start()
    {
        TrySubscribe();
        ForceRefresh();
    }

    void OnEnable()
    {
        TrySubscribe();
        ForceRefresh();
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnMushroomsChanged -= HandleChange;
    }

    void Update()
    {
        // Fallback polling in case event didn't fire/bind
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.totalMushrooms != lastShownTotal)
        {
            int total = GameManager.Instance.totalMushrooms;
            int per = Mathf.Max(1, GameManager.Instance.mushroomsPerAnimal);
            float p = (total % per) / (float)per;
            HandleChange(total, p);
        }
    }

    void TrySubscribe()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnMushroomsChanged += HandleChange;
        else
            Debug.LogWarning("[UI] GameManager.Instance is null (subscribe)");
    }

    void ForceRefresh()
    {
        if (GameManager.Instance == null) return;
        int total = GameManager.Instance.totalMushrooms;
        int per = Mathf.Max(1, GameManager.Instance.mushroomsPerAnimal);
        float p = (total % per) / (float)per;
        HandleChange(total, p);
    }

    void HandleChange(int total, float progress01)
    {
        lastShownTotal = total;
        if (tmpText) tmpText.text = $"Mushrooms: {total}";
        if (legacyText) legacyText.text = $"Mushrooms: {total}";
        if (progressSlider) progressSlider.value = progress01;
        // Debug to confirm UI is updating:
        Debug.Log($"[UI] total={total}, p={progress01:0.00}");
    }
}




