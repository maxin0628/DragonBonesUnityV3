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
using Com.Viperstudio.Events;
using Com.Viperstudio.Geom;

namespace DragonBones
{



	public class Armature : IAnimatable
	{

	
		public 		static IEventDispatcher *soundEventDispatcher;

	
		public	string name;
		
		public object userData;
		
	
		protected 	bool _needUpdate;
		protected bool _slotsZOrderChanged;
		protected bool _delayDispose;
		protected bool _lockDispose;
		
		protected List<Bone> _boneList;
		protected List<Slot> _slotList;
		protected List<EventData> _eventDataList;
		
		protected ArmatureData *_armatureData;
		protected Animation *_animation;
		protected IEventDispatcher *_eventDispatcher;
		protected object _display;



		    public bool autoTween;

		public Armature (ArmatureData armatureData, Animation animation, EventDispatcher eventDispatcher, object display)
		{

			_armatureData = armatureData;
			_animation = animation;
			_eventDispatcher = eventDispatcher;
			_display = display;
			_animation._armature = this;
			_needUpdate = false;
			_slotsZOrderChanged = false;
			_delayDispose = false;
			_lockDispose = false;
			userData = null;

		}

	
		private	static bool sortBone(KeyValuePair<int, Bone> a, KeyValuePair<int, Bone> b)
		{
			if (a.Key < b.Key)
						return -1;
			if (a.Key == b.Key)
						return 0;
			if (a.Key > b.Key)
						return 1;

	    }
		private static bool sortSlot(Slot a,  Slot b)
		{
			if (a.getZOrder() < b.getZOrder())
			    return -1;
			if (a.getZOrder() == b.getZOrder())
			    return 0;
			if (a.getZOrder() > b.getZOrder())
			    return 1;
			
		}
		
	
		public 	virtual Rectangle getBoundingBox()
	    {
				return null;
	    }
		
		public virtual List<Bone> getBones() 
	    {
				return _boneList;
		}
		public virtual List<Slot> getSlots() 
	    {
				return _slotList;
		}
		
		public virtual  ArmatureData getArmatureData()
	    {
				return _armatureData;
		}
		public virtual Animation getAnimation()
	    {
				return _animation;
		}
		public virtual void getDisplay()
		{
				return _display;
		}
		public virtual EventDispatcher getEventDispatcher()
		{
				return _eventDispatcher;
		}
		
		
		
	
	    public	virtual Bone getBone(string boneName) {
				if (boneName.Count<=0)
				{
					// throw
				}
				
				for (int i = 0; i< _boneList.Count; ++i)
				{
					if (_boneList[i].name == boneName)
					{
						return _boneList[i];
					}
				}
				
				return null;
		}
		public virtual Bone getBoneByDisplay(object display) {
				if (!display)
				{
					// throw
				}
				
				Slot slot = getSlotByDisplay(display);
				return slot ? slot._parent : null;
		      	
		}
		public virtual void addBone(Bone bone){
				if (!bone)
				{
					// throw
				}
				
				if (bone._parent)
				{
					bone._parent.removeChild(bone);
				}
				
				bone.setArmature(this);

		}
		public virtual void addBone(Bone bone, string parentBoneName){
				if (parentBoneName.Length<=0)
				{
					// throw
				}
				
				Bone boneParent = getBone(parentBoneName);
				
				if (!boneParent)
				{
					// throw
				}
				
				boneParent.addChild(bone);

	    }
		public virtual void removeBone(Bone bone){
				if (!bone || bone._armature != this)
				{
					// throw
				}
				
				if (bone._parent)
				{
					bone._parent.removeChild(bone);
				}
				else
				{
					bone.setArmature(null);
				}

		}
		public virtual Bone removeBone(string boneName){
				if (boneName.Length<=0)
				{
					// throw
				}
				
				Bone bone = getBone(boneName);
				
				if (bone)
				{
					removeBone(bone);
				}
				
				return bone;

		}
		
		public virtual Slot getSlot(string slotName){
				if (slotName.Length<=0)
				{
					// throw
				}
				
				for (int i = 0; i < _slotList.Count;  ++i)
				{
					if (_slotList[i].name == slotName)
					{
						return _slotList[i];
					}
				}
				
				return null;

		}
		public virtual Slot getSlotByDisplay(object display){
				
				if (!display)
				{
					// throw
				}
				
				for (int i = 0; l < _slotList.Count;  ++i)
				{
					if (_slotList[i]._display == display)
					{
						return _slotList[i];
					}
				}
				
				return null;

		}
		public virtual void addSlot(Slot slot, string parentBoneName){
				Bone bone = getBone(parentBoneName);
				
				if (!bone)
				{
					// throw
				}
				
				bone.addChild(slot);

		}
		public virtual void removeSlot(Slot slot){
				if (!slot || slot._armature != this)
				{
					// throw
				}
				
				slot._parent.removeChild(slot);
		}
		public virtual Slot removeSlot(string slotName){
				Slot slot = getSlot(slotName);
				
				if (slot)
				{
					removeSlot(slot);
				}
				
				return slot;

		}
		public virtual void replaceSlot(string boneName, string oldSlotName, Slot newSlot){
				Bone bone = getBone(boneName);
				if (!bone) return;
				
				List<Slot> slots = bone.getSlots();


				Slot it = slots.Find(
					delegate(Slot tmp)  
				                     {  
					return oldSlotName == tmp.name;
				});  

				if (it != slots[slots.Count-1])
				{
					Slot oldSlog = it;
					newSlot._tweenZOrder = oldSlog._tweenZOrder;
					newSlot._originZOrder = oldSlog._originZOrder;
					newSlot._offsetZOrder = oldSlog._offsetZOrder;
					removeSlot(oldSlog);
				}
				
				newSlot.name = oldSlotName;
				bone.addChild(newSlot);

		}
		public virtual void sortSlotsByZOrder(){
				_slotList.Sort(sortSlot);
				
				for (int i = 0; i<_slotList.Count; ++i)
				{
					Slot slot = _slotList[i];
					
					if (slot._isShowDisplay)
					{
						slot.removeDisplayFromContainer();
					}
				}
				
				for (int i = 0; i < _slotList.Count;  ++i)
				{
					Slot slot = _slotList[i];
					
					if (slot._isShowDisplay)
					{
						slot.addDisplayToContainer(_display, -1);
					}
				}
				
				_slotsZOrderChanged = false;

		}
		
