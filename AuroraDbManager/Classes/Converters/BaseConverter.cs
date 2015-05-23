namespace AuroraDbManager.Classes.Converters {
    using System;
    using System.Windows.Markup;

    public abstract class BaseConverter: MarkupExtension {
        public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
    }
}