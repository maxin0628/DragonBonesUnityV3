// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using Com.Viperstudio.Geom;
using System.Collections.Generic;
namespace DragonBones
{
		public class DragonBonesData
		{

	  
	
		public bool autoSearch;
		public string name;
		public List<ArmatureData> armatureDataList;
				public DragonBonesData ()
				{
				}

		ArmatureData getArmatureData(string armatureName) 
		{
			for (int i = 0; i < armatureDataList.Count; ++i)
			{
				if (armatureDataList[i].name == armatureName)
				{
					return armatureDataList[i];
				}
			}
			
			return null;
		}

		public void dispose()
		{
			for (int i = 0; i < armatureDataList.Count; ++i)
			{
				armatureDataList[i].dispose();
				//delete armatureDataList[i];
			}
			
			armatureDataList.Clear();
		}


	}
}

