using UnityEditor;
using UnityEngine;


namespace LostLight
{


    public class AutoBoxColliderTool
    {
        [MenuItem("Tools/一键为子物体添加自适应BoxCollider")]
        private static void AddCollidersToSelection()
        {
            // 检查是否有物体被选中
            if (Selection.activeGameObject == null)
            {
                Debug.LogWarning("请先选择一个父物体。");
                return;
            }

            GameObject parentObj = Selection.activeGameObject;
            // 开始递归处理
            AddCollidersRecursively(parentObj.transform);
        }

        private static void AddCollidersRecursively(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // 如果子物体有渲染器，则为其添加BoxCollider
                if (child.GetComponent<Renderer>() != null)
                {
                    // 先检查是否已存在碰撞体，避免重复添加
                    if (child.GetComponent<Collider>() == null)
                    {
                        child.gameObject.AddComponent<BoxCollider>();
                    }
                }
                // 继续递归处理更深层级的子物体
                if (child.childCount > 0)
                {
                    AddCollidersRecursively(child);
                }
            }
            Debug.Log("所有子物体碰撞体添加完成！");
        }
    }

}