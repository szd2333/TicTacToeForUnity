using UnityEngine;
using UObject = UnityEngine.Object;

namespace TTT
{
    public class goutil
    {
        public static bool IsNil(GameObject gameObject)
        {
            return gameObject == null || gameObject.Equals(null);
        }
        
        public static bool IsNil(Component component)
        {
            if (component == null)
            {
                return true;
            }

            return IsNil(component.gameObject);
        }

        public static bool IsNil(UObject uObj)
        {
            if (uObj is GameObject)
            {
                return IsNil(uObj as GameObject);
            }
            if (uObj is Component)
            {
                return IsNil(uObj as Component);
            }

            return uObj == null;
        }

        public static void SetActive(GameObject gameObject, bool active)
        {
            if (goutil.IsNil(gameObject))
            {
                return;
            }
            gameObject.SetActive(active);
        }
    }
}