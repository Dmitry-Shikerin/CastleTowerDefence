using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRotations.Services.Implementation
{
    public class CharacterRotationService : ICharacterRotationService
    {
        public float GetAngleRotation(Vector3 enemyPosition, Vector3 characterPosition)
        {
            Vector3 lookDirection = enemyPosition - characterPosition;
            lookDirection.y = characterPosition.y;

            return Vector3.SignedAngle(Vector3.forward, lookDirection, Vector3.up);
        }
    }
}