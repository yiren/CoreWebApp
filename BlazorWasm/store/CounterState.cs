using Fluxor;
namespace BlazorWasm.Store
{
    public record CounterState
    {
        public int Count { get; init; }
    }

    public class CounterFeatureState : Feature<CounterState>
    {
        public override string GetName()=> "[Blazor]Counter";

        protected override CounterState GetInitialState()
        {
            return new CounterState(){
                Count=0
            };
        }        
    } 
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit{}
}