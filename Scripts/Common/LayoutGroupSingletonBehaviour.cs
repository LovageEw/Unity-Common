using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace UIs {
    public class LayoutGroupSingletonBehaviour<T> : LayoutGroupMonoBehaviour where T : LayoutGroupSingletonBehaviour<T> {
        protected static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null) {
                        UnityConsole.LogWarning(typeof(T) + " is nothing !!");
                    }
                }

                return instance;
            }
        }

        protected void Awake() {
            CheckInstance();
        }

        protected bool CheckInstance() {
            if (instance == null) {
                instance = (T)this;
                return true;
            } else if (Instance == this) {
                return true;
            }

            Destroy(this);
            return false;
        }

        public static bool IsExist {
            get {
                return instance != null;
            }
        }
    }
}