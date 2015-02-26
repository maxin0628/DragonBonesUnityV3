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
namespace DragonBones
{
		public class EventData
		{

		public string Z_ORDER_UPDATED = "zorderUpdate";
		public string ANIMATION_FRAME_EVENT = "animationFrameEvent";
		public string BONE_FRAME_EVENT = "boneFrameEvent";
		public string SOUND = "sound";
		public string FADE_IN = "fadeIn";
		public string FADE_OUT = "fadeOut";
		public string START = "start";
		public string COMPLETE = "complete";
		public string LOOP_COMPLETE = "loopComplete";
		public string FADE_IN_COMPLETE = "fadeInComplete";
		public string FADE_OUT_COMPLETE = "fadeOutComplete";
		
		public string _ERROR = "error";

	   private List<EventData> _pool;
		
	
		public	string frameLabel;
		public string sound;
		
		public Armature armature;
		public Bone bone;
		public AnimationState animationState;
		public Frame frame;
		
	private:
			EventType _type;


		public enum  EventType
		{
			Z_ORDER_UPDATED,
			ANIMATION_FRAME_EVENT,
			BONE_FRAME_EVENT,
			SOUND,
			FADE_IN, FADE_OUT, START, COMPLETE, LOOP_COMPLETE, FADE_IN_COMPLETE, FADE_OUT_COMPLETE,
			_ERROR
		};

		public string typeToString(EventData.EventType eventType)
		{
			switch (eventType)
			{
			case EventType.Z_ORDER_UPDATED:
				return Z_ORDER_UPDATED;
				
			case EventType.ANIMATION_FRAME_EVENT:
				return ANIMATION_FRAME_EVENT;
				
			case EventType.BONE_FRAME_EVENT:
				return BONE_FRAME_EVENT;
				
			case EventType.SOUND:
				return SOUND;
				
			case EventType.FADE_IN:
				return FADE_IN;
				
			case EventType.FADE_OUT:
				return FADE_OUT;
				
			case EventType.START:
				return START;
				
			case EventType.COMPLETE:
				return COMPLETE;
				
			case EventType.LOOP_COMPLETE:
				return LOOP_COMPLETE;
				
			case EventType.FADE_IN_COMPLETE:
				return FADE_IN_COMPLETE;
				
			case EventType.FADE_OUT_COMPLETE:
				return FADE_OUT_COMPLETE;
				
			default:
				break;
			}
			
			// throw
			return _ERROR;
		}



		public EventData borrowObject(EventType eventType)
		{
			if (_pool.Count<=0)
			{
				return new EventData(eventType, null);
			}
			
			EventData eventData = _pool[_pool.Count-1];
			eventData._type = eventType;
			_pool.RemoveAt(_pool.Count-1);
			return eventData;
		}
		
		public void returnObject(EventData eventData)
		{

			if (_pool.IndexOf(eventData) < 0 )
			{
				_pool.Add(eventData);
			}

			eventData.clear();
		}
		
		public void clearObjects()
		{
			for (int i = 0; i < _pool.Count;  ++i)
			{
				_pool[i].clear();
				//delete _pool[i];
			}

			_pool.Clear();
		}

		public EventType getType() 
		{
			return _type;
		}
		
		public string getStringType() 
		{
			return typeToString(_type);
		}


		public EventData (EventType type = EventType._ERROR, Armature armatureTarget = null)
		{
			_type = type;
			armature = armatureTarget;
			bone = null;
			animationState = null;
			frame = null;
		}

		public void clear()
		{
			armature = null;
			bone = null;
			animationState = null;
			frame = null;
			
			frameLabel = null;
			sound = null;
		}
		
		public void copy( EventData copyData)
		{
			_type = copyData._type;
			frameLabel = copyData.frameLabel;
			sound = copyData.sound;
			armature = copyData.armature;
			bone = copyData.bone;
			animationState = copyData.animationState;
			frame = copyData.frame;
		}


		}
}

