using UnityEngine;

namespace Blade.Combat
{
    public interface IKnockBackable
    {
        public void KnockBack(Vector3 direction, MovementDataSO knockBackData);
    }
}