using Frosty.Core;
using Frosty.Core.Controls;
using FrostySdk.Interfaces;
using System.Windows.Media;

namespace UIWidgetEditorPlugin
{
    public class UIWidgetBlueprintAssetDefinition : AssetDefinition
    {
        public override ImageSource GetIcon()
        {
            return new ImageSourceConverter().ConvertFromString("pack://application:,,,/UIWidgetEditorPlugin;component/Images/UIWidgetIcon.png") as ImageSource;
        }

        public override FrostyAssetEditor GetEditor(ILogger logger)
        {
            return new UIWidgetEditor(logger);
        }
    }
}
