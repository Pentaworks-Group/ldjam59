using System.Linq;

using Assets.Scripts.Core.Persistence;

namespace Assets.Scripts.Prefabs.Menu.SavedGames
{
    public class LevelListContainerBehaviour : GameFrame.Core.UI.List.ListContainerBehaviour<SavedGamePreview>
    {
        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            var savedGames = Base.Core.Game.GetSavedGamePreviews();

            if (savedGames.Count < this.NumDisplayedSlots)
            {
                var keyNumber = 0;

                while (savedGames.Count < this.NumDisplayedSlots)
                {
                    var key = $"SaveGame-{keyNumber}";

                    if (!savedGames.ContainsKey(key))
                    {
                        savedGames[key] = new SavedGamePreview()
                        {
                            Key = key,
                            IsEmpty = true,
                        };
                    }

                    keyNumber++;
                }
            }

            SetContentList(savedGames.Values.OrderByDescending(s => s.Key).ToList());
        }
    }
}
