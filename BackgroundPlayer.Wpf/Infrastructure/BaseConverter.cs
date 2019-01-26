﻿using System;
using System.Windows.Markup;

namespace BackgroundPlayer.Wpf.Infrastructure
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
