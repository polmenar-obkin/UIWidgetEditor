using Frosty.Core.Controls;
using FrostySdk.Interfaces;
using System.Windows;

namespace UIWidgetEditorPlugin
{
    public class UIWidgetEditor : FrostyAssetEditor
    {
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
        }
    }
}
