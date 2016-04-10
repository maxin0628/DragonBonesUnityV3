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
using System.Collections.Generic;
using Com.Viperstudio.Utils;
using DragonBones.Utils;
using DragonBones;
using Com.Viperstudio.Geom;

namespace DragonBones
{
	public class ObjectDataParser
	{

		private delegate Object Function(Dictionary<string, Object> frameObject, uint frameRate);
		
		public static DragonBonesData ParseSkeletonData(Dictionary<string, Object> rawData)
		{
			if(rawData == null)
			{
				throw new ArgumentException();
			}
			
			string version = rawData[ConstValues.A_VERSION].ToString();

            /*
			switch (version)
			{
			case DragonBones.Core.DragonBones.DATA_VERSION:
				break;
			default:
				throw new Exception("Nonsupport version!");
			}
            */
			
			uint frameRate =  uint.Parse(rawData[ConstValues.A_FRAME_RATE].ToString());

            DragonBonesData data = new DragonBonesData();
			data.name = rawData[ConstValues.A_NAME] as String;

			foreach(Dictionary<String, Object> armatureObject in rawData[ConstValues.ARMATURE] as List<Object>)
			{

				data.armatureDataList.Add(parseArmatureData(armatureObject as Dictionary<string, object>, data, frameRate));
			}

			return data;
		}
		
		private static ArmatureData parseArmatureData(Dictionary<String, Object> armatureObject, DragonBonesData data, uint frameRate)
		{
			ArmatureData armatureData = new ArmatureData();
			armatureData.name = armatureObject[ConstValues.A_NAME] as String;

			//Logger.Log("ObjectDataParser::: " + armatureObject[ConstValues.BONE].ToString());
			foreach(Dictionary<String, Object> boneObject in armatureObject[ConstValues.BONE] as List<object>)
			{

				armatureData.boneDataList.Add(parseBoneData(boneObject as Dictionary<string, object>));
			}
			
			foreach(Dictionary<String, Object> skinObject in armatureObject[ConstValues.SKIN] as List<object>)
			{
				armatureData.skinDataList.Add(parseSkinData(skinObject as Dictionary<string, object>, data));
			}
			
			DBDataUtil.TransformArmatureData(armatureData);
			armatureData.sortBoneDataList();
			
			foreach(Dictionary<String, Object> animationObject in armatureObject[ConstValues.ANIMATION] as List<object>)
			{
				armatureData.animationDataList.Add(parseAnimationData(animationObject as Dictionary<string, object>, armatureData, frameRate));
			}
			
			return armatureData;
		}
		
		private static BoneData parseBoneData(Dictionary<String, Object> boneObject)
		{
			BoneData boneData = new BoneData();
			boneData.name = boneObject[ConstValues.A_NAME] as String;
			if (boneObject.ContainsKey (ConstValues.A_PARENT)) {
				boneData.parent = boneObject [ConstValues.A_PARENT] as string;
			}
			if (boneObject.ContainsKey (ConstValues.A_LENGTH)) {
				boneData.length = boneObject [ConstValues.A_LENGTH] == null ? 0 : (int)boneObject [ConstValues.A_LENGTH];
			}


			if (boneObject.ContainsKey (ConstValues.A_INHERIT_SCALE)) {
				Object scaleModeObj = boneObject[ConstValues.A_INHERIT_SCALE];
			   if (scaleModeObj != null) 
				{
					bool scaleMode = bool.Parse(scaleModeObj.ToString());
					boneData.inheritScale = scaleMode;
				}
			}
			if(boneObject.ContainsKey (ConstValues.A_FIXED_ROTATION))
			{
		     	bool inheritRotation = bool.Parse(boneObject[ConstValues.A_FIXED_ROTATION].ToString ());
			    if (inheritRotation)
			     {
				   boneData.inheritRotation = inheritRotation;
			     }
			}
			parseTransform(boneObject[ConstValues.TRANSFORM] as Dictionary<string, object>, boneData.global);
			boneData.transform.Copy(boneData.global);

			//Logger.Log (boneData.name + " " +  boneData.transform.X + " " + boneData.transform.Y);
			
			return boneData;
		}
		
		private static SkinData parseSkinData(Dictionary<String, Object> skinObject, DragonBonesData data)
		{
			SkinData skinData = new SkinData();
			skinData.name = skinObject[ConstValues.A_NAME] as String;
			
			foreach(Dictionary<String, Object> slotObject in skinObject[ConstValues.SLOT] as List<object>)
			{
				skinData.slotDataList.Add(parseSlotData(slotObject as Dictionary<string, object>, data));
			}
			
			return skinData;
		}
		
