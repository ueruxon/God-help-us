using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Actors;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Services
{
    public class ActorRegistry
    {
        private readonly List<Actor> _allActors;
        private readonly Dictionary<string, Actor> _allActorById;

        public ActorRegistry()
        {
            _allActors = new List<Actor>();
            _allActorById = new Dictionary<string, Actor>();
        }

        public void Register(string actorId, Actor actor)
        {
            if (_allActors.Contains(actor) == false) 
                _allActors.Add(actor);

            _allActorById[actorId] = actor;
        }

        public void Unregister(string actorId, Actor actor)
        {
            if (_allActorById.ContainsKey(actorId))
                _allActorById.Remove(actorId);
        }

        public Actor GetActor(string actorId)
        {
            if (_allActorById.ContainsKey(actorId) == false)
                Debug.LogWarning($"Actor ID {actorId} is not found");

            return _allActorById[actorId];
        }

        public List<Actor> GetAllActors()
        {
            return _allActors;
        }
    }
}