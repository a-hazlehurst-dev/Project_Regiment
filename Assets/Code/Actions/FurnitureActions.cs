using System;
using UnityEngine;

public static class FurnitureActions
{
	public static void Door_UpdateAction(Furniture furn, float deltaTime){
		if (furn.GetParameter("is_opening") >= 1) {
			var val = furn.GetParameter ("openness");
			furn.ChangeParameter ("openness" , val+= deltaTime * 2) ;
			if (furn.GetParameter ("openness") >= 1) {
				furn.SetParameter ("is_opening", 0);
			}
		} else {
			var val = furn.GetParameter ("openness");
			furn.ChangeParameter ("openness" , val -= deltaTime * 2) ;
		}

		furn.SetParameter ("openness", Mathf.Clamp01 (furn.GetParameter ("openness")));

        if (furn.cbOnChanged != null)
        {
            furn.cbOnChanged(furn);
        }
	}

	public static Enterability Door_IsEnterable(Furniture furn){
		furn.SetParameter("is_opening", 1);

		if (furn.GetParameter("openness") >= 1) {
			return Enterability.Ok;
		} else {
			return Enterability.Wait;
		}
	}


}

