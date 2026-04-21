using Frosty.Core;
using FrostySdk.IO;
using FrostySdk.Managers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UIWidgetEditorPlugin.Models
{
    public class WidgetEditor
    {
        public dynamic UIWidgetSize { get; set; }

        public void GetWidgetSize()
        {
            // get opened asset object
            EbxAsset asset = App.AssetManager.GetEbx(App.EditorWindow.GetOpenedAssetEntry() as EbxAssetEntry);

            if (asset == null)
            {
                App.Logger.LogError("Could not get opened asset!");
                return;
            }

            UIWidgetSize = ((dynamic)asset.RootObject).Object.Internal.Size;

            App.Logger.Log($"Widget length = {UIWidgetSize.X}; height = {UIWidgetSize.Y}");
        }

        public void RenderSizeOnView(Viewbox widgetViewBox)
        {
            Rectangle rectangle = new Rectangle()
            {
                Width = UIWidgetSize.X,
                Height = UIWidgetSize.Y,
                StrokeThickness = 2d,
                Stroke = Brushes.White,
                Fill = Brushes.Transparent
            };

            widgetViewBox.Child = rectangle;
        }
    }
}
