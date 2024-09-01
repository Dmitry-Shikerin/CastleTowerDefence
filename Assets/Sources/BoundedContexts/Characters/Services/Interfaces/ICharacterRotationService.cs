using UnityEngine;

namespace Sources.BoundedContexts.Characters.Services.Interfaces
{
    public interface ICharacterRotationService
    {
        float GetAngleRotation(Vector3 enemyPosition, Vector3 characterPosition);
    }
}