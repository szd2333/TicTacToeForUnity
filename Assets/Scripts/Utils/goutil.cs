using UnityEngine;

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
    }
}