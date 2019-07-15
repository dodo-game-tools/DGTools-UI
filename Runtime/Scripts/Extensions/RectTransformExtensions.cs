using UnityEngine;

namespace DGTools.UI
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Set the scale to 1,1,1
        /// </summary>
        public static void SetDefaultScale(this RectTransform trans)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Set the point in which both anchors and the pivot should be placed. This makes it very easy to set positions and scales, but it destroys autoscaling.
        /// </summary>
        /// <param name="aVec">Point in which both anchors and the pivot should be placed</param>
        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        /// <summary>
        /// Get the current size of the <see cref="RectTransform"/> as a Vector2
        /// </summary>
        public static Vector2 GetSize(this RectTransform trans)
        {
            return trans.rect.size;
        }

        /// <summary>
        /// Get the width of the rect
        /// </summary>
        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }

        /// <summary>
        /// Get the height of the rect
        /// </summary>
        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        /// <summary>
        /// Set the position of the <see cref="RectTransform"/> within it's parent's coordinates. Depending on the position of the pivot, the RectTransform actual position will differ.
        /// </summary>
        /// <param name="newPos">The new position of the <see cref="RectTransform"/></param>
        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        }

        /// <summary>
        /// Set the position of a specific corner of the <see cref="RectTransform"/> within it's parent's coordinates.
        /// </summary>
        /// <param name="newPos">The new position of the <see cref="RectTransform"/></param>
        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        /// <summary>
        /// Set the position of a specific corner of the <see cref="RectTransform"/> within it's parent's coordinates.
        /// </summary>
        /// <param name="newPos">The new position of the <see cref="RectTransform"/></param>
        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        /// <summary>
        /// Set the position of a specific corner of the <see cref="RectTransform"/> within it's parent's coordinates.
        /// </summary>
        /// <param name="newPos">The new position of the <see cref="RectTransform"/></param>
        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        /// <summary>
        /// Set the position of a specific corner of the <see cref="RectTransform"/> within it's parent's coordinates.
        /// </summary>
        /// <param name="newPos">The new position of the <see cref="RectTransform"/></param>
        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        /// <summary>
        /// Set the dimensions of the <see cref="RectTransform"/> regardless of its anchors, pivot and offsets.
        /// </summary>
        /// <param name="newSize">The new size of the <see cref="RectTransform"/></param>
        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        /// <summary>
        /// Set the width of the <see cref="RectTransform"/> regardless of its anchors, pivot and offsets.
        /// </summary>
        /// <param name="newSize">The new width of the <see cref="RectTransform"/></param>
        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        /// <summary>
        /// Set the height of the <see cref="RectTransform"/> regardless of its anchors, pivot and offsets.
        /// </summary>
        /// <param name="newSize">The new height of the <see cref="RectTransform"/></param>
        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

    }
}
