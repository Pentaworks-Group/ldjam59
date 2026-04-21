using System.Collections.Generic;

namespace Assets.Scripts.Scenes.Game.Level
{
    public class LevelListContainerBehaviour : GameFrame.Core.UI.List.ListContainerBehaviour<LevelPreview>
    {
        public override void CustomStart()
        {
            UpdateList();
        }
        
        protected override void CustomOnEnable()
        {
            base.CustomOnEnable();

            UpdateList();
        }

        public void UpdateList()
        {
            var levelScores = Base.Core.Game.State.LevelScores;
            List<LevelPreview> previews = new List<LevelPreview>();

            var levels = Base.Core.Game.State.Mode.Levels;
            for (var index = 0; index < levels.Count; index++)
            {
                var levelDefinition = levels[index];
                var score = levelScores.GetValueOrDefault(levelDefinition.Reference, new Core.Models.LevelScore());
                var preview = new LevelPreview()
                {
                    Score = score,
                    Reference = levelDefinition.Reference,
                    Name = levelDefinition.Name,
                    Description = levelDefinition.Description,
                    Index = index,
                };
                previews.Add(preview);
            }
            previews.Reverse();
            SetContentList(previews);
        }


    }
}
