using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Consume.Simple
{
    public interface IBindableListComponent<TModel>
        where TModel : new()
    {

        Action<ModelHandler<TModel>> Added { get; set; }
        
        Action<ModelHandler<TModel>> Removed { get; set; }

        Action Cleared { get; set; }

        ModelHandler<TModel> Add();

        void Clear();

    }
}
