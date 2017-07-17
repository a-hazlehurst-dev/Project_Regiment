﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	public class InventorySpriteController : MonoBehaviour
	{
		Dictionary<Inventory, GameObject> _inventoryGameObjectMapper;
		SpriteManager _spriteManager;
		InventoryService _inventoryService;

		public GameObject inv_ui_prefab;

		Transform inventoryHolder;

		// Use this for initialization
		void Start () {

		}


		public void Init(SpriteManager spriteManager, InventoryService inventoryService)
		{
			_spriteManager = spriteManager;

			_inventoryService = inventoryService;

			_inventoryGameObjectMapper = new Dictionary<Inventory, GameObject>();

			inventoryHolder = new GameObject("InventoryHolder").transform;

			_inventoryService.Register_OnInventory_Created(OnInventoryCreated);

			foreach (var key in GameManager.Instance.GetInventories().Keys) {
			foreach (var item in GameManager.Instance.GetInventories()[key]) {
					OnInventoryCreated (item);
				}

			}
		}

		public void OnInventoryCreated(Inventory inventory){

		Debug.Log ("Created " + inventory.objectType);
			GameObject inv_go = new GameObject ();

			_inventoryGameObjectMapper.Add (inventory, inv_go);

			inv_go.name = inventory.objectType;
			inv_go.transform.position = new Vector3 (inventory.Tile.X, inventory.Tile.Y, 0);
			var sr = inv_go.AddComponent<SpriteRenderer> ();
			sr.sprite = _spriteManager.InventoryObjects [inventory.objectType];
			sr.sortingLayerName = "Furniture";

			if (inventory.maxStackSize > 1) {	
				GameObject ui_go = Instantiate (inv_ui_prefab);
			    ui_go.transform.SetParent (inv_go.transform);
			    ui_go.transform.localPosition = Vector3.zero; //TODO: if sprite anchor gets changed we'll have to changes ti.

			    ui_go.GetComponentInChildren<Text>().text = inventory.stackSize.ToString();
			}

			inv_go.transform.SetParent ( inventoryHolder );


			//FIX ME ADD ON CHANGED CALL BACK
			//	inventory.RegisterOnCharacterChangedCallback (OnInventoryChanged);
		}

		public void OnInventoryChanged(Inventory inventory){
			if (!_inventoryGameObjectMapper.ContainsKey (inventory)) {
				Debug.Log ("OnCharacterChanged: cannot find character.");
				return;
			}

			GameObject inv_go = _inventoryGameObjectMapper [inventory];

			inv_go.transform.position = new Vector3 (inventory.Tile.X, inventory.Tile.Y, 0);
			inv_go.transform.SetParent ( inventoryHolder );
		}
	}

