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
using Com.Viperstudio.Geom;
using Com.Viperstudio.Utils;
namespace DragonBones
{
		public class TimelineState
		{

        private const float HALF_PI = (float)Math.PI * 0.5f;
        private enum  UpdateState {UPDATE, UPDATE_ONCE, UNUPDATE};
		
		private  static List<TimelineState> _pool = new List<TimelineState>();

        public static float GetEaseValue(float value, float easing)
        {
            float valueEase = float.NaN;
            if (easing > 1)
            {
                valueEase = (float)(0.5 * (1 - Math.Cos(value * Math.PI)) - value);
                easing -= 1;
            }
            else if (easing > 0)
            {
                valueEase = (float)Math.Sin(value * HALF_PI) - value;
            }
            else if (easing < 0)
            {
                valueEase = 1 - (float)Math.Cos(value * HALF_PI) - value;
                easing *= -1;
            }
            return valueEase * easing + value;
        }


        public static TimelineState borrowObject()
		{
			if (_pool.Count<=0)
			{
				return new TimelineState();
			}
			
			TimelineState timelinseState = _pool[_pool.Count-1];
			_pool.RemoveAt(_pool.Count-1);
			return timelinseState;
		}
		
		public static void returnObject(TimelineState timelineState)
		{
			if (_pool.IndexOf( timelineState) < 0)
			{
				_pool.Add(timelineState);
			}
			
			timelineState.clear();
		}
		
		public static void clearObjects()
		{
			for (int i = 0; i < _pool.Count; ++i)
			{
				_pool[i].clear();
				//delete _pool[i];
			}
			
			_pool.Clear();
		}
		
	
		public	string name;
		
	
		public 	bool _blendEnabled;
		public bool _isComplete;
		private bool _tweenTransform;
		private bool _tweenScale;
		private bool _tweenColor;
		private int _currentTime;
		private int _currentFrameIndex;
		private int _currentFramePosition;
		private int _currentFrameDuration;
		private int _totalTime;
		public float _weight = 0;
		private float _tweenEasing;
		
		private UpdateState _updateState;
		public DBTransform _transform = new DBTransform();
		private DBTransform _durationTransform = new DBTransform();
		private DBTransform _originTransform = new DBTransform();
		public Point _pivot = new Point();
		private Point _durationPivot = new Point();
		private Point _originPivot = new Point();
		private ColorTransform _durationColor;
		
		private Bone _bone;
		public AnimationState _animationState;
		private TransformTimeline _timeline;


		public TimelineState ()
		{
		}

        
        public void fadeIn(Bone bone, AnimationState animationState, TransformTimeline timeline)
		{
			_bone = bone;
			_animationState = animationState;
			_timeline = timeline;
			_isComplete = false;
			_blendEnabled = false;
			_tweenTransform = false;
			_tweenScale = false;
			_tweenColor = false;
			_currentTime = -1;
			_currentFrameIndex = -1;
			_weight = 1.0f;
			_tweenEasing = DragonBones.USE_FRAME_TWEEN_EASING;
			_totalTime = _timeline.duration;
			name = _timeline.name;
			_transform.X = 0.0f;
			_transform.Y = 0.0f;
			_transform.ScaleX = 0.0f;
			_transform.ScaleY = 0.0f;
			_transform.SkewX = 0.0f;
			_transform.SkewY = 0.0f;
			_pivot.X = 0.0f;
			_pivot.Y = 0.0f;
			_durationTransform.X = 0.0f;
			_durationTransform.Y = 0.0f;
			_durationTransform.ScaleX = 0.0f;
			_durationTransform.ScaleY = 0.0f;
			_durationTransform.SkewX = 0.0f;
			_durationTransform.SkewY = 0.0f;
			_durationPivot.X = 0.0f;
			_durationPivot.Y = 0.0f;
			// copy
			_originTransform.Copy(_timeline.originTransform);
			// copy
			_originPivot.Copy (_timeline.originPivot);
			
			switch (_timeline.frameList.Count)
			{
			case 0:
				_updateState = UpdateState.UNUPDATE;
				break;
				
			case 1:
				_updateState = UpdateState.UPDATE_ONCE;
				break;

			default:
				_updateState = UpdateState.UPDATE;
				break;
			}
			
			_bone.addState(this);
		}

