using MyAssets.Scripts.Data.SO;

namespace MyAssets.Scripts.Data
{
    public class GameData
    {
        public GoldData Gold { get; }
        public ScoreData Score { get; }
        public AbilityData[] Abilities { get; }
        public PlayerSO PlayerSO { get; }
        public SkinsData Skins { get; }
        public LevelData[] Levels { get; }
        public LevelData CurrentLevel { get; set; }

        public GameData(AbilitySO[] abilitiesSO, SkinSO[] skins, LevelSO[] levels, PlayerSO playerSO)
        {
            Levels = new LevelData[levels.Length];
            for (var i = 0; i < levels.Length; i++)
                Levels[i] = new LevelData(levels[i]);
            CurrentLevel = Levels[0];//ToDo
            Abilities = new AbilityData[abilitiesSO.Length];
            for (var i = 0; i < abilitiesSO.Length; i++)
                Abilities[i] = new AbilityData(abilitiesSO[i]);
            PlayerSO = playerSO;
            Gold = new GoldData();
            Score = new ScoreData();
            Skins = new SkinsData(skins);
        }
        
        public void CompleteCurrentLevel()
        {
            Gold.GoldCount += 1;
            CurrentLevel.LevelState = LevelState.Completed;
            foreach (var level in Levels)
                if (level.LevelSO.ID == CurrentLevel.LevelSO.ID + 1)
                {
                    level.LevelState = LevelState.Unlocked;
                    break;    
                }
        }
        
        public bool SkipCurrentLevel()
        {
            if(Gold.GoldCount < 2)
                return false;
            Gold.GoldCount -= 2;
            foreach (var level in Levels)
                if (level.LevelSO.ID == CurrentLevel.LevelSO.ID + 1)
                {
                    level.LevelState = LevelState.Unlocked;
                    break;    
                }
            return true;
        }
        
        public void SetNextLevel()
        {
            foreach (var level in Levels)
                if (level.LevelSO.ID == CurrentLevel.LevelSO.ID + 1)
                {
                    CurrentLevel = level;
                    break;    
                }
        }
    }
}