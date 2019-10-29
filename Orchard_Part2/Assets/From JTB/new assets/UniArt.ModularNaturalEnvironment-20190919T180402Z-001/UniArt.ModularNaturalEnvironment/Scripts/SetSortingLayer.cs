using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniArt.ModularPaintedEnvironnement
{
	[AddComponentMenu("UniArt/ModularNaturalEnvironment/Scripts/SetSortingLayer")]
	[ExecuteInEditMode()]

	public class SetSortingLayer : MonoBehaviour 
	{	
		public int orderInLayer;
		public string sortingLayerName = "Default";
		
		private void Update()
		{
			if(GetComponent<Renderer>() != null)
			{
				GetComponent<Renderer>().sortingOrder = orderInLayer;
				GetComponent<Renderer>().sortingLayerName = sortingLayerName;
			}	
		}
	}
}