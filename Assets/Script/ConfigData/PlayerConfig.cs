using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Program.Data {
    public class PlayerConfig:SerializedScriptableObject {
        public int id { get; private set; }
        public string name { get; private set; }
        public int age { get; private set; }

        public PlayerConfig(int id, string name, int age) {
            this.id = id;
            this.name = name;
            this.age = age;
        }
    }

    public static class PlayerHelper {
        readonly static List<PlayerConfig> playerConfigs = new List<PlayerConfig>();
        static bool Initialized = false;

        public static void Initialize() {
            if (Initialized)
                return;
            Initialized = true;

            
        }
    }
}