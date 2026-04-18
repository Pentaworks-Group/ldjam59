

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core
{
    public class LevelConverter
    {

        public Level Convert(LevelDefinition levelDefinition)
        {

            var convertedLevel = new Level()
            {
                Name = levelDefinition.Name,
                Reference = levelDefinition.Reference,
                Description = levelDefinition.Description,
                Seed = levelDefinition.Seed,
            };
            return convertedLevel;
        }
    }
}
