using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    public class HotelMovementSystem : MonoBehaviour
    {
        public float moveSpeed;

        [SerializeField] private RectTransform referenceRect;
        [SerializeField] private Targetlayer[] layerTargets;
        [SerializeField] private Transition[] transitions;
        [Space]
        /*[Header("Test")]
        [SerializeField] private RectTransform rectTransformForMoveTest;
        [SerializeField] private Vector2 testTargetPosition;*/
        [Space]
        [SerializeField] private bool debug = true;

        private void OnEnable()
        {
            debug = false;
        }

        [System.Serializable]
        public struct Targetlayer
        {
            public float Y;
            public float[] X;
        }

        [System.Serializable]
        public struct Transition
        {
            public int TargetLayerIdA;
            public float pointA;
            public int TargetLayerIdB;
            public float pointB;
        }

/*        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MoveRectToHotelTargetCur(rectTransformForMoveTest, testTargetPosition, () => Debug.Log("Finish"));
            }
        }*/
        public void MoveRectToHotelTargetCur(RectTransform rectTransform, Vector2 targetPos, Action onComplete)
        {
            StartCoroutine(MoveRectToHotelTarget(rectTransform, targetPos, onComplete));
        }
        private IEnumerator MoveRectToHotelTarget(RectTransform rectTransform, Vector2 targetPos, Action onComplete)
        {
            float startX, startY;
            FindClosestTargetInLayers(rectTransform.anchoredPosition, out startX, out startY);
            rectTransform.anchoredPosition = new Vector2(startX, startY);

            List<Vector2> chainPos = new List<Vector2>();

            float targetX, targetY;
            FindClosestTargetInLayers(targetPos, out targetX, out targetY);

            while (true)
            {
                if (rectTransform.anchoredPosition.y == targetY)
                {
                    yield return MoveRectToTarget(rectTransform, new Vector2(targetX, targetY));
                    break;
                }
                else
                {
                    Vector2 start;
                    Vector2 end;
                    FindClosestTargetInTransition(rectTransform.anchoredPosition.y, targetY, out start, out end);

                    if (rectTransform.anchoredPosition.y > start.y)
                    {
                        yield return MoveRectToTarget(rectTransform, end);
                        yield return MoveRectToTarget(rectTransform, start);
                    }
                    else
                    {
                        yield return MoveRectToTarget(rectTransform, start);
                        yield return MoveRectToTarget(rectTransform, end);
                    }
                }
            }

            onComplete?.Invoke();
        }

        private IEnumerator MoveRectToTarget(RectTransform rectTransform, Vector2 target)
        {
            while (Vector2.Distance(rectTransform.anchoredPosition, target) > 0.01f)
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
            rectTransform.anchoredPosition = target;
        }

        private void FindClosestTargetInLayers(Vector2 position, out float targetX, out float targetY)
        {
            float minDis = float.MaxValue;
            targetX = 0;
            targetY = 0;

            foreach (var layer in layerTargets)
            {
                foreach (var item in layer.X)
                {
                    float dist = Vector2.Distance(position, new Vector2(item, layer.Y));
                    if (dist < minDis)
                    {
                        minDis = dist;
                        targetX = item;
                        targetY = layer.Y;
                    }
                }
            }
        }

        private void FindClosestTargetInTransition(float currentY, float targetY, out Vector2 start, out Vector2 end)
        {
            Transition closestTransition = transitions[0];
            float minCurrentDistance = float.MaxValue;
            float minTargetDistance = float.MaxValue;

            foreach (var item in transitions)
            {
                float distanceA = Mathf.Abs(currentY - layerTargets[item.TargetLayerIdA].Y);
                float distanceB = Mathf.Abs(currentY - layerTargets[item.TargetLayerIdB].Y);
                float curDistance = Mathf.Min(distanceA, distanceB);
                float targetMidY = (layerTargets[item.TargetLayerIdA].Y + layerTargets[item.TargetLayerIdB].Y) / 2f;
                float targetDistance = Mathf.Abs(targetY - targetMidY);

                if (curDistance < minCurrentDistance)
                {
                    minCurrentDistance = curDistance;
                    minTargetDistance = targetDistance;
                    closestTransition = item;
                }
                else if (curDistance == minCurrentDistance && targetDistance < minTargetDistance)
                {
                    minTargetDistance = targetDistance;
                    closestTransition = item;
                }
            }

            if (Mathf.Abs(currentY - layerTargets[closestTransition.TargetLayerIdA].Y) <
                    Mathf.Abs(currentY - layerTargets[closestTransition.TargetLayerIdB].Y))
            {
                start = new Vector2(closestTransition.pointA, layerTargets[closestTransition.TargetLayerIdA].Y);
                end = new Vector2(closestTransition.pointB, layerTargets[closestTransition.TargetLayerIdB].Y);
            }
            else
            {
                start = new Vector2(closestTransition.pointB, layerTargets[closestTransition.TargetLayerIdB].Y);
                end = new Vector2(closestTransition.pointA, layerTargets[closestTransition.TargetLayerIdA].Y);
            }
        }

        private void OnDrawGizmos()
        {
            if (referenceRect == null || !debug) return;

            Vector3 center = referenceRect.position;
            Vector2 scale = referenceRect.lossyScale;

            /*Gizmos.color = Color.magenta;
            Vector3 testTargetPos = center + new Vector3(testTargetPosition.x * scale.x, testTargetPosition.y * scale.y, 0);
            Gizmos.DrawSphere(testTargetPos, 20);*/

            // Рисуем горизонтальные точки (красные сферы) и соединяем их линиями
            if (layerTargets != null)
            {
                foreach (var layer in layerTargets)
                {
                    Vector3? previousPoint = null;
                    foreach (var item in layer.X)
                    {
                        Gizmos.color = Color.red;
                        Vector3 worldPos = center + new Vector3(item * scale.x, layer.Y * scale.y, 0);
                        Gizmos.DrawSphere(worldPos, 20);

                        // Рисуем линию между соседними точками
                        if (previousPoint.HasValue)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(previousPoint.Value, worldPos);
                        }
                        previousPoint = worldPos;
                    }
                }
            }

            // Рисуем переходы
            if (transitions != null)
            {
                foreach (var transition in transitions)
                {
                    Vector3 worldPosA = center + new Vector3(transition.pointA * scale.x, layerTargets[transition.TargetLayerIdA].Y * scale.y, 0);
                    Vector3 worldPosB = center + new Vector3(transition.pointB * scale.x, layerTargets[transition.TargetLayerIdB].Y * scale.y, 0);

                    // Рисуем точки переходов (жёлтые сферы)
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(worldPosA, 20);
                    Gizmos.DrawSphere(worldPosB, 20);

                    // Рисуем линию между ними (синяя)
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(worldPosA, worldPosB);
                }
            }
        }
    }
}