		public void fadeOut()
		{
			_transform.SkewX = DragonBones.formatRadian(_transform.SkewX);
			_transform.SkewY = DragonBones.formatRadian(_transform.SkewY);
		}

		public void update(float progress)
		{
			if (_updateState == UpdateState.UPDATE)
			{
				updateMultipleFrame(progress);
			}
			else if (_updateState == UpdateState.UPDATE_ONCE)
			{
				updateSingleFrame();
				_updateState = UpdateState.UNUPDATE;
			}
		}
		
		public void updateMultipleFrame(float progress)
		{
			progress /= _timeline.scale;
			progress += _timeline.offset;
            //Logger.Log(progress);
			int currentTime = (int)(_totalTime * progress);
			int currentPlayTimes = 0;
			int playTimes = _animationState.getPlayTimes();
			
			if (playTimes == 0)
			{
				_isComplete = false;
				currentPlayTimes = (int)(Math.Ceiling(Math.Abs(currentTime) / (float)(_totalTime)));
				currentTime -= (int)(Math.Floor(currentTime / (float)(_totalTime))) * _totalTime;
				
				if (currentTime < 0)
				{
					currentTime += _totalTime;
				}
			}
			else
			{
				int totalTimes = playTimes * _totalTime;
				
				if (currentTime >= totalTimes)
				{
					currentTime = totalTimes;
					_isComplete = true;
				}
				else if (currentTime <= -totalTimes)
				{
					currentTime = -totalTimes;
					_isComplete = true;
				}
				else
				{
					_isComplete = false;
				}
				
				if (currentTime < 0)
				{
					currentTime += totalTimes;
				}
				
				currentPlayTimes = (int)(Math.Ceiling(currentTime / (float)(_totalTime)));
				
				if (_isComplete)
				{
					currentTime = _totalTime;
				}
				else
				{
					currentTime -= (int)(Math.Floor(currentTime / (float)(_totalTime))) * _totalTime;
				}
			}
			
			if (currentPlayTimes == 0)
			{
				currentPlayTimes = 1;
			}
			
			if (_currentTime != currentTime)
			{
				_currentTime = currentTime;

 				TransformFrame prevFrame = null;
				TransformFrame currentFrame = null;
				
				for (int i = 0; i < _timeline.frameList.Count; ++i)
				{
					if (_currentFrameIndex < 0)
					{
						_currentFrameIndex = 0;
					}
					else if (_currentTime < _currentFramePosition || _currentTime >= _currentFramePosition + _currentFrameDuration)
					{
						++_currentFrameIndex;
						
						if (_currentFrameIndex >= (int)(_timeline.frameList.Count))
						{
							if (_isComplete)
							{
								--_currentFrameIndex;
								break;
							}
							else
							{
								_currentFrameIndex = 0;
							}
						}
					}
					else
					{
						break;
					}

                  

                    currentFrame = (TransformFrame)(_timeline.frameList[_currentFrameIndex]);
					
					if (prevFrame!=null)
					{

						_bone.arriveAtFrame(prevFrame, this, _animationState, true);
					}
					
					_currentFrameDuration = currentFrame.duration;
					_currentFramePosition = currentFrame.position;

                   
					prevFrame = currentFrame;
				}
				
				if (currentFrame!=null)
				{
					_bone.arriveAtFrame(currentFrame, this, _animationState, false);
					_blendEnabled = currentFrame.displayIndex >= 0;
					
					if (_blendEnabled)
					{
						updateToNextFrame(currentPlayTimes);
                        
					}
					else
					{
						_tweenEasing = DragonBones.NO_TWEEN_EASING;
						_tweenTransform = false;
						_tweenScale = false;
						_tweenColor = false;
					}
				}
				
				if (_blendEnabled)
				{
					updateTween();
				}
			}
		}


