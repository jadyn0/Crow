using UnityEngine;
namespace StatePattern
{
    public interface ICrowState
    {
        void EnterState(CrowController crowController);
        void UpdateState();
        void FixedUpdateState();
        void Collision(Collision2D collision);
    }
}
