#ifndef __SPRITE_SCENE_H__
#define __SPRITE_SCENE_H__

#include "cocos2d.h"

class SpriteScene : public cocos2d::Layer
{
public:
	static cocos2d::Scene* createScene();

	virtual bool init();

	void Update(float eTime);
	void UpdatePerSec(float eTime);

	CREATE_FUNC(SpriteScene);
};

#endif