using ChestSystem.Main;
using UnityEngine;

namespace ChestSystem.Resource
{
    public class ResourceController
    {
        private ResourceView resourceView;
        private ResourceModel resourceModel;

        public ResourceController(ResourceView resourceView)
        {
            this.resourceView = resourceView;
            resourceModel = new ResourceModel();
            resourceView.SetResourceController(this);
            GameService.Instance.eventService.OnChestCollected.AddListener(SetTotalGemsAndCoinsCount);

        }

        ~ResourceController()
        {
            GameService.Instance.eventService.OnChestCollected.RemoveListener(SetTotalGemsAndCoinsCount);
        }

        public void SetTotalGemsAndCoinsCount(int coins, int gems)
        {
            AddCoins(coins);
            AddGems(gems);
        }

        public void AddCoins(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            int newCount = GetCoinsCount() + amount;
            SetCoinsCount(newCount);
        }

        public void DeductCoins(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            int newCount = GetCoinsCount() - amount;
            if (newCount < 0) newCount = 0;
            SetCoinsCount(newCount);
        }

        public void AddGems(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            int newCount = GetGemsCount() + amount;
            SetGemsCount(newCount);
        }

        public void DeductGems(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            int newCount = GetGemsCount() - amount;
            if (newCount < 0) newCount = 0;
            SetGemsCount(newCount);
        }

        public void SetCoinsCount(int count)
        {
            resourceModel.SetCoinCount(count);
            resourceView.DisplayCoins();
        }

        public void SetGemsCount(int count)
        {
            resourceModel.SetGemsCount(count);
            resourceView.DisplayGems();
        }

        public int GetCoinsCount() => resourceModel.GetCoinCount();

        public int GetGemsCount() => resourceModel.GetGemsCount();
    }
}