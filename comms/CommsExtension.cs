using UnityEngine;
using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Framework.API;

namespace comms
{
	/// <summary>
	/// <p>This Extension waits for a ContextView to be added as a configuration
	/// and maps it into the context's injector.</p>
	///
	/// <p>It should be installed before context initialization.</p>
	/// </summary>
	public class CommsExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;
		
		
        //[Inject] public IMediatorMap mediatorMap;
        
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			_injector = context.injector;

            context.AfterInitializing(AfterInitializing);
		}
        
		private void AfterInitializing()
		{
            Transform contextViewTransform = _injector.GetInstance<Transform>();
            Comms.initialize(contextViewTransform);
		}
	}
}

