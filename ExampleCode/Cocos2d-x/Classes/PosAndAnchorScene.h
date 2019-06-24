#ifndef __POSANDANCHOR_SCENE_H__
#define __POSANDANCHOR_SCENE_H__

#include "cocos2d.h"

class PosAndAnchorScene : public cocos2d::Layer
{
public:
	static cocos2d::Scene* createScene();

	virtual bool init();

	void Update(float eTime);
	void UpdatePerSec(float eTime);

	CREATE_FUNC(PosAndAnchorScene);
};

#endif

