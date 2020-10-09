using UnityEngine;

public interface IUser
{
    string Id { get; }
    EntityType EntityType { get; }
    Animator Animator { get; }
}