﻿using System.Collections.Generic;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;

            int temp = GetTemperature();

            if (temp >= overheatTemperature) return;

            for (int i = 0; i <= temp; i++)

            {

                var projectile = CreateProjectile(forTarget);

                AddProjectileToList(projectile, intoList);

            }

            IncreaseTemperature();
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            List<Vector2Int> reachableTargets = GetReachableTargets();
            if (reachableTargets.Count == 0)
            {
                return reachableTargets;
            }

            var nearestTarget = reachableTargets[0];
            float nearestDistance = float.MaxValue;

            foreach (Vector2Int target in reachableTargets)
            {
                float distanceToTarget = DistanceToOwnBase(target);
                if (distanceToTarget < nearestDistance)
                {
                    nearestTarget = target;
                    nearestDistance = distanceToTarget;
                }
            }

            List<Vector2Int> result = new List<Vector2Int>();
            result.Add(nearestTarget); // 
            return result;
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown / 10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                    _temperature = 0f;  // Сброс температуры после перегрева
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}