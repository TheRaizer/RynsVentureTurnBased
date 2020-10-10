using UnityEngine;

public class BattleEntitySpritePositions : MonoBehaviour
{
    public Vector2 PlayerSpritesReferenceVector;
    public Vector2 EnemySpritesReferenceVector;

    public float StartinYPosForSprites { get; private set; }
    [field: SerializeField] public Transform SpritesParentTransform { get; private set; }

    private void Awake()
    {
        StartinYPosForSprites = PlayerSpritesReferenceVector.y;
    }

    public void ResetYOfSpriteReferences()
    {
        PlayerSpritesReferenceVector.y = StartinYPosForSprites;
        EnemySpritesReferenceVector.y = StartinYPosForSprites;
    }
}
