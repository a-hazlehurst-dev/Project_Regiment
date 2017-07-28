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

	furniture.SetParameter ("is_opening", 1);

	if (furniture.GetParameter("is_opening") >= 1) then

		furniture.ChangeParameter ("openness" , deltaTime * 2)  --FIXME: param for opening speed.

		if (furniture.GetParameter ("openness") >= 1) then

			furniture.SetParameter ("is_opening", 0);
		
		else 
			furniture.ChangeParameter ("openness" , deltaTime * -2) ;
		end
	end
		
	furniture.SetParameter ("openness", Clamp01(furniture.GetParameter ("openness")));

	if (furniture.cbOnChanged != nil) then

        furniture.cbOnChanged(furniture);

	end

end



function IsEnterable_Door(furniture)
	furniture.SetParameter("is_opening", 1)

	if(furniture.GetParameter("openness")>=1) then

		return Enterability.Ok

	end

	return Enterability.Wait

end
