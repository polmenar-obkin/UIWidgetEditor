using Frosty.Core;
using Frosty.Core.Controls;
using FrostySdk.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UIWidgetEditorPlugin
{
    [TemplatePart(Name = PART_PropertyGridVisibilityButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_GridSplitter, Type = typeof(GridSplitter))]
    [TemplatePart(Name = PART_PropertyGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_MainGrid, Type = typeof(Grid))]
    public class UIWidgetEditor : FrostyAssetEditor
    {
        public bool PGVisible { get; set; } = false;

        public UIWidgetEditor(ILogger inLogger) : base(inLogger)
        {
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

            // collapse gridsplitter and property grid
            mainGrid.ColumnDefinitions[2].MinWidth = 0;
            mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
            mainGrid.ColumnDefinitions[2].Width = new GridLength(0);

            pgVisibilityBtn.Click += PGVisibilityBtn_Click;

            App.Logger.Log("Widget view loaded.");
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

        #region Template Part names

        private const string PART_PropertyGridVisibilityButton = "PART_PropertyGridVisibilityButton";
        private const string PART_GridSplitter = "PART_GridSplitter";
        private const string PART_PropertyGrid = "PART_PropertyGrid";
        private const string PART_MainGrid = "PART_MainGrid";

        #endregion

        #region Template Parts

        private Button pgVisibilityBtn;
        private GridSplitter gridSplitter;
        private Grid propertyGrid;
        private Grid mainGrid;

        #endregion
    }
}
