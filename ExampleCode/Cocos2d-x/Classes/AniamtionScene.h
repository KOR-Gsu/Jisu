#ifndef __ANIMATIONSCENE_SCENE_H__
#define __ANIMATIONSCENE_SCENE_H__

#include "cocos2d.h"

USING_NS_CC;

class AniamtionScene : public cocos2d::Layer
{
private:

public:
	static cocos2d::Scene* createScene();

	virtual bool init();
	void CallBackFunc();

	CREATE_FUNC(AniamtionScene);
};

#endif // __ANIMATIONSCENE_SCENE_H__