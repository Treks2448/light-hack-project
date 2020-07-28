using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Progression
{
    public class LevelManager : MonoBehaviour
    {
        // Varieables visible in editor
        [SerializeField] private Level firstLevel;
        
        // Private Variables
        private Level[] levels;
        private Level currentLevel;
        private bool saveFileExists = false;

        void Start()
        {
            // Find all the levels in the scene. Used for saving/loading levels
            levels = new Level[GetComponentsInChildren<Level>().Length];
            for (int i = 0; i < GetComponentsInChildren<Level>().Length; i++)
            {
                levels[i] = GetComponentsInChildren<Level>()[i];
            }
            Load();
            // TODO: remove before final release of the game
            UnlockAllLevels();
        }

        // Public Functions
        public bool SaveFileExists() { return saveFileExists; }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/ProgressInfo.dat");

            LevelsData data = new LevelsData(levels.Length);
            for (int i = 0; i < levels.Length; i++)
            {
                data.levelStates[i] = levels[i].GetCurrentState();
            }

            bf.Serialize(file, data);
            file.Close();
        }

        public void Reset()
        {
            // Reset all levels to locked state both in the save file and in the game
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/ProgressInfo.dat");

            LevelsData data = new LevelsData(levels.Length);
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].SetState(Level.State.locked);

                data.levelStates[i] = Level.State.locked;
            }

            bf.Serialize(file, data);
            file.Close();

            // Unlock the first level and save the game
            firstLevel.SetState(Level.State.unlocked);
            Save();
        }

        public void Load() // Loads player progress from file
        {
            if (File.Exists(Application.persistentDataPath + "/ProgressInfo.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/ProgressInfo.dat", FileMode.Open);
                LevelsData data = new LevelsData(levels.Length);
                data = (LevelsData)bf.Deserialize(file);
                
                file.Close();

                for (int i = 0; i < data.levelStates.Length; i++)
                {
                    levels[i].SetState(data.levelStates[i]);
                }
                saveFileExists = true;
            }
            else
            {
                saveFileExists = false;
            }
        }

        public void StartLatestUnlockedLevel() // called on "continue from last game" option selected
        {
            Level level = firstLevel;
            while (level.GetNextLevel() != null)
            {
                // load this level if it was active last time
                if (level.GetCurrentState() == Level.State.active)
                { 
                    level.Load();
                    currentLevel = level;
                    return; 
                }
                // if the next level is locked, then load this level
                if (level.GetNextLevel().GetCurrentState() == Level.State.locked) 
                {
                    level.Load();
                    currentLevel = level;
                    return;
                }
                // Otherwise, repeat steps for the next level
                level = level.GetNextLevel();
            }

            // Load the last level if all levels have been unlocked
            level.Load();
            currentLevel = level;
        }

        public void StartLevel(Level level)
        {
            if (level.GetCurrentState() != Level.State.locked)
            {
                level.Load();
                currentLevel = level;
            }
        }

        public Level CurrentLevel()
        {
            return currentLevel;
        }

        public void UnlockAllLevels() // TODO: Cheat, remove later
        {
            Level level = firstLevel;
            while (level.GetNextLevel() != null)
            {
                level.Complete();
                level = level.GetNextLevel();
            }
        }
    }

    [Serializable]
    public class LevelsData
    {
        public Level.State[] levelStates;
        public LevelsData(int nStates)
        {
            levelStates = new Level.State[nStates];
        }
        
    }
}

