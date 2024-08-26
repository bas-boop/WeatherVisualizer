using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Canvas
{
    public sealed class ScrollUI : MonoBehaviour
    {
        [SerializeField] private RectTransform thisRect;
        [SerializeField] private float easeTime = 0.5f;
        [SerializeField] private int totalPages = 3;

        public int CurrentPage { get; private set; }

        [SerializeField, Space] private UnityEvent onChange;

        private bool _isScrolling;
        private float _viewportWidth;
        private float _maxPosition;

        private void Awake()
        {
            if (thisRect == null) 
                thisRect = GetComponent<RectTransform>();

            totalPages = transform.childCount;
            
            _viewportWidth = ((RectTransform) thisRect.parent).rect.width;
            UpdateMaxPosition();
        }

        public void AddPage()
        {
            totalPages++;
            UpdateMaxPosition();
        }

        public void RemovePage()
        {
            if (totalPages == 1)
                return;
            
            totalPages--;
            UpdateMaxPosition();
        }
        
        public void ScrollToNextPosition()
        {
            if (CurrentPage == 1 - totalPages)
                return;
            
            ScrollToPosition(-1);
        }

        public void ScrollToPreviousPosition()
        {
            if (CurrentPage == 0)
                return;
            
            ScrollToPosition(1);
        }

        private void ScrollToPosition(int direction)
        {
            if (_isScrolling)
                return;
            
            Vector2 targetPosition = thisRect.anchoredPosition + new Vector2(_viewportWidth * direction, 0);
            targetPosition.x = Mathf.Clamp(targetPosition.x, -_maxPosition, 0);
            CurrentPage += direction;
            
            StartCoroutine(ScrollToTarget(targetPosition));
        }
        
        private void UpdateMaxPosition() => _maxPosition = _viewportWidth * (totalPages - 1);

        private IEnumerator ScrollToTarget(Vector2 targetPosition)
        {
            Vector2 startPosition = thisRect.anchoredPosition;
            float elapsedTime = 0f;
            
            while (elapsedTime < easeTime)
            {
                _isScrolling = true;
                thisRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / easeTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isScrolling = false;
            thisRect.anchoredPosition = targetPosition;
            onChange?.Invoke();
        }
    }
}