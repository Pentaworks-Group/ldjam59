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

                return gameTest;
            }
        }

        public const String AudioTestName = "AudioTest";
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
                MainMenu,
                Game
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
