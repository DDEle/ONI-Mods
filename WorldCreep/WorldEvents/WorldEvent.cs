﻿using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class WorldEvent : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        [Serialize]
        public bool immediateStart;
        [SerializeField]
        [Serialize]
        public float power;
        [SerializeField]
        [Serialize]
        public float durationInSeconds;
        [SerializeField]
        public Dictionary<int, float> affectedCells;
        public SchedulerHandle schedule;

        public float DurationInCycles
        {
            get => durationInSeconds / 600f;
            set => durationInSeconds = value * 600f;
        }

        public abstract void Begin();
        public abstract void End();
        protected abstract void SetPower();

        protected override void OnSpawn()
        {
            base.OnSpawn();
            WorldEventScheduler.Instance.WorldEvents.Add(this);
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            WorldEventScheduler.Instance.WorldEvents.Remove(this);
        }
    }
}
