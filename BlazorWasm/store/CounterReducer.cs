using Fluxor;

namespace BlazorWasm.Store
{
    public static class CounterReducer
    {
        [ReducerMethod]
        public static CounterState OnAddCounter(CounterState state, AddCounter action)
        {
            return state with
            {
                Count =state.Count+1
            };
        }

    }
}