using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class HotelMovementSystem : MonoBehaviour
{  
    [SerializeField] private RectTransform rectTransformForMoveTest;
    [SerializeField] private Vector2 testTargetPosition;
    [SerializeField] private float moveSpeed;
    [Space]
    [SerializeField] private RectTransform referenceRect;
    [SerializeField] private Targetlayer[] layerTargets;
    [SerializeField] private Transition[] transitions;
    [Space]
    [SerializeField] private bool debug = true;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MoveRectToHotelTarget(rectTransformForMoveTest, testTargetPosition, () => Debug.Log("Finish")));
        }
    }

    public IEnumerator MoveRectToHotelTarget(RectTransform rectTransform, Vector2 targetLocalPos, Action onComplete)
    {
        float startX, startY;
        FindClosestTargetInLayers(rectTransform.anchoredPosition, out startX, out startY);
        rectTransform.anchoredPosition = new Vector2(startX, startY);

        List<Vector2> chainPos = new List<Vector2>();

        float targetX, targetY;
        FindClosestTargetInLayers(targetLocalPos, out targetX, out targetY);
        
        while (true)
        {
            if (rectTransform.anchoredPosition.y == targetY)
            {
                yield return MoveRectToTarget(rectTransform, new Vector2(targetX, targetY));
                break;
            }
            else
            {
                Debug.Log("Layer");

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

                Debug.Log($"Start: X:{start.x} Y:{start.y}");
                Debug.Log($"End: X:{end.x} Y:{end.y}");
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

        Debug.Log($"currentY: {currentY} targetY: {targetY}");

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

        Gizmos.color = Color.magenta;
        Vector3 testTargetPos = center + new Vector3(testTargetPosition.x * scale.x, testTargetPosition.y * scale.y, 0);
        Gizmos.DrawSphere(testTargetPos, 20);

        // Рисуем горизонтальные точки (красные сферы)
        if (layerTargets != null)
        {
            Gizmos.color = Color.red;
            foreach (var layer in layerTargets)
            {
                foreach (var item in layer.X)
                {
                    Vector3 worldPos = center + new Vector3(item * scale.x, layer.Y * scale.y, 0);
                    Gizmos.DrawSphere(worldPos, 20);
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

                // Рисуем точки переходов (зелёные сферы)
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(worldPosA, 20);
                Gizmos.DrawSphere(worldPosB, 20);

                // Рисуем линию между ними (синяя)
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(worldPosA, worldPosB);
            }
        }
    }
}
