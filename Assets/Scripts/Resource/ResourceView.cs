using TMPro;
using UnityEngine;

namespace ChestSystem.Resource
{
    public class ResourceView : MonoBehaviour
    {
        private ResourceController resourceController;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI gemsText;

        public void SetResourceController(ResourceController resourceController)
        {
            this.resourceController = resourceController;
            DisplayCoins();
            DisplayGems();
        }

        public void DisplayCoins()
        {
            coinsText.text = resourceController.GetCoinsCount().ToString();
        }

        public void DisplayGems()
        {
            gemsText.text = resourceController.GetGemsCount().ToString();
        }
    }
}