		public void updateToNextFrame(int currentPlayTimes)
		{
			bool tweenEnabled = false;
			int nextFrameIndex = _currentFrameIndex + 1;
			
			if (nextFrameIndex >= (int)(_timeline.frameList.Count))
			{
				nextFrameIndex = 0;
			}
			
			 TransformFrame currentFrame = (TransformFrame)(_timeline.frameList[_currentFrameIndex]);
			 TransformFrame nextFrame = (TransformFrame)(_timeline.frameList[nextFrameIndex]);
			
			if (
				nextFrameIndex == 0 &&
				(
				!_animationState.lastFrameAutoTween ||
				(
				_animationState.getPlayTimes()!=0 &&
				_animationState.getCurrentPlayTimes() >= _animationState.getPlayTimes() &&
				((_currentFramePosition + _currentFrameDuration) / _totalTime + currentPlayTimes - _timeline.offset) * _timeline.scale > 0.999999f
				)
				)
				)
			{
				_tweenEasing = DragonBones.NO_TWEEN_EASING;
				tweenEnabled = false;
			}
			else if (currentFrame.displayIndex < 0 || nextFrame.displayIndex < 0)
			{
				_tweenEasing = DragonBones.NO_TWEEN_EASING;
				tweenEnabled = false;
			}
			else if (_animationState.autoTween)
			{
				_tweenEasing = _animationState.getClip().tweenEasing;
				
				if (_tweenEasing == DragonBones.USE_FRAME_TWEEN_EASING)
				{
					_tweenEasing = currentFrame.tweenEasing;
					
					if (_tweenEasing == DragonBones.NO_TWEEN_EASING)    // frame no tween
					{
						tweenEnabled = false;
					}
					else
					{
						if (_tweenEasing == DragonBones.AUTO_TWEEN_EASING)
						{
							_tweenEasing = 0.0f;
						}
						
						// _tweenEasing [-1, 0) 0 (0, 1] (1, 2]
						tweenEnabled = true;
					}
				}
				else    // animationData overwrite tween
				{
					// _tweenEasing [-1, 0) 0 (0, 1] (1, 2]
					tweenEnabled = true;
				}
			}
			else
			{
				_tweenEasing = currentFrame.tweenEasing;

				if (_tweenEasing == DragonBones.NO_TWEEN_EASING || _tweenEasing == DragonBones.AUTO_TWEEN_EASING)    // frame no tween
				{
					_tweenEasing = DragonBones.NO_TWEEN_EASING;
					tweenEnabled = false;
				}
				else
				{
					// _tweenEasing [-1, 0) 0 (0, 1] (1, 2]
					tweenEnabled = true;
				}
			}
			
			if (tweenEnabled)
			{
				// transform
				_durationTransform.X = nextFrame.transform.X - currentFrame.transform.X;
				_durationTransform.Y = nextFrame.transform.Y - currentFrame.transform.Y;
				_durationTransform.SkewX = nextFrame.transform.SkewX - currentFrame.transform.SkewX;
				_durationTransform.SkewY = nextFrame.transform.SkewY - currentFrame.transform.SkewY;
				_durationTransform.ScaleX = nextFrame.transform.ScaleX - currentFrame.transform.ScaleX + nextFrame.scaleOffset.X;
				_durationTransform.ScaleY = nextFrame.transform.ScaleY - currentFrame.transform.ScaleY + nextFrame.scaleOffset.Y;
				
				if (nextFrameIndex == 0)
				{
					_durationTransform.SkewX = DragonBones.formatRadian(_durationTransform.SkewX);
					_durationTransform.SkewY = DragonBones.formatRadian(_durationTransform.SkewY);
				}
				
				_durationPivot.X = nextFrame.pivot.X - currentFrame.pivot.X;
				_durationPivot.Y = nextFrame.pivot.Y - currentFrame.pivot.Y;
				
				if (
					_durationTransform.X!=0 ||
					_durationTransform.Y!=0 ||
					_durationTransform.SkewX!=0 ||
					_durationTransform.SkewY!=0 ||
					_durationTransform.ScaleX!=0 ||
					_durationTransform.ScaleY!=0 ||
					_durationPivot.X!=0 ||
					_durationPivot.Y!=0
					)
				{
					_tweenTransform = true;
					_tweenScale = currentFrame.tweenScale;
				}
				else
				{
					_tweenTransform = false;
					_tweenScale = false;
				}
				
				// color
				if (currentFrame.color!=null && nextFrame.color!=null)
				{
					_durationColor.AlphaOffset = nextFrame.color.AlphaOffset - currentFrame.color.AlphaOffset;
					_durationColor.RedOffset = nextFrame.color.RedOffset - currentFrame.color.RedOffset;
					_durationColor.GreenOffset = nextFrame.color.GreenOffset - currentFrame.color.GreenOffset;
					_durationColor.BlueOffset = nextFrame.color.BlueOffset - currentFrame.color.BlueOffset;
					_durationColor.AlphaMultiplier = nextFrame.color.AlphaMultiplier - currentFrame.color.AlphaMultiplier;
					_durationColor.RedMultiplier = nextFrame.color.RedMultiplier - currentFrame.color.RedMultiplier;
					_durationColor.GreenMultiplier = nextFrame.color.GreenMultiplier - currentFrame.color.GreenMultiplier;
					_durationColor.BlueMultiplier = nextFrame.color.BlueMultiplier - currentFrame.color.BlueMultiplier;
					
					if (
						_durationColor.AlphaOffset!=0 ||
						_durationColor.RedOffset!=0 ||
						_durationColor.GreenOffset!=0 ||
						_durationColor.BlueOffset!=0 ||
						_durationColor.AlphaMultiplier!=0 ||
						_durationColor.RedMultiplier!=0 ||
						_durationColor.GreenMultiplier!=0 ||
						_durationColor.BlueMultiplier!=0
						)
					{
						_tweenColor = true;
					}
					else
					{
						_tweenColor = false;
					}
				}
				else if (currentFrame.color!=null)
				{
					_tweenColor = true;
					_durationColor.AlphaOffset = -currentFrame.color.AlphaOffset;
					_durationColor.RedOffset = -currentFrame.color.RedOffset;
					_durationColor.GreenOffset = -currentFrame.color.GreenOffset;
					_durationColor.BlueOffset = -currentFrame.color.BlueOffset;
					_durationColor.AlphaMultiplier = 1 - currentFrame.color.AlphaMultiplier;
					_durationColor.RedMultiplier = 1 - currentFrame.color.RedMultiplier;
					_durationColor.GreenMultiplier = 1 - currentFrame.color.GreenMultiplier;
					_durationColor.BlueMultiplier = 1 - currentFrame.color.BlueMultiplier;
				}
				else if (nextFrame.color!=null)
				{
					_tweenColor = true;
					_durationColor.AlphaOffset = nextFrame.color.AlphaOffset;
					_durationColor.RedOffset = nextFrame.color.RedOffset;
					_durationColor.GreenOffset = nextFrame.color.GreenOffset;
					_durationColor.BlueOffset = nextFrame.color.BlueOffset;
					_durationColor.AlphaMultiplier = nextFrame.color.AlphaMultiplier - 1;
					_durationColor.RedMultiplier = nextFrame.color.RedMultiplier - 1;
					_durationColor.GreenMultiplier = nextFrame.color.GreenMultiplier - 1;
					_durationColor.BlueMultiplier = nextFrame.color.BlueMultiplier - 1;
				}
				else
				{
					_tweenColor = false;
				}
			}
			else
			{
				_tweenTransform = false;
				_tweenScale = false;
				_tweenColor = false;
			}
			
			if (!_tweenTransform)
			{
				if (_animationState.additiveBlending)
				{
					_transform.X = currentFrame.transform.X;
					_transform.Y = currentFrame.transform.Y;
					_transform.SkewX = currentFrame.transform.SkewX;
					_transform.SkewY = currentFrame.transform.SkewY;
					_transform.ScaleX = currentFrame.transform.ScaleX;
					_transform.ScaleY = currentFrame.transform.ScaleY;
					_pivot.X = currentFrame.pivot.X;
					_pivot.Y = currentFrame.pivot.Y;
				}
				else
				{
					_transform.X = _originTransform.X + currentFrame.transform.X;
					_transform.Y = _originTransform.Y + currentFrame.transform.Y;
					_transform.SkewX = _originTransform.SkewX + currentFrame.transform.SkewX;
					_transform.SkewY = _originTransform.SkewY + currentFrame.transform.SkewY;
					_transform.ScaleX = _originTransform.ScaleX + currentFrame.transform.ScaleX;
					_transform.ScaleY = _originTransform.ScaleY + currentFrame.transform.ScaleY;
					_pivot.X = _originPivot.X + currentFrame.pivot.X;
					_pivot.Y = _originPivot.Y + currentFrame.pivot.Y;
				}
				
				_bone.invalidUpdate();
			}
			else if (!_tweenScale)
			{
				if (_animationState.additiveBlending)
				{
					_transform.ScaleX = currentFrame.transform.ScaleX;
					_transform.ScaleY = currentFrame.transform.ScaleY;
				}
				else
				{
					_transform.ScaleX = _originTransform.ScaleX + currentFrame.transform.ScaleX;
					_transform.ScaleY = _originTransform.ScaleY + currentFrame.transform.ScaleY;
				}
			}
			
			if (!_tweenColor && _animationState.displayControl)
			{
				if (currentFrame.color!=null)
				{
					_bone.updateColor(
						currentFrame.color.AlphaOffset,
						currentFrame.color.RedOffset,
						currentFrame.color.GreenOffset,
						currentFrame.color.BlueOffset,
						currentFrame.color.AlphaMultiplier,
						currentFrame.color.RedMultiplier,
						currentFrame.color.GreenMultiplier,
						currentFrame.color.BlueMultiplier,
						true
						);
				}
				else if (_bone._isColorChanged)
				{

					_bone.updateColor(0, 0, 0, 0, 1.0f, 1.0f, 1.0f, 1.0f, false);
				}
			}
		}

