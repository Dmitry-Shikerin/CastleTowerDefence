using UnityEngine;

namespace Sources.BoundedContexts.CharacterRotations.Services.Interfaces
{
    public interface ICharacterRotationService
    {
        float GetAngleRotation(Vector3 enemyPosition, Vector3 characterPosition);
    }
}