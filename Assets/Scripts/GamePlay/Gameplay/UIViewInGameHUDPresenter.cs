using MobaPrototype.UIViewImplementation;
using VContainer.Unity;

namespace MobaPrototype
{
    public class UIViewInGameHUDPresenter : IInitializable
    {
        private UIViewDebugSideBar uiViewDebugSideBar;

        public UIViewInGameHUDPresenter(UIViewDebugSideBar uiViewDebugSideBar)
        {
            this.uiViewDebugSideBar = uiViewDebugSideBar;
        }

        public void Initialize()
        {
        }
    }
}