		public void updateTween()
		{
			float progress = (_currentTime - _currentFramePosition) / (float)(_currentFrameDuration);
                   
            if (_tweenEasing!=0 && _tweenEasing != DragonBones.NO_TWEEN_EASING)
			{
				progress = DragonBones.getEaseValue(progress, _tweenEasing);
			}
			
			 TransformFrame currentFrame = (TransformFrame)(_timeline.frameList[_currentFrameIndex]);

			if (_tweenTransform)
			{
				DBTransform currentTransform = currentFrame.transform;
				Point currentPivot = currentFrame.pivot;
				
				if (_animationState.additiveBlending)
				{
					//additive blending
					_transform.X = currentTransform.X + _durationTransform.X * progress;
					_transform.Y = currentTransform.Y + _durationTransform.Y * progress;
					_transform.SkewX = currentTransform.SkewX + _durationTransform.SkewX * progress;
					_transform.SkewY = currentTransform.SkewY + _durationTransform.SkewY * progress;
					
					if (_tweenScale)
					{
						_transform.ScaleX = currentTransform.ScaleX + _durationTransform.ScaleX * progress;
						_transform.ScaleY = currentTransform.ScaleY + _durationTransform.ScaleY * progress;
					}
					
					_pivot.X = currentPivot.X + _durationPivot.X * progress;
					_pivot.Y = currentPivot.Y + _durationPivot.Y * progress;
				}
				else
				{
					// normal blending
					_transform.X = _originTransform.X + currentTransform.X + _durationTransform.X * progress;
					_transform.Y = _originTransform.Y + currentTransform.Y + _durationTransform.Y * progress;
					_transform.SkewX = _originTransform.SkewX + currentTransform.SkewX + _durationTransform.SkewX * progress;
					_transform.SkewY = _originTransform.SkewY + currentTransform.SkewY + _durationTransform.SkewY * progress;

                    //if(_timeline.name == "man-weapon")
                    //  Logger.Log(currentTransform.SkewX + " " + _durationTransform.SkewX + "  " + progress);

                    if (_tweenScale)
					{
						_transform.ScaleX = _originTransform.ScaleX + currentTransform.ScaleX + _durationTransform.ScaleX * progress;
						_transform.ScaleY = _originTransform.ScaleY + currentTransform.ScaleY + _durationTransform.ScaleY * progress;
					}

					_pivot.X = _originPivot.X + currentPivot.X + _durationPivot.X * progress;
					_pivot.Y = _originPivot.Y + currentPivot.Y + _durationPivot.Y * progress;
				}
				
				_bone.invalidUpdate();
			}
			
			if (_tweenColor && _animationState.displayControl)
			{
				if (currentFrame.color!=null)
				{
					_bone.updateColor(
						(int)(currentFrame.color.AlphaOffset + _durationColor.AlphaOffset * progress),
						(int)(currentFrame.color.RedOffset + _durationColor.RedOffset * progress),
						(int)(currentFrame.color.GreenOffset + _durationColor.GreenOffset * progress),
						(int)(currentFrame.color.BlueOffset + _durationColor.BlueOffset * progress),
						currentFrame.color.AlphaMultiplier + _durationColor.AlphaMultiplier * progress,
						currentFrame.color.RedMultiplier + _durationColor.RedMultiplier * progress,
						currentFrame.color.GreenMultiplier + _durationColor.GreenMultiplier * progress,
						currentFrame.color.BlueMultiplier + _durationColor.BlueMultiplier * progress,
						true
						);
				}
				else
				{
					_bone.updateColor(
						(int)(_durationColor.AlphaOffset * progress),
						(int)(_durationColor.RedOffset * progress),
						(int)(_durationColor.GreenOffset * progress),
						(int)(_durationColor.BlueOffset * progress),
						1.0f + _durationColor.AlphaMultiplier * progress,
						1.0f + _durationColor.RedMultiplier * progress,
						1.0f + _durationColor.GreenMultiplier * progress,
						1.0f + _durationColor.BlueMultiplier * progress,
						true
						);
				}
			}


        }


