using ChestSystem.Chest;
using ChestSystem.Main;
using ChestSystem.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    private ChestController chestController;
    [SerializeField] private Image chestImage;
    private Button chestButton;
    [SerializeField] private TextMeshProUGUI chestStateText;
    [SerializeField] private TextMeshProUGUI chestUnlockingTimerText;

    private void Start()
    {
        chestButton = GetComponent<Button>();
        chestButton.onClick.AddListener(ShowUnlockPanel);
    }

    private void Update()
    {
        if (chestController != null)
        {
            if (chestController.CurrentChestState() is UnlockingState)
                chestController.UpdateState();
        }
    }

    public void SetController(ChestController chestController, ChestType chestType)
    {
        this.chestController = chestController;
        SetChestImage(chestType);
    }
    public void SetChestImage(Sprite sprite)
    {
        if (sprite != null)
        {
            chestImage.sprite = sprite;
            chestImage.transform.localScale = new Vector2(1f, 1f);
        }
        else
        {
            Debug.LogWarning("Sprite is null!");
        }
    }

    public void SetChestImage(ChestType chestType)
    {
        Sprite sprite = chestController.GetChestImage(chestType);
        SetChestImage(sprite);
    }


    public void SetChestTimerText(float timeInSeconds)
    {
        if (!chestUnlockingTimerText.gameObject.activeInHierarchy)
        {
            chestUnlockingTimerText.gameObject.SetActive(true);
        }

        chestUnlockingTimerText.text = chestController.TimeFormat(timeInSeconds);
    }

    private void ShowUnlockPanel()
    {
        chestController.EnableUnlockSelection();
    }

    public void SetChestStateText(string state) => chestStateText.text = state;

    public void DisableTimerText() => chestUnlockingTimerText.gameObject.SetActive(false);
}
