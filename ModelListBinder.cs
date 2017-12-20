using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Consume.Simple
{

    public class ModelListBinder<TModel, TView>  : IModelItemBinder<TView, List<TModel>>
       where TModel : new()
       where TView : View, IBindableListComponent<TModel>

    {

        List<ModelHandler<TModel>> handlers;

        public ModelListBinder()
        {
            handlers = new List<ModelHandler<TModel>>();
        }

        protected TView Component;
        public TView View {
            get => Component;
            set
            {
                Component = value;
                Component.Added = handler => handlers.Add(handler);
                Component.Removed = handler => handlers.Add(handler);
                Component.Cleared = () => handlers.Clear();
            }
        }


        public List<TModel> Value {
            get
            {
                var list = new List<TModel>();
                foreach (var handler in handlers)
                    list.Add(handler.Model);
                return list;
            }
            set
            {
                Component.Clear();
                foreach (var model in value)
                {
                    var handler = Component.Add();
                    handler.Model = model;
                    handlers.Add(handler);
                }
            }
        }

        public virtual void ClearError()
        {
            Component.BackgroundColor = Color.White;
        }

        public virtual void OnError()
        {
            Component.BackgroundColor = Color.LightPink;
        }
        
    }
}