        public void updateSingleFrame()
		{
			TransformFrame currentFrame = (TransformFrame)_timeline.frameList[0];
			_bone.arriveAtFrame(currentFrame, this, _animationState, false);
			_isComplete = true;
			_tweenTransform = false;
			_tweenScale = false;
			_tweenColor = false;
			_tweenEasing = DragonBones.NO_TWEEN_EASING;
			_blendEnabled = currentFrame.displayIndex >= 0;
			
			if (_blendEnabled)
			{
				if (_animationState.additiveBlending)
				{
					// additive blending
					// singleFrame.transform (0)
					_transform.X =
						_transform.Y =
							_transform.SkewX =
							_transform.SkewY =
							_transform.ScaleX =
							_transform.ScaleY = 0.0f;
					_pivot.X =
						_pivot.Y = 0.0f;
				}
				else
				{
					// normal blending
					// timeline.originTransform + singleFrame.transform (0)
					// copy
					_transform.Copy( _originTransform);
					// copy
					_pivot.Copy( _originPivot);
				}

				_bone.invalidUpdate();
				
				if (_animationState.displayControl)
				{
					if (currentFrame.color!= null)
					{
						_bone.updateColor(
							currentFrame.color.AlphaOffset,
							currentFrame.color.RedOffset,
							currentFrame.color.GreenOffset,
							currentFrame.color.BlueOffset,
							currentFrame.color.AlphaMultiplier,
							currentFrame.color.RedMultiplier,
							currentFrame.color.GreenMultiplier,
							currentFrame.color.BlueMultiplier,
							true
							);
					}
					else if (_bone._isColorChanged)
					{
						_bone.updateColor(0, 0, 0, 0, 1.0f, 1.0f, 1.0f, 1.0f, false);
					}
				}
			}
		}


		private void clear(){
			if(_bone!=null)
			{
				_bone.removeState(this);
				_bone = null;
			}
			
			_animationState = null;
			_timeline = null;

		}
}
}

