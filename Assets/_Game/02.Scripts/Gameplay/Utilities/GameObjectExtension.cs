using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }

                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}