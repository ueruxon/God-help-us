using Game.Scripts.Common.Interfaces;
using Game.Scripts.GameplayLogic.ResourceManagement;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public interface IResourceRequester : IEntity
    {
        public void Delivery(Resource resource);
    }
}