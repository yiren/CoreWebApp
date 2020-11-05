using Fluxor;

namespace BlazorWasm.store
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