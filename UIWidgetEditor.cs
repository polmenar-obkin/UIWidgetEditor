using Frosty.Core.Controls;
using FrostySdk.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UIWidgetEditorPlugin.Models;

namespace UIWidgetEditorPlugin
{
    [TemplatePart(Name = PART_PropertyGridVisibilityButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_GridSplitter, Type = typeof(GridSplitter))]
    [TemplatePart(Name = PART_PropertyGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_MainGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_WidgetViewBox, Type = typeof(Viewbox))]
    [TemplatePart(Name = PART_WidgetViewGrid, Type = typeof(Grid))]
    public class UIWidgetEditor : FrostyAssetEditor
    {
        public bool PGVisible { get; set; } = false;
        private ScaleTransform View_Scale { get; set; }
        private TransformGroup View_TransformGroup { get; set; }
        private TranslateTransform View_Translate { get; set; }

        public UIWidgetEditor(ILogger inLogger) : base(inLogger)
        {
            View_TransformGroup = new TransformGroup();
            View_Scale = new ScaleTransform(1, 1);
            View_Translate = new TranslateTransform();

            View_TransformGroup.Children.Add(View_Scale);
            View_TransformGroup.Children.Add(View_Translate);
        }

        static UIWidgetEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWidgetEditor), new FrameworkPropertyMetadata(typeof(UIWidgetEditor)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            pgVisibilityBtn = GetTemplateChild(PART_PropertyGridVisibilityButton) as Button;
            gridSplitter = GetTemplateChild(PART_GridSplitter) as GridSplitter;
            propertyGrid = GetTemplateChild(PART_PropertyGrid) as Grid;
            mainGrid = GetTemplateChild(PART_MainGrid) as Grid;
            widgetViewBox = GetTemplateChild(PART_WidgetViewBox) as Viewbox;
            widgetViewGrid = GetTemplateChild(PART_WidgetViewGrid) as Grid;

            // collapse gridsplitter and property grid
            mainGrid.ColumnDefinitions[2].MinWidth = 0;
            mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
            mainGrid.ColumnDefinitions[2].Width = new GridLength(0);

            // expand/collapse property grid
            pgVisibilityBtn.Click += PGVisibilityBtn_Click;

            MouseWheel += UIWidgetEditor_MouseWheel;

            WidgetEditor widgetEdit = new WidgetEditor();
            widgetEdit.GetWidgetSize();
            widgetEdit.RenderSizeOnView(widgetViewBox);
        }

        private void UIWidgetEditor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool zoomOut = e.Delta < 0;

            double maxZoom = 3;
            double minZoom = 1;
            double zoomValue = 0.1;

            // set the origin of scale to where the mouse is currently positioned
            Point transformOrigin = Mouse.GetPosition(widgetViewGrid);
            transformOrigin.X = Math.Round(transformOrigin.X / widgetViewGrid.ActualWidth, 2);
            transformOrigin.Y = Math.Round(transformOrigin.Y / widgetViewGrid.ActualHeight, 2);

            widgetViewBox.RenderTransformOrigin = transformOrigin;
            widgetViewBox.RenderTransform = View_TransformGroup;

            if (zoomOut)
            {
                // zoom out
                View_Scale.ScaleX -= zoomValue;
            }
            else
            {
                // zoom in
                View_Scale.ScaleX += zoomValue;
            }

            View_Scale.ScaleX = Clamp(View_Scale.ScaleX, minZoom, maxZoom);
            View_Scale.ScaleY = Clamp(View_Scale.ScaleX, minZoom, maxZoom);
        }

        private void PGVisibilityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!PGVisible)
            {
                pgVisibilityBtn.Content = new Image() { Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/UIWidgetEditorPlugin;component/Images/right-arrow-collapse.png") as ImageSource };
                
                gridSplitter.Visibility = Visibility.Visible;
                propertyGrid.Visibility = Visibility.Visible;

                mainGrid.ColumnDefinitions[2].MinWidth = 200;

                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(2);
                mainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                pgVisibilityBtn.ToolTip = "Collapse Property Grid";
            }
            else
            {
                pgVisibilityBtn.Content = new Image() { Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/UIWidgetEditorPlugin;component/Images/left-arrow-expand.png") as ImageSource };
                
                gridSplitter.Visibility = Visibility.Hidden;
                propertyGrid.Visibility = Visibility.Hidden;

                mainGrid.ColumnDefinitions[2].MinWidth = 0;

                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
                mainGrid.ColumnDefinitions[2].Width = new GridLength(0);

                pgVisibilityBtn.ToolTip = "Expand Property Grid";
            }

            PGVisible = !PGVisible;
        }

        private double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        #region Template Part names

        private const string PART_PropertyGridVisibilityButton = "PART_PropertyGridVisibilityButton";
        private const string PART_GridSplitter = "PART_GridSplitter";
        private const string PART_PropertyGrid = "PART_PropertyGrid";
        private const string PART_MainGrid = "PART_MainGrid";
        private const string PART_WidgetViewBox = "PART_WidgetViewBox";
        private const string PART_WidgetViewGrid = "PART_WidgetViewGrid";

        #endregion

        #region Template Parts

        private Button pgVisibilityBtn;
        private GridSplitter gridSplitter;
        private Grid propertyGrid;
        private Grid mainGrid;
        private Viewbox widgetViewBox;
        private Grid widgetViewGrid;

        #endregion
    }
}
