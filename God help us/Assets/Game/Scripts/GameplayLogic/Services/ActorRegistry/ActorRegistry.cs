using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Actors;

namespace Game.Scripts.GameplayLogic.Services.ActorRegistry
{
    public class ActorRegistry : IActorRegistry
    {
        private readonly List<Actor> _allActors;

        public ActorRegistry()
        {
            _allActors = new List<Actor>();
        }

        public void Register(Actor actor)
        {
            if (_allActors.Contains(actor) == false) 
                _allActors.Add(actor);
        }

        public void Unregister(Actor actor)
        {
            
        }

        public Actor GetActor() => 
            null;

        public List<Actor> GetAllActors()
        {
            return _allActors;
        }
    }

    public interface IActorRegistry
    {
        public void Register(Actor actor);
        public void Unregister(Actor actor);

        public Actor GetActor();
        
        // все доступные актеры?
        public List<Actor> GetAllActors();
    }
}