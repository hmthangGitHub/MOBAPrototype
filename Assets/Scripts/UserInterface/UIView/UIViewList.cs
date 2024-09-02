using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIView
{
    public class UIViewList<T, TModel> : UIViewBase<ReactiveCollection<TModel>> where T : UIViewBase<TModel>
    {
        [SerializeField] private T template;
        [SerializeField] private List<T> instanceList;
        
        protected override void OnSetModel(ReactiveCollection<TModel> models)
        {
            template.gameObject.SetActive(false);
            OnReset();

            foreach (var model in models)
            {
                instanceList.Add(CreateInstanceAndSetModel(model));
            }

            models.ObserveAdd()
                .Subscribe(x =>
                {
                    instanceList.Insert(x.Index, CreateInstanceAndSetModel(x.Value));
                }).AddTo(disposables);
            
            models.ObserveRemove()
                .Subscribe(x =>
                {
                    instanceList.RemoveAt(x.Index);
                }).AddTo(disposables);
            
            models.ObserveReset()
                .Subscribe(_ =>
                {
                    OnReset();
                }).AddTo(disposables);
            
            models.ObserveReplace()
                .Subscribe(x =>
                {
                    instanceList[x.Index] = CreateInstanceAndSetModel(x.NewValue);
                }).AddTo(disposables);
        }

        private T CreateInstanceAndSetModel(TModel model)
        {
            var instance = Instantiate<T>(template, transform);
            instance.SetModel(model);
            instance.gameObject.SetActive(true);
            return instance;
        }

        private void OnReset()
        {
            transform.Cast<Transform>().Where(x => x != template.transform)
                .ToList()
                .ForEach(x => Destroy(x.gameObject));

            instanceList.Clear();
        }
    }
}