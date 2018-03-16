using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Services.Interfaces
{
    public interface IDeferredActionService
    {
        /**********************************************************************/
        #region Methods

        void AddAction(Action action);

        void AddAction<TService>(Action<TService> action);

        void AddAction<TService1, TService2>(Action<TService1, TService2> action);

        void AddAction(Func<Task> action);

        void AddAction<TService>(Func<TService, Task> action);

        void AddAction<TService1, TService2>(Func<TService1, TService2, Task> action);

        #endregion Methods
    }
}
