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
        }

        public void SetTotalGemsAndCoinsCount(int coins, int gems)
        {
            int totalCoins = coins + GetCoinsCount();
            int totalGems = gems + GetGemsCount();

            SetCoinsCount(totalCoins);
            SetGemsCount(totalGems);
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