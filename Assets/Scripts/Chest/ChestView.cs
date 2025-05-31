using ChestSystem.Chest;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    private ChestController chestController;
    [SerializeField] private Image chestImage;
    [SerializeField] private TextMeshProUGUI chestStateText;
    [SerializeField] private TextMeshProUGUI chestUnlockingTimerText;

    public void SetController(ChestController chestController, ChestType chestType)
    {
        this.chestController = chestController;
        SetChestImage(chestType);
    }

    private void SetChestImage(ChestType chestType)
    {
        Sprite sprite = chestController.GetChestImage(chestType);
        Debug.Log($"Setting chest image: {sprite?.name ?? "NULL"}");

        if (sprite != null)
        {
            chestImage.sprite = sprite;
            chestImage.transform.localScale = new Vector2(1f, 1f);
        }
        else
        {
            Debug.LogWarning("Chest sprite is null!");
        }
    }
}
