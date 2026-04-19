using System;
using System.Collections.Generic;
using System.Linq;
using GameFrame.Core;

namespace Assets.Scripts.Constants
{
    public static class Scenes
    {
        public const String MainMenuName = "MainMenu";
        private static Scene mainMenu;
        public static Scene MainMenu
        {
            get
            {
                if (mainMenu == default)
                {
                    mainMenu = new Scene()
                    {
                        Name = MainMenuName,
                        IsStoppingBackgroundMusic = true
                    };
                }

                return mainMenu;
            }
        }

        public const String CreditsName = "Credits";
        private static Scene credits;
        public static Scene Credits
        {
            get
            {
                if (credits == default)
                {
                    credits = new Scene()
                    {
                        Name = CreditsName,
                        IsStoppingBackgroundMusic = true,
                    };
                }

                return credits;
            }
        }

        public const String OptionsName = "Options";
        private static Scene options;
        public static Scene Options
        {
            get
            {
                if (options == default)
                {
                    options = new Scene()
                    {
                        Name = OptionsName,
                        IsStoppingBackgroundMusic = true,
                    };
                }

                return options;
            }
        }

        public const String SavedGamesName = "SavedGames";
        private static Scene savedGames;
        public static Scene SavedGames
        {
            get
            {
                if (savedGames == default)
                {
                    savedGames = new Scene()
                    {
                        Name = SavedGamesName,
                        IsStoppingBackgroundMusic = true,
                    };
                }

                return savedGames;
            }
        }

        public const String GameModeName = "GameMode";
        private static Scene gameMode;
        public static Scene GameMode
        {
            get
            {
                if (gameMode == default)
                {
                    gameMode = new Scene()
                    {
                        Name = GameModeName,
                        IsStoppingBackgroundMusic = true,
                        //AmbienceClips = new List<String>()
                        //{
                        //    "WoodSound"
                        //},
                        BackgroundClips = new List<String>() { }
                    };
                }

                return gameMode;
            }
        }

        public const String GameName = "Game";
        private static Scene game;
        public static Scene Game
        {
            get
            {
                if (game == default)
                {
                    game = new Scene()
                    {
                        Name = GameName,
                        IsStoppingBackgroundMusic = true,
                        //AmbienceClips = new List<String>()
                        //{
                        //    "WoodSound"
                        //},
                        BackgroundClips = new List<String>() { }
                    };
                }

                return game;
            }
        }

        public const String LevelCompletedName = "LevelCompleted";
        private static Scene levelCompleted;
        public static Scene LevelCompleted
        {
            get
            {
                if (levelCompleted == default)
                {
                    levelCompleted = new Scene()
                    {
                        Name = LevelCompletedName,
                        IsStoppingBackgroundMusic = true,
                        //AmbienceClips = new List<String>()
                        //{
                        //    "WoodSound"
                        //},
                        //BackgroundClips = new List<String>()
                        //{
                        //    "Background"
                        //}
                    };
                }

                return levelCompleted;
            }
        }

        public const String GameOverName = "GameOver";
        private static Scene gameOver;
        public static Scene GameOver
        {
            get
            {
                if (gameOver == default)
                {
                    gameOver = new Scene()
                    {
                        Name = GameOverName,
                        IsStoppingBackgroundMusic = true,
                    };
                }

                return gameOver;
            }
        }

        public const String ShootingStarsName = "ShootingStars";
        private static Scene shootingStars;
        public static Scene ShootingStars
        {
            get
            {
                if (shootingStars == default)
                {
                    shootingStars = new Scene()
                    {
                        Name = ShootingStarsName,
                        BackgroundClips = new List<String>()
                        {
                            "ShootingStars"
                        }
                    };
                }

                return shootingStars;
            }
        }

        public const String GameTestName = "GameTest";
        private static Scene gameTest;
        public static Scene GameTest
        {
            get
            {
                if (gameTest == default)
                {
                    gameTest = new Scene()
                    {
                        Name = GameTestName,
                        BackgroundClips = new List<String>()
                        {
                            "ShootingStars"
                        }
                    };
                }

                return shootingStars;
            }
        }

        public const String AudioTestName = "GameTest";
        private static Scene audioTest;
        public static Scene AudioTest
        {
            get
            {
                if (audioTest == default)
                {
                    audioTest = new Scene()
                    {
                        Name = AudioTestName,
                        BackgroundClips = new List<String>()
                        {
                        }
                    };
                }

                return audioTest;
            }
        }

        public static List<Scene> GetGameScenes()
        {
            return new List<Scene>()
            {
                Game,
                MainMenu,
                Options,
                SavedGames,
                Credits,
                GameMode,
                LevelCompleted,
                GameOver,
                ShootingStars
            };
        }

        public static List<Scene> GetDevelopmentScenes()
        {
            return new List<Scene>()
            {
                GameTest,
                AudioTest
            };
        }

        public static IList<Scene> GetAll()
        {
            return GetGameScenes().Concat(GetDevelopmentScenes()).ToList();
        }
    }
}
