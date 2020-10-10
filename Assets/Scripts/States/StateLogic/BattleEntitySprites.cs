using UnityEngine;

public class BattleEntitySprites
{
    private readonly BattleEntitiesManager battleEntitiesManager;
    private readonly BattleEntitySpritePositions spritePositions;

    public BattleEntitySprites(BattleEntitiesManager _battleEntitiesManager, BattleEntitySpritePositions _spritePositions)
    {
        battleEntitiesManager = _battleEntitiesManager;
        spritePositions = _spritePositions;
    }

    public void DestroyPlayerSprites()
    {
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                Object.Destroy(battleEntitiesManager.ActivePlayableCharacters[i].Animator.gameObject);
            }
        }
    }

    public void DestroyEnemySprites()
    {
        for (int i = 0; i < battleEntitiesManager.Enemies.Length; i++)
        {
            if (battleEntitiesManager.Enemies[i] != null)
            {
                Object.Destroy(battleEntitiesManager.Enemies[i].GetComponent<Enemy>().Animator.gameObject);
            }
        }
    }

    public void OutputActivePlayerSprites()
    {
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                GameObject g = Object.Instantiate(battleEntitiesManager.ActivePlayableCharacters[i].BattleVersionPrefab);
                battleEntitiesManager.ActivePlayableCharacters[i].Animator = g.GetComponent<Animator>();
                g.transform.SetParent(spritePositions.SpritesParentTransform);
                RectTransform rect = g.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(spritePositions.PlayerSpritesReferenceVector.x, spritePositions.PlayerSpritesReferenceVector.y);
                spritePositions.PlayerSpritesReferenceVector.y -= ConstantNumbers.SPACE_BETWEEN_ENTITIES;
            }
        }
        spritePositions.ResetYOfSpriteReferences();
    }

    public void OutputEnemySprites()
    {
        for (int i = 0; i < battleEntitiesManager.Enemies.Length; i++)
        {
            if (battleEntitiesManager.Enemies[i] != null)
            {
                Enemy enemy = battleEntitiesManager.Enemies[i].GetComponent<Enemy>();
                GameObject g = Object.Instantiate(enemy.BattleVersionPrefab);
                enemy.Animator = g.GetComponent<Animator>();
                g.transform.SetParent(spritePositions.SpritesParentTransform);
                RectTransform rect = g.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(spritePositions.EnemySpritesReferenceVector.x, spritePositions.EnemySpritesReferenceVector.y);
                spritePositions.EnemySpritesReferenceVector.y -= ConstantNumbers.SPACE_BETWEEN_ENTITIES;
            }
        }
        spritePositions.ResetYOfSpriteReferences();
    }
}
