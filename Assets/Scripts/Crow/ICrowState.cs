using UnityEngine;
namespace StatePattern
{
    public interface ICrowState
    {
        void EnterState(CrowController crowController);
        void UpdateState();
        void FixedUpdateState();
    }
}
