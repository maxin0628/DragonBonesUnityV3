﻿
using System.Collections.Generic;

namespace DragonBones
{

	public class AnimationData: Timeline  {

	    public	bool autoTween;
		public	int frameRate;
		public	int playTimes;
		public	float fadeTime;
		// use frame tweenEase, NaN
		// overwrite frame tweenEase, [-1, 0):ease in, 0:line easing, (0, 1]:ease out, (1, 2]:ease in out
		public	float tweenEasing;
		
		public string name;
		public List<TransformTimeline> timelineList;
		public List<string> hideTimelineList;

		public TransformTimeline getTimeline(string timelineName)
		{
			for (int i = 0, l = timelineList.Count; i < l; ++i)
			{
				if (timelineList[i].name == timelineName)
				{
					return timelineList[i];
				}
			}
			
			return null;

		}

		public virtual void dispose()
		{
			base.dispose();
			_dispose();
		}

		private void _dispose()
		{
			for (int i = 0; i < timelineList.Count; ++i)
			{
				timelineList[i].dispose();
				//delete timelineList[i];
			}
			
			timelineList.Clear();
			hideTimelineList.Clear();
		}




	}

}