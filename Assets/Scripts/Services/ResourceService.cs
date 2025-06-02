using ChestSystem.Resource;

namespace ChestSystem.Service
{
    public class ResourceService
    {
        private ResourceController resourceController;

        public ResourceService(ResourceView resourceView)
        {
            resourceController = new ResourceController(resourceView);
        }

        public ResourceController GetResourceController()
        {
            return resourceController;
        }
    }
}