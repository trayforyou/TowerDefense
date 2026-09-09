using System;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyAnimator : IDisposable
    {
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        private static readonly int Attack = Animator.StringToHash("Attack");

        private readonly Animator _animator;
        private readonly Mover _mover;
        private readonly EnemyAttacker _attacker;

        public EnemyAnimator(Animator animator, Mover mover, EnemyAttacker attacker)
        {
            _mover = mover;
            _attacker = attacker;
            _animator = animator;
        }

        public void TurnOff() =>
            UnsubscribeAll();

        public void TurnOn() =>
            SubscribeAll();

        public void Stop()
        {
            if (_animator != null)
                _animator.SetBool(IsRun, false);
        }

        public void Dispose() =>
            UnsubscribeAll();

        private void SubscribeAll()
        {
            _mover.Running += AnimateRun;
            _attacker.Attacking += AnimateAttack;
        }

        private void UnsubscribeAll()
        {
            _mover.Running -= AnimateRun;
            _attacker.Attacking -= AnimateAttack;
        }

        private void AnimateRun() =>
            _animator.SetBool(IsRun, true);

        private void AnimateAttack() =>
            _animator.SetTrigger(Attack);
    }
}