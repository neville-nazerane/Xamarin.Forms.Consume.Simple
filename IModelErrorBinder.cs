﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Consume.Simple
{
    public interface IModelErrorBinder<TView>
        where TView : View
    {

        TView View { get; set; }

        void Add(string error);

        void Clear();

    }

    public abstract class GenericStackErrorBinder<TLabel, TStackLayout> : IModelErrorBinder<TStackLayout>
        where TStackLayout : StackLayout
        where TLabel : Label, new()
    {
        TStackLayout layout;
        public TStackLayout View { get => layout; set => layout = value; }

        public virtual void Add(string error)
        {
            layout.Children.Add(new TLabel
            {
                TextColor = Color.OrangeRed,
                Text = error
            });
        }

        public virtual void Clear()
        {
            layout.Children.Clear();
        }
    }

    public class StackErrorBinder : GenericStackErrorBinder<Label, StackLayout> { }


}
