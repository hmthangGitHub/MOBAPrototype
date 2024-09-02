using System;
using System.Collections;
using System.Collections.Generic;
using UIView;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewInGameHUD : UIViewBase<UIViewInGameHUD.UIModel>
    {
        [Serializable]
        public class UIModel
        {
        }

        protected override void OnSetModel(UIModel model)
        {
        }
    }
}