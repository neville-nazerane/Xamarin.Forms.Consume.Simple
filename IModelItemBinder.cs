using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Converter = System.ComponentModel.TypeConverter;

namespace Xamarin.Forms.Consume.Simple
{
    public interface IModelItemBinder<TView, TData>
    {

        TView View { get; set; }

        TData Value { get; set; }

        void OnError();

        void ClearError();

    }

    public abstract class GenericModelEntryBinder<TEntry, TData> : IModelItemBinder<TEntry, TData>
        where TEntry : Entry
    {

        private TEntry _View;
        public virtual TEntry View { get => _View; set => _View = value; }

        public virtual TData Value {
            get
            {
                var converter = TypeDescriptor.GetConverter(typeof(TData));
                return (TData) converter.ConvertFromString(View.Text);
            }
            set
            {
                if (View != null && value != null)
                    View.Text = value.ToString();
            }
        }

        private Type _ItemDataType;
        Converter _TypeConverter;
        public Type ItemDataType
        {
            get => _ItemDataType;
            set {
                _TypeConverter = TypeDescriptor.GetConverter(value);
                _ItemDataType = value;
            }
        }


        public virtual void ClearError()
        {
            View.BackgroundColor = Color.White;
        }

        public virtual void OnError()
        {
            View.BackgroundColor = Color.LightPink;
        }

        
    }

    public class ModelEntryBinder<T> : GenericModelEntryBinder<Entry, T> { }

   

}
