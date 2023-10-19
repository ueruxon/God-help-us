using Game.Scripts.Data.ResourcesData;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceManagement
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public string Id => _id;
        private string _id;

        public ResourceType Type => _type;
        private ResourceType _type;
        
        private ResourceConfig _data;

        public void Construct(string id, ResourceConfig data)
        {
            _id = id;
            _data = data;
            _type = _data.Type;
            _meshRenderer.material.color = _data.Color;
        }
    }
}