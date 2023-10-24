using Game.Scripts.GameplayLogic.ResourceManagement;

namespace Game.Scripts.Common.Interfaces
{
    public interface IResourceProvider : IEntity
    {
        public Resource GetResource();
    }
}