		private static SlotData parseSlotData(Dictionary<String, Object> slotObject, DragonBonesData data)
		{
			SlotData slotData = new SlotData();
			slotData.name = slotObject[ConstValues.A_NAME] as String;
			slotData.parent = slotObject[ConstValues.A_PARENT] as String;
			slotData.zOrder = (float)slotObject[ConstValues.A_Z_ORDER];

			if (slotObject.ContainsKey (ConstValues.A_BLENDMODE)) {
			  
			  if (slotObject [ConstValues.A_BLENDMODE] == null) 
			  {
				slotData.blendMode = DragonBones.getBlendModeByString("normal");
			  }
			  else
			  {
				slotData.blendMode = DragonBones.getBlendModeByString(slotObject [ConstValues.A_BLENDMODE].ToString());
			  }
			}
			foreach(Dictionary<String, Object> displayObject in slotObject[ConstValues.DISPLAY] as List<object>)
			{
				slotData.displayDataList.Add(parseDisplayData(displayObject as Dictionary<string, object>, data));
			}
			
			return slotData;
		}
		
		private static DisplayData parseDisplayData(Dictionary<String, Object> displayObject, DragonBonesData data)
		{
			DisplayData displayData = new DisplayData();
			displayData.name = displayObject[ConstValues.A_NAME] as String;
			displayData.type = DragonBones.getDisplayTypeByString( displayObject[ConstValues.A_TYPE] as String);

            displayData.pivot = new Point();
                /*
                data.addSubTexturePivot(
				0, 
				0, 
				displayData.name
				);
			    */
			parseTransform(displayObject[ConstValues.TRANSFORM] as Dictionary<String, object>, displayData.transform, displayData.pivot);
			
			return displayData;
		}
		
		private static AnimationData parseAnimationData(Dictionary<String, Object> animationObject, ArmatureData armatureData, uint frameRate)
		{
			AnimationData animationData = new AnimationData();
			animationData.name = animationObject[ConstValues.A_NAME] as String;
			animationData.frameRate = (int)frameRate;

            animationData.playTimes = int.Parse(animationObject[ConstValues.A_LOOP].ToString());
			animationData.fadeTime = (float)animationObject[ConstValues.A_FADE_IN_TIME];
			animationData.duration = (int)((float)animationObject [ConstValues.A_DURATION] * 1000.0f /frameRate);
			animationData.scale = (float)animationObject[ConstValues.A_SCALE];
			
			if(animationObject.ContainsKey(ConstValues.A_TWEEN_EASING))
			{
				Object tweenEase = animationObject[ConstValues.A_TWEEN_EASING];
				if(
					tweenEase == null
					)
				{
					animationData.tweenEasing = float.NaN;
				}
				else
				{
					animationData.tweenEasing = (float)tweenEase;
				}
			}
			else
			{
				animationData.tweenEasing = float.NaN;
			}
			
			parseTimeline(animationObject as Dictionary<string, object>, animationData, parseMainFrame, frameRate);
			
			TransformTimeline timeline;
			string timelineName;
			foreach(Dictionary<String, Object> timelineObject in animationObject[ConstValues.TIMELINE] as List<object>)
			{
				timeline = parseTransformTimeline(timelineObject as Dictionary<string, object>, animationData.duration, frameRate);
				timelineName = (timelineObject as Dictionary<string, object>)[ConstValues.A_NAME] as String;
				animationData.AddTimeline(timeline, timelineName);
			}
			
			DBDataUtil.AddHideTimeline(animationData, armatureData);
			DBDataUtil.TransformAnimationData(animationData, armatureData);
			
			return animationData;
		}
		
		private static void parseTimeline(Dictionary<String, Object> timelineObject, Timeline timeline, Function frameParser, uint frameRate)
		{

			if(timelineObject.ContainsKey(ConstValues.FRAME))
			{
				int position = 0;
				Frame frame = null;

				foreach(Dictionary<String, Object> frameObject in timelineObject[ConstValues.FRAME] as List<object>)
				{
					frame = frameParser(frameObject as Dictionary<string, object>, frameRate) as Frame;
					frame.position = position;
					timeline.frameList.Add(frame);
					position += frame.duration;
				}
				if(frame!=null)
				{
					frame.duration = timeline.duration - frame.position;
				}

			}

		
		}
		
		private static TransformTimeline parseTransformTimeline(Dictionary<String, Object> timelineObject, int duration, uint frameRate)
		{
			TransformTimeline timeline = new TransformTimeline();
			timeline.duration = duration;
			
			parseTimeline(timelineObject, timeline, parseTransformFrame, frameRate);
			
			timeline.scale = (float)timelineObject[ConstValues.A_SCALE];
            timeline.offset = (float)timelineObject[ConstValues.A_OFFSET];
			
			return timeline;
		}
		
