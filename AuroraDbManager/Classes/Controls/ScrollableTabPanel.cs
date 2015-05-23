// 
// 	ScrollableTabPanel.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 23/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Classes.Controls {
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public class ScrollableTabPanel: Panel, IScrollInfo, INotifyPropertyChanged {
        private static readonly GradientStopCollection OpacityMaskStopsTransparentOnLeftAndRight = new GradientStopCollection {
                                                                                                                                  new GradientStop(Colors.Transparent, 0.0),
                                                                                                                                  new GradientStop(Colors.Black, 0.2),
                                                                                                                                  new GradientStop(Colors.Black, 0.8),
                                                                                                                                  new GradientStop(Colors.Transparent, 1.0)
                                                                                                                              };

        private static readonly GradientStopCollection OpacityMaskStopsTransparentOnLeft = new GradientStopCollection {
                                                                                                                          new GradientStop(Colors.Transparent, 0),
                                                                                                                          new GradientStop(Colors.Black, 0.5)
                                                                                                                      };

        private static readonly GradientStopCollection OpacityMaskStopsTransparentOnRight = new GradientStopCollection {
                                                                                                                           new GradientStop(Colors.Black, 0.5),
                                                                                                                           new GradientStop(Colors.Transparent, 1)
                                                                                                                       };

        public static readonly DependencyProperty RightOverflowMarginProperty = DependencyProperty.Register("RightOverflowMargin", typeof(int), typeof(ScrollableTabPanel),
                                                                                                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty AnimationTimeSpanProperty = DependencyProperty.Register("AnimationTimeSpanProperty", typeof(TimeSpan), typeof(ScrollableTabPanel),
                                                                                                          new FrameworkPropertyMetadata(new TimeSpan(0, 0, 0, 0, 100),
                                                                                                                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineScrollPixelCountProperty = DependencyProperty.Register("LineScrollPixelCount", typeof(int), typeof(ScrollableTabPanel),
                                                                                                             new FrameworkPropertyMetadata(15, FrameworkPropertyMetadataOptions.AffectsRender));

        private readonly TranslateTransform _scrollTransform = new TranslateTransform();
        private bool _canScrollHorizontally = true;
        private Size _controlExtent = new Size(0, 0);
        private Vector _offset;
        private ScrollViewer _owningScrollViewer;
        private Size _viewport = new Size(0, 0);

        public ScrollableTabPanel() {
            RenderTransform = _scrollTransform;
            SizeChanged += OnSizeChanged;
        }

        public Size Extent { get { return _controlExtent; } private set { _controlExtent = value; } }

        public Size Viewport { get { return _viewport; } private set { _viewport = value; } }

        public bool IsOnFarLeft { get { return Math.Abs(HorizontalOffset) <= 0.0; } }

        public bool IsOnFarRight { get { return Math.Abs((HorizontalOffset + Viewport.Width) - ExtentWidth) <= 0.0; } }

        public bool CanScroll { get { return ExtentWidth > Viewport.Width; } }

        public bool CanScrollLeft { get { return CanScroll && !IsOnFarLeft; } }

        public bool CanScrollRight { get { return CanScroll && !IsOnFarRight; } }

        public int RightOverflowMargin { get { return (int)GetValue(RightOverflowMarginProperty); } set { SetValue(RightOverflowMarginProperty, value); } }

        public TimeSpan AnimationTimeSpan { get { return (TimeSpan)GetValue(AnimationTimeSpanProperty); } set { SetValue(AnimationTimeSpanProperty, value); } }

        public int LineScrollPixelCount { get { return (int)GetValue(LineScrollPixelCountProperty); } set { SetValue(LineScrollPixelCountProperty, value); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanHorizontallyScroll { get { return _canScrollHorizontally; } set { _canScrollHorizontally = value; } }

        public bool CanVerticallyScroll { get { return false; } set { } }

        public double ExtentHeight { get { return Extent.Height; } }

        public double ExtentWidth { get { return Extent.Width; } }

        public double HorizontalOffset { get { return _offset.X; } private set { _offset.X = value; } }

        public void LineDown() { }

        public void LineLeft() { SetHorizontalOffset(HorizontalOffset - LineScrollPixelCount); }

        public void LineRight() { SetHorizontalOffset(HorizontalOffset + LineScrollPixelCount); }

        public void LineUp() { }

        public Rect MakeVisible(Visual visual, Rect rectangle) {
            if(rectangle.IsEmpty || visual == null || Equals(visual, this) || !IsAncestorOf(visual))
                return Rect.Empty;
            var ctrl = InternalChildren.Cast<UIElement>().FirstOrDefault(child => child.Equals(visual));
            if(ctrl == null)
                return rectangle;
            if(Equals(ctrl, InternalChildren[0]))
                SetHorizontalOffset(0);
            else if(Equals(ctrl, InternalChildren[InternalChildren.Count - 1]))
                SetHorizontalOffset(ExtentWidth - Viewport.Width);
            else {
                var offset = GetLeftEdge(ctrl);
                SetHorizontalOffset(CalculateNewScrollOffset(HorizontalOffset, HorizontalOffset + Viewport.Width, offset, offset + ctrl.DesiredSize.Width));
            }
            return new Rect(HorizontalOffset, 0, ctrl.DesiredSize.Width, Viewport.Height);
        }

        public void MouseWheelDown() { }

        public void MouseWheelLeft() { }

        public void MouseWheelRight() { }

        public void MouseWheelUp() { }

        public void PageDown() { }

        public void PageLeft() { }

        public void PageRight() { }

        public void PageUp() { }

        public ScrollViewer ScrollOwner {
            get { return _owningScrollViewer; }
            set {
                _owningScrollViewer = value;
                if(_owningScrollViewer != null)
                    ScrollOwner.Loaded += OnScrollOwnerLoaded;
                else
                    ScrollOwner.Loaded -= OnScrollOwnerLoaded;
            }
        }

        public void SetHorizontalOffset(double offset) {
            RemoveOpacityMasks();
            HorizontalOffset = Math.Max(0, Math.Min(ExtentWidth - Viewport.Width, Math.Max(0, offset)));
            if(ScrollOwner != null)
                ScrollOwner.InvalidateScrollInfo();
            var scrollAnimation = new DoubleAnimation(_scrollTransform.X, (-HorizontalOffset), new Duration(AnimationTimeSpan), FillBehavior.HoldEnd) {
                                                                                                                                                          AccelerationRatio = 0.5,
                                                                                                                                                          DecelerationRatio = 0.5
                                                                                                                                                      };
            scrollAnimation.Completed += OnScrollAnimationCompleted;
            _scrollTransform.BeginAnimation(TranslateTransform.XProperty, scrollAnimation, HandoffBehavior.Compose);
            InvalidateMeasure();
        }

        public void SetVerticalOffset(double offset) { }

        public double VerticalOffset { get { return 0; } }

        public double ViewportHeight { get { return Viewport.Height; } }

        public double ViewportWidth { get { return Viewport.Width; } }

        private static double CalculateNewScrollOffset(double leftViewPort, double rightViewPort, double leftChild, double rightChild) {
            var isLeft = leftChild < leftViewPort && rightChild < rightViewPort;
            if (!isLeft && !(rightChild > rightViewPort && leftChild > leftViewPort))
                return leftViewPort;
            if (isLeft && !(rightChild - leftChild > rightViewPort - leftViewPort))
                return leftChild;
            return (rightChild - (rightViewPort - leftViewPort));
        }

        private void UpdateMembers(Size extent, Size viewportSize) {
            if(extent != Extent) {
                Extent = extent;
                if(ScrollOwner != null)
                    ScrollOwner.InvalidateScrollInfo();
            }
            if(viewportSize != Viewport) {
                Viewport = viewportSize;
                if(ScrollOwner != null)
                    ScrollOwner.InvalidateScrollInfo();
            }
            if(HorizontalOffset + Viewport.Width + RightOverflowMargin > ExtentWidth)
                SetHorizontalOffset(HorizontalOffset + Viewport.Width + RightOverflowMargin);
            NotifyPropertyChanged("CanScroll");
            NotifyPropertyChanged("CanScrollLeft");
            NotifyPropertyChanged("CanScrollRight");
        }

        private double GetLeftEdge(UIElement child) {
            double totalWidth = 0;
            foreach(UIElement element in InternalChildren) {
                var width = element.DesiredSize.Width;
                if(child != null && Equals(child, element))
                    return totalWidth;
                totalWidth += width;
            }
            return totalWidth;
        }

        private double OverflowToRight(UIElement child) {
            var intersect = GetIntersectionRectangle(child);
            if(intersect != Rect.Empty && CanScrollRight && intersect.Width < child.DesiredSize.Width && intersect.X > 0)
                return intersect.Width / child.DesiredSize.Width;
            return 1;
        }

        private double OverflowToLeft(UIElement child) {
            var intersect = GetIntersectionRectangle(child);
            if(intersect != Rect.Empty && CanScrollLeft && intersect.Width < child.DesiredSize.Width && Math.Abs(intersect.X) < 0.0)
                return intersect.Width / child.DesiredSize.Width;
            return 1;
        }

        private Rect ScrollViewerRectangle { get { return new Rect(new Point(0, 0), ScrollOwner.RenderSize); } }

        private Rect GetChildRectangle(UIElement child) {
            var childTransform = child.TransformToAncestor(ScrollOwner);
            return childTransform.TransformBounds(new Rect(new Point(0, 0), child.RenderSize));
        }

        private Rect GetIntersectionRectangle(UIElement child) {
            return Rect.Intersect(ScrollViewerRectangle, GetChildRectangle(child));
        }

        private void RemoveOpacityMasks() {
            foreach(UIElement child in Children)
                child.OpacityMask = null;
        }

        private void UpdateOpacityMasks() {
            var rect = ScrollViewerRectangle;
            if(rect == Rect.Empty)
                return;
            foreach(UIElement child in Children) {
                var childRect = GetChildRectangle(child);
                if(rect.Contains(childRect))
                    child.OpacityMask = null;
                else {
                    var overflowLeft = OverflowToLeft(child);
                    var overflowRight = OverflowToRight(child);
                    if(overflowLeft < 1 && overflowRight < 1)
                        child.OpacityMask = new LinearGradientBrush(OpacityMaskStopsTransparentOnLeftAndRight, new Point(0, 0), new Point(1, 0));
                    else if(overflowLeft < 1)
                        child.OpacityMask = new LinearGradientBrush(OpacityMaskStopsTransparentOnLeft, new Point(1 - overflowLeft, 0), new Point(1, 0));
                    else if(overflowRight < 1)
                        child.OpacityMask = new LinearGradientBrush(OpacityMaskStopsTransparentOnRight, new Point(0, 0), new Point(overflowRight, 0));
                    else
                        child.OpacityMask = null;
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize) {
            var resultSize = new Size(0, availableSize.Height);
            foreach(UIElement child in InternalChildren) {
                child.Measure(availableSize);
                resultSize.Width += child.DesiredSize.Width;
            }
            UpdateMembers(resultSize, availableSize);
            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ? resultSize.Width : availableSize.Width;
            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            if(InternalChildren == null || InternalChildren.Count < 1)
                return finalSize;
            double totalWidth = 0;
            foreach(UIElement child in InternalChildren) {
                var width = child.DesiredSize.Width;
                child.Arrange(new Rect(totalWidth, 0, width, child.DesiredSize.Height));
                totalWidth += width;
            }
            return finalSize;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved) {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            UpdateOpacityMasks();
        }

        protected override void OnChildDesiredSizeChanged(UIElement child) {
            base.OnChildDesiredSizeChanged(child);
            UpdateOpacityMasks();
        }

        private void NotifyPropertyChanged(String strPropertyName) {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(strPropertyName));
        }

        private void OnScrollOwnerLoaded(object sender, RoutedEventArgs e) { UpdateOpacityMasks(); }

        private void OnScrollAnimationCompleted(object sender, EventArgs e) {
            UpdateOpacityMasks();
            foreach(UIElement child in InternalChildren)
                child.InvalidateArrange();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) { UpdateOpacityMasks(); }
    }
}