using ChestSystem.Chest;
using UnityEngine;

public class ChestView : MonoBehaviour
{
    private ChestController chestController;

    public void SetController(ChestController chestController)
    {
        this.chestController = chestController;
    }
}
