using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Xamarin.Forms.Consume.Simple
{
    public class ModelHandler<TModel> : ConsumeAPI.UI.Simple.ModelHandler<TModel>
        where TModel : new()
    {

        public void Link<T>(Expression<Func<TModel, T>> lamda, Entry entry, StackLayout layout)
            => Link<T, Entry, StackLayout, ModelEntryBinder<T>, StackErrorBinder>(lamda, entry, layout);

    }
}