		public virtual void invalidUpdate(){
				for (int i = 0; l = _boneList.Count;  ++i)
				{
					_boneList[i].invalidUpdate();
				}

		}
		public virtual void invalidUpdate(string boneName){
				if (boneName.Length<=0)
				{
					// throw
				}
				
				Bone bone = getBone(boneName);
				
				if (bone)
				{
					bone.invalidUpdate();
				}

		}
		
		public virtual void advanceTime(float passedTime){
				_lockDispose = true;
				_animation.advanceTime(passedTime);
				passedTime *= _animation._timeScale;
				const bool isFading = _animation._isFading;
				
				for (int i = _boneList.Count; i>0; i--;)
				{
					_boneList[i].update(isFading);
				}
				
				for (int i = _slotList.Count; i>0; i--;)
				{
					Slot slot = _slotList[i];
					slot.update();
					
					if (slot._isShowDisplay && slot._childArmature)
					{
						slot._childArmature.advanceTime(passedTime);
					}
				}
				
				if (_slotsZOrderChanged)
				{
					sortSlotsByZOrder();
					
					#if NEED_Z_ORDER_UPDATED_EVENT
					if (_eventDispatcher.hasEvent(EventData.EventType.Z_ORDER_UPDATED))
					{
						EventData eventData = new EventData(EventData.EventType.Z_ORDER_UPDATED, this);
						_eventDataList.Add(eventData);
					}
					#endif
				}
				
				if (!_eventDataList.Count<=0)
				{
					for (int i = 0; i< _eventDataList.Count;  ++i)
					{
						_eventDispatcher.dispatchEvent(_eventDataList[i]);
						EventData.returnObject(_eventDataList[i]);
					}
					
					_eventDataList.Clear();
				}
				
				_lockDispose = false;
				
				if (_delayDispose)
				{
					Dispose();
				}

		}
		
	
		protected	virtual void addObject(object obj){
				Bone bone = obj as Bone;
				Slot slot = obj as Slot;
				
				if (bone)
				{


					Bone iterator = _boneList.Find(delegate (Bone tmp){
						return bone == tmp;


				} );
					
					if (!iterator)
					{
						_boneList.Add(bone);
						sortBones();
						_animation.updateAnimationStates();
					}
				}
				else if (slot)
				{
					Slot iterator = _slotList.Find(delegate (Slot tmp){
						return slot == tmp;
						

					} );

					
					if (!iterator )
					{
						_slotList.Add(slot);
					}
				}

		}
		protected  virtual void removeObject(object obj){
				Bone bone = obj as Bone;
				Slot slot = obj as Slot;
				
				if (bone)
				{
					if(_boneList.IndexOf(bone)>=0)
					{
						_boneList.Remove(iterator);
						_animation.updateAnimationStates();
					}
				}
				else if (slot)
				{
				   if(_slotList.IndexOf(slot)>=0)
					{
					 _slotList.Remove(slot);
					}
				}

		}
		protected virtual void sortBones()
		{
			if (_boneList.Count<=0)
			{
				return;
			}
			
			List< KeyValuePair<int , Bone>> sortedList;
			
			for (int i = 0; i < _boneList.Count;  ++i)
			{
				Bone bone = _boneList[i];
				Bone parentBone = bone;
				int level = 0;
				
				while (parentBone)
				{
					parentBone = parentBone._parent;
					++level;
				}
				
				sortedList.Add(new KeyValuePair(level , bone));
			}
			

			
			sortedList.Sort (sortBone);
			

			
		    for (int i = 0; i < sortedList.Count; ++i)
			{
				_boneList[i] = sortedList[i].Value;
			}
		}
		
		protected virtual void arriveAtFrame(Frame frame, AnimationState animationState, bool isCross)
		{
			if (!frame.evt.length<=0 && _eventDispatcher.hasEvent(EventData.EventType.ANIMATION_FRAME_EVENT))
			{
				EventData eventData = EventData.borrowObject(EventData.EventType.ANIMATION_FRAME_EVENT);
				eventData.armature = this;
				eventData.animationState = animationState;
				eventData.frameLabel = frame.evt;
				eventData.frame = frame;
				_eventDataList.Add(eventData);
			}
			
			if (!frame.sound.length<=0 && soundEventDispatcher && soundEventDispatcher.hasEvent(EventData.EventType.SOUND))
			{
				EventData eventData = EventData.borrowObject(EventData.EventType.SOUND);
				eventData.armature = this;
				eventData.animationState = animationState;
				eventData.sound = frame.sound;
				soundEventDispatcher.dispatchEvent(eventData);
			}
			
			if (!frame.action.length<=0)
			{
				if (animationState.displayControl)
				{
					_animation.gotoAndPlay(frame.action);
				}
			}
		}



	}




}

