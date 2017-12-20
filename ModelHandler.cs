using ConsumeAPI.Simple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Consume.Simple
{
    public class ModelHandler<TModel>
        where TModel : new()
    {

        List<Action<TModel>> Generators = new List<Action<TModel>>();
        List<Action<TModel>> Consumers = new List<Action<TModel>>();

        Dictionary<string, Action<IEnumerable<string>>> ErrorGenerators 
                                = new Dictionary<string, Action<IEnumerable<string>>>();

        List<Action> ClearErrors = new List<Action>();

        TModel _Model;
        public TModel Model {
            get
            {
                if (_Model == null) _Model = new TModel();
                foreach (var gen in Generators)
                    gen(_Model);
                return _Model;
            }
            set
            {
                _Model = value;
                foreach (var con in Consumers)
                    con(value);
            }
        }

        static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        protected void Link<T, TItem, TError, TItemBinder, TErrorBinder>
                    (Expression<Func<TModel, T>> lamda, TItem TextField, TError ErrorList)
            where TItem : View
            where TError : View
            where TItemBinder : IModelItemBinder<TItem, T>, new()
            where TErrorBinder : IModelErrorBinder<TError>, new()
        {
            var itemBinder = new TItemBinder();
            var member = ((MemberExpression)lamda.Body).Member;
            if (TextField != null)
            {
                itemBinder.View = TextField;
                var prop = typeof(TModel).GetRuntimeProperty(member.Name);
                Generators.Add(m => prop.SetValue(m, itemBinder.Value));
                Consumers.Add(m =>
                {
                    var a = prop.GetValue(m);
                    var b = GetDefault(typeof(T));
                    itemBinder.Value = (T)(a ?? b);
                });
            }
            if (ErrorList != null)
            {
                var errorBinder = new TErrorBinder { View = ErrorList };
                ClearErrors.Add(() => {
                    errorBinder.Clear();
                    if (TextField != null)
                    {
                        itemBinder.ClearError();
                    }
                });
                ErrorGenerators.Add(member.Name, errors =>
                {
                    foreach (string error in errors)
                        errorBinder.Add(error);
                    if (TextField != null)
                    {
                        if (errors.Count() > 0)
                            itemBinder.OnError();
                        else
                            itemBinder.ClearError();
                    }
                }
                );
            }
        }

        public virtual void Link<TListModel, TListView>(Expression<Func<TModel, List<TListModel>>> lamda,
                                    TListView listView, StackLayout ErrorList = null)
            where TListModel : new()
            where TListView : View, IBindableListComponent<TListModel>
            => Link<List<TListModel>, TListView, StackLayout, ModelListBinder<TListModel, TListView>, StackErrorBinder>
                        (lamda, listView, ErrorList);

        public virtual void Link<T>(Expression<Func<TModel, T>> lamda, Entry TextField, StackLayout ErrorList = null)
            => Link<T, Entry, StackLayout, ModelEntryBinder<T>, StackErrorBinder>(lamda, TextField, ErrorList);

        public async Task SubmitAsync(Func<TModel, Task<DefaultConsumedResult>> func, Action<string> OnSuccess)
             => SubmitBase(await func(Model), OnSuccess);

        public void Submit(Func<TModel, DefaultConsumedResult> func, Action<string> OnSuccess)
             => SubmitBase(func(Model), OnSuccess);

        public async Task SubmitAsync<TResult>(Func<TModel, Task<ConsumedResult<TResult>>> func, Action<TResult> OnSuccess)
        {
            await DuringAsyncSubmit(async () => SubmitBase(await func(Model), OnSuccess));
        }

        public void Submit<TResult>(Func<TModel, ConsumedResult<TResult>> func, Action<TResult> OnSuccess)
             => SubmitBase(func(Model), OnSuccess);

        void SubmitBase<TResult>(ConsumedResult<TResult> result, Action<TResult> OnSuccess)
        {
            foreach (var clear in ClearErrors)
                clear();
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    OnSuccess(result.Data);
                    break;
                case HttpStatusCode.Unauthorized:
                    OnUnauthorized(result.TextResponse);
                    break;
                case HttpStatusCode.InternalServerError:
                    OnInternalServerError(result.TextResponse);
                    break;
                case HttpStatusCode.Forbidden:
                    OnForbidden(result.TextResponse);
                    break;
                case HttpStatusCode.NotFound:
                    OnNotFound(result.TextResponse);
                    break;
                case HttpStatusCode.BadGateway:
                    OnBadGateway(result.TextResponse);
                    break;
                case HttpStatusCode.BadRequest:
                    foreach (var error in result.Errors)
                    {
                        if (ErrorGenerators.ContainsKey(error.Key))
                        {
                            var gen = ErrorGenerators[error.Key];
                            gen(error.Errors);
                        }
                    }
                    break;
            }
        }

        protected virtual void OnUnauthorized(string response)
        {

        }

        protected virtual void OnForbidden(string response)
        {

        }

        protected virtual void OnInternalServerError(string response)
        {

        }

        protected virtual void OnNotFound(string response)
        {

        }

        protected virtual void OnBadGateway(string response)
        {

        }

        /// <summary>
        /// Runs during the async submission. 
        /// Can be used for procressing animations
        /// </summary>
        /// <param name="submission">To be called when submisison needs to be executed</param>
        /// <returns></returns>
        protected virtual async Task DuringAsyncSubmit(Func<Task> submission)
        {
            await submission();
        }

    }
}
