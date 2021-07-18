using System;
using System.Collections.Generic;
using UnityEngine;

public class SoftwareProducts : MonoBehaviour
{
	public SoftwareProductDefinition Get(SOFTWARE_PRODUCTS ProductToGet)
	{
		if (this.productLookUp.ContainsKey(ProductToGet))
		{
			return this.Products[this.productLookUp[ProductToGet]];
		}
		return null;
	}

	private void Awake()
	{
		LookUp.SoftwareProducts = this;
		for (int i = 0; i < this.Products.Count; i++)
		{
			SoftwareProductDefinition softwareProductDefinition = this.Products[i];
			if (!this.productLookUp.ContainsKey(softwareProductDefinition.Product))
			{
				this.productLookUp.Add(softwareProductDefinition.Product, i);
			}
		}
	}

	public List<SoftwareProductDefinition> Products;

	private Dictionary<SOFTWARE_PRODUCTS, int> productLookUp = new Dictionary<SOFTWARE_PRODUCTS, int>();
}
