namespace ChestSystem.Resource
{
    public class ResourceModel
    {
        private int coinsCount;
        private int gemsCount;

        public ResourceModel()
        {
            gemsCount = 15;
            coinsCount = 0;
        }

        public void SetCoinCount(int coinCount) => this.coinsCount = coinCount;

        public int GetCoinCount() => coinsCount;

        public void SetGemsCount(int gemsCount) => this.gemsCount = gemsCount;

        public int GetGemsCount() => gemsCount;
    }
}