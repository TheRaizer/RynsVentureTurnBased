public interface ILevel//health exponentially increase while damage scale linearly increases
{
    void OnLevelUp();
    float PercentHealthIncreasePerLevel { get; }
    float DamageScaleIncreasePerLevel { get; }//since damage scale is multiplied by each attacks damage the scale does not need to exponentially increase
}