		private static void parseFrame(Dictionary<String, Object> frameObject, Frame frame, uint frameRate)
		{
			frame.duration = (int)((float)frameObject[ConstValues.A_DURATION] * 1000.0f / frameRate);
			if(frameObject.ContainsKey(ConstValues.A_ACTION))
			    frame.action = frameObject[ConstValues.A_ACTION] as String;
			if(frameObject.ContainsKey(ConstValues.A_EVENT))
			    frame.evt = frameObject[ConstValues.A_EVENT] as String;
			if(frameObject.ContainsKey(ConstValues.A_SOUND))
			    frame.sound = frameObject[ConstValues.A_SOUND] as String;
		}
		
		private static Frame parseMainFrame(Dictionary<String, Object> frameObject, uint frameRate)
		{
			Frame frame = new Frame();
			parseFrame(frameObject, frame, frameRate);
			return frame;
		}
		
		private static TransformFrame parseTransformFrame(Dictionary<String, Object> frameObject, uint frameRate)
		{
			TransformFrame frame = new TransformFrame();
			parseFrame(frameObject, frame, frameRate);

			if(frameObject.ContainsKey(ConstValues.A_HIDE))
			{
				frame.visible = (uint)frameObject[ConstValues.A_HIDE] != 1;
			}
			else
			{
				frame.visible = true;
			}

			
			if(frameObject.ContainsKey(ConstValues.A_TWEEN_EASING ))
			{
				Object tweenEase = frameObject[ConstValues.A_TWEEN_EASING];
				if(
					tweenEase == null
					)
				{
					frame.tweenEasing = float.NaN;
				}
				else
				{
					frame.tweenEasing = (float)tweenEase;
				}
			}
			else
			{
				frame.tweenEasing = 0f;
			}

			if(frameObject.ContainsKey(ConstValues.A_TWEEN_ROTATE))
			{
				frame.tweenRotate = (int)frameObject[ConstValues.A_TWEEN_ROTATE];
			}
			   
			if(frameObject.ContainsKey(ConstValues.A_DISPLAY_INDEX))
			{

				frame.displayIndex = int.Parse(frameObject[ConstValues.A_DISPLAY_INDEX].ToString());

			}
			     


			if(frameObject.ContainsKey(ConstValues.A_Z_ORDER))
			     frame.zOrder = (float)frameObject[ConstValues.A_Z_ORDER];
			if(frameObject.ContainsKey(ConstValues.TRANSFORM))
				 parseTransform(frameObject[ConstValues.TRANSFORM] as Dictionary<string, object>, frame.global, frame.pivot);

			frame.transform.Copy(frame.global);
			Dictionary<String, Object> colorTransformObject = null;
			if(frameObject.ContainsKey(ConstValues.COLOR_TRANSFORM))
			   colorTransformObject = frameObject[ConstValues.COLOR_TRANSFORM] as Dictionary<string, object>;

			if(colorTransformObject!=null)
			{
				frame.color = new ColorTransform();
				frame.color.AlphaOffset = (float)colorTransformObject[ConstValues.A_ALPHA_OFFSET];
				frame.color.RedOffset = (float)colorTransformObject[ConstValues.A_RED_OFFSET];
				frame.color.GreenOffset = (float)colorTransformObject[ConstValues.A_GREEN_OFFSET];
				frame.color.BlueOffset = (float)colorTransformObject[ConstValues.A_BLUE_OFFSET];

				frame.color.AlphaMultiplier = (float)colorTransformObject[ConstValues.A_ALPHA_MULTIPLIER] * 0.01f;
				frame.color.RedMultiplier = (float)colorTransformObject[ConstValues.A_RED_MULTIPLIER] * 0.01f;
				frame.color.GreenMultiplier = (float)colorTransformObject[ConstValues.A_GREEN_MULTIPLIER] * 0.01f;
				frame.color.BlueMultiplier = (float)colorTransformObject[ConstValues.A_BLUE_MULTIPLIER] * 0.01f;
			}
			
			return frame;
		}
		
		private static void parseTransform(Dictionary<String, Object> transformObject, DBTransform transform, Point pivot = null)
		{
			if(transformObject!=null)
			{
				if(transform!=null)
				{
					transform.X = (float)transformObject[ConstValues.A_X];
					transform.Y = (float)transformObject[ConstValues.A_Y];
					transform.SkewX = (float)transformObject[ConstValues.A_SKEW_X] * ConstValues.ANGLE_TO_RADIAN;
					transform.SkewY = (float)transformObject[ConstValues.A_SKEW_Y] * ConstValues.ANGLE_TO_RADIAN;
					transform.ScaleX = (float)transformObject[ConstValues.A_SCALE_X];
					transform.ScaleY = (float)transformObject[ConstValues.A_SCALE_Y];

				}
				if(pivot!=null)
				{
					pivot.X = (float)transformObject[ConstValues.A_PIVOT_X];
					pivot.Y = (float)transformObject[ConstValues.A_PIVOT_Y];
				}
			}
		}
	}
}

