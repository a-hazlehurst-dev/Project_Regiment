function Clamp01(value)
	if(value >1 ) then
		return 1
	elseif (value < 0) then
		return 0
	end
	return value

end


function OnUpdate_Smelter(furniture, deltaTime)

	if(furniture.tile.room ==nil) then
		return "furniture's room was null"
	end

	if (furniture.Tile.Room.GetEnviromenntAmount("temperature")  <1.2) then
		furniture.Tile.Room.ChangeEnvironment ("temperature", 0.1 * deltaTime)
	end
end


function OnUpdate_Door(furniture, deltaTime)



	if (furniture.GetParameter("is_opening") >= 1.0) then
		
	    furniture.ChangeParameter ("openness" , deltaTime * 2)  --FIXME: param for opening speed.

		if (furniture.GetParameter ("openness") >= 1) then
			furniture.SetParameter ("is_opening", 0)
		end
		
	else 
		furniture.ChangeParameter ("openness" , deltaTime * -2) 
	end
		
	furniture.SetParameter ("openness", Clamp01(furniture.GetParameter ("openness")))

	if (furniture.cbOnChanged != nil) then

        furniture.cbOnChanged(furniture)

	end


	return furniture.GetParameter ("openness") 
end


function IsEnterable_Door(furniture)
	furniture.SetParameter("is_opening", 1)
	
	if(furniture.GetParameter("openness")>=1.0) then

		return 0 --Enterability.OK

	end

	return 2 -- enterability.Wait
	

end


--[[

function OnUpdate_Stockpile( furniture, deltaTime )
	if(true) then
		return 
	end

	if (furniture.Tile.inventory != nil and furniture.Tile.inventory.StackSize >= furniture.Tile.inventory.maxStackSize)    then
    
        furniture.CancelJobs();
        return;
	end

	return;

	if (furniture.JobCount() > 0) then
		-- already have a job.
       return;
	end


    itemsDesired = {}
  
	if (furniture.Tile.inventory == nil) then

        itemsDesired = Stockpile_GetItemsFromFilter()
	else

        desInv = furniture.Tile.inventory.Clone()

        desInv.maxStackSize -= desiredInv.StackSize
        desInv.StackSize = 0
        itemsDesired = { desInv }
	end



	j = Job.__new(furniture.Tile, nil,nil,0,itemsDesired, false)

	j.CanTakeFromStockpile = false
	j.furnitureToOperate = furniture
	j.Register_JobWorked_Callback("Stockpile_JobWorked")
	furniture.AddJob(j)
end

function  Stockpile_GetItemsFromFilter() 

	-- should be removed from lua. instead call c# to get the list.
	return Inventory.__new("clay", 50, 0) 
end

function Stockpile_JobWorked(job)	
	job.CancelJobs()

	for k,inv in pairs (job._inventoryRequirements) do
		if (inv.StackSize > 0) then
			GameManager.Instance.InventoryService.PlaceInventory(job.Tile, inv)
			return
		end
	end

end

--]]

