using System;
using System.Collections.Generic;
using Game.Scripts.GameplayLogic.AI.Reporting;
using Game.Scripts.GameplayLogic.Registers;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Scripts.UI.DebugWindow
{
    public class AIDebugWindow : MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private ActionDebugItem _itemPrefab;
        [SerializeField] private TMP_Text _actorName;
        [SerializeField] private ActionDebugItem _activeActionItem;

        private AIReporter _aiReporter;
        private ActorRegistry _actorRegistry;

        private List<ActionDebugItem> _debugItems;

        [Inject]
        public void Construct(AIReporter reporter, ActorRegistry actorRegistry)
        {
            _aiReporter = reporter;
            _actorRegistry = actorRegistry;
            _debugItems = new List<ActionDebugItem>();
        }

        public void Init()
        {
            _aiReporter.DecisionDetailsReported += OnDecisionDetailsProduced;

            CreateHistoryItems();
        }

        private void CreateHistoryItems()
        {
            _activeActionItem.gameObject.SetActive(false);
            
            for (int i = 0; i < 5; i++)
            {
                ActionDebugItem item = Instantiate(_itemPrefab, _contentRoot);
                item.gameObject.SetActive(false);
                _debugItems.Add(item);
            }
        }

        private void OnDecisionDetailsProduced()
        {
            List<ReportDetails> detailsList = _aiReporter
                .GetReportDetailsForActor(_actorRegistry.GetAllActorIds()[0]);

            int index = 0;
            for (int i = detailsList.Count - 1; i >= 0; i--)
            {
                ReportDetails details = detailsList[i];
                _actorName.SetText(details.ActorName);
                
                if (i == detailsList.Count - 1)
                {
                    _activeActionItem.ActionName.SetText(details.ActionName);
                    _activeActionItem.Score.SetText(details.Score);
                    _activeActionItem.gameObject.SetActive(true);

                    continue;
                }
                
                ActionDebugItem item = _debugItems[index];
                item.ActionName.SetText(details.ActionName);
                item.Score.SetText(details.Score);
                item.gameObject.SetActive(true);

                index++;
            }
        }

        private void OnDestroy() => 
            _aiReporter.DecisionDetailsReported -= OnDecisionDetailsProduced;
    }
}