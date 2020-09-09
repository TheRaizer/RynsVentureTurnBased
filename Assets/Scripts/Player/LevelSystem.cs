using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSystem
{
    [field: SerializeField] public int Experience { get; set; }
    [field: SerializeField] public int ExperienceToNextLevel { get; private set; }

    [SerializeField] private float percentExperiencePerLevel;
    [SerializeField] private int level = 1;

    private readonly ILevel entityToLevel;

    public LevelSystem(ILevel _entityToLevel, int baseExperienceToNextLevel, float _percentExperiencePerLevel)
    {
        entityToLevel = _entityToLevel;
        ExperienceToNextLevel = baseExperienceToNextLevel;
        percentExperiencePerLevel = _percentExperiencePerLevel;
    }

    public void CheckLevel()
    {
        if(Experience < ExperienceToNextLevel)
        {
            Debug.Log("Current Exp: " + Experience);
            Debug.Log("Attempted to Level up without required amount of exp");
            return;
        }

        while(Experience >= ExperienceToNextLevel)
        {
            Experience -= ExperienceToNextLevel;
            ExperienceToNextLevel += MathExtension.RoundToNearestInteger(ExperienceToNextLevel * percentExperiencePerLevel);
            level++;
            entityToLevel.OnLevelUp();
        }

        Debug.Log("Current Exp: " + Experience);
        Debug.Log("Exp To next level: " + ExperienceToNextLevel);
    }